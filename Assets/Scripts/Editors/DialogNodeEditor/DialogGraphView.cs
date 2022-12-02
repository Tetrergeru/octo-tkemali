using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
{
    public DialogGraphView()
    {
        this.styleSheets.Add(Resources.Load<StyleSheet>("DialogGraph"));

        this.SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
    }

    public void Load(Dialog dialog)
    {
        var inputs = new Dictionary<string, Port>();
        var edgeRequests = new List<(Port output, string inputId)>();
        foreach (var topic in dialog.Topics)
        {
            switch (topic)
            {
                case ExactAnswer ea:
                    {
                        var node = new AnswerNode(this, topic.PropPosition, topic.PropId);
                        node.Text = ea.Text;
                        inputs[topic.PropId] = node.Input;
                        foreach (var nextTopicId in ea.NextTopicIds)
                            edgeRequests.Add((node.Output, nextTopicId));
                        if (ea.Action != null && ea.Action != "")
                            edgeRequests.Add((node.Action, ea.Action));
                        AddElement(node);
                        break;
                    }
                case Question q:
                    {
                        var node = new QuestionNode(this, topic.PropPosition, topic.PropId);
                        node.Text = q.Text;
                        inputs[topic.PropId] = node.Input;
                        edgeRequests.Add((node.Output, q.AnswerId));
                        AddElement(node);
                        break;
                    }
                case Condition c:
                    {
                        var node = new ConditionNode(this, topic.PropPosition, topic.PropId);
                        foreach (var @case in c.Cases)
                        {
                            var port = node.MakeCondition(@case.Condition);
                            inputs[topic.PropId] = port;
                            edgeRequests.Add((port, @case.NextId));
                        }
                        edgeRequests.Add((node.Other, c.Default.NextId));
                        inputs[topic.PropId] = node.Input;
                        AddElement(node);
                        break;
                    }
                case DialogAction a:
                    {
                        var node = new ActionNode(this, topic.PropPosition, topic.PropId);
                        node.Text = a.Script;
                        inputs[topic.PropId] = node.Input;
                        AddElement(node);
                        break;
                    }
            }
        }
        foreach (var (output, inputId) in edgeRequests)
        {
            var edge = output.ConnectTo(inputs[inputId]);
            AddElement(edge);
        }
    }

    public void Save(Dialog dialog)
    {
        dialog.Clear();
        this.Query<DialogNode>().ForEach(node => dialog.Add(node.Save()));
    }

    public void AddAnswerNode()
    {
        this.AddElement(new AnswerNode(this));
    }

    public void AddQuestionNode()
    {
        this.AddElement(new QuestionNode(this));
    }

    public void AddActionNode()
    {
        this.AddElement(new ActionNode(this));
    }

    public void AddConditionNode()
    {
        this.AddElement(new ConditionNode(this));
    }

    private AnswerNode AddNodeWithParams(Vector2 position, string id)
    {
        return new AnswerNode(this, position, id);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        foreach (var port in ports)
        {
            if (port != startPort && port.node != startPort.node)
                compatiblePorts.Add(port);
        }

        return compatiblePorts;
    }
}
