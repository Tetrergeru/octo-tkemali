using System;
using System.Collections.Generic;
using UnityEditor;
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
                case Answer ea:
                    {
                        var node = new AnswerNode(this, topic.Position, topic.Id);
                        node.Text = ea.Text;
                        inputs[topic.Id] = node.Input;
                        foreach (var nextTopicId in ea.NextTopicIds)
                            edgeRequests.Add((node.Output, nextTopicId));
                        if (ea.Action != null && ea.Action != "")
                            edgeRequests.Add((node.Action, ea.Action));
                        AddElement(node);
                        break;
                    }
                case Question q:
                    {
                        var node = new QuestionNode(this, topic.Position, topic.Id);
                        node.Text = q.Text;
                        inputs[topic.Id] = node.Input;
                        if (q.AnswerId != null && q.AnswerId != "")
                            edgeRequests.Add((node.Output, q.AnswerId));
                        AddElement(node);
                        break;
                    }
                case AnswerSwitch c:
                    {
                        var node = new AnswerSwitchNode(this, topic.Position, topic.Id);
                        foreach (var @case in c.Cases)
                        {
                            var port = node.MakeCondition(@case.Condition);
                            inputs[topic.Id] = port;
                            edgeRequests.Add((port, @case.NextId));
                        }
                        edgeRequests.Add((node.Other, c.Default.NextId));
                        inputs[topic.Id] = node.Input;
                        AddElement(node);
                        break;
                    }
                case DialogAction a:
                    {
                        var node = new ActionNode(this, topic.Position, topic.Id);
                        node.Text = a.Script;
                        inputs[topic.Id] = node.Input;
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
        dialog.Persist();
    }

    public void AddAnswerNode()
    {
        this.AddElement(new AnswerNode(this, NodeSpawnpoint()));
    }

    public void AddQuestionNode()
    {
        this.AddElement(new QuestionNode(this, NodeSpawnpoint()));
    }

    public void AddActionNode()
    {
        this.AddElement(new ActionNode(this, NodeSpawnpoint()));
    }

    public void AddConditionNode()
    {
        this.AddElement(new AnswerSwitchNode(this, NodeSpawnpoint()));
    }

    private Vector2 NodeSpawnpoint()
        => -(Vector2)this.viewTransform.position + new Vector2(30, 30);

    private AnswerNode AddNodeWithParams(Vector2 position, string id)
        => new AnswerNode(this, position, id);

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        foreach (var port in ports)
        {
            if (IsCompatible(port, startPort))
                compatiblePorts.Add(port);
        }

        return compatiblePorts;
    }

    private bool IsCompatible(Port startPort, Port otherPort)
    {
        return startPort != otherPort
            && startPort.node != otherPort.node
            && startPort.portType == otherPort.portType
            && (
                IsFromTo<QuestionNode, AnswerNode>(startPort, otherPort)
                || IsFromTo<QuestionNode, AnswerSwitchNode>(startPort, otherPort)
                || IsFromTo<AnswerSwitchNode, AnswerNode>(startPort, otherPort)
                || IsFromTo<AnswerNode, QuestionNode>(startPort, otherPort)
                || IsFromTo<AnswerNode, ActionNode>(startPort, otherPort)
            )
            ;
    }

    private static bool IsFromTo<From, To>(Port fst, Port snd)
    where From : DialogNode
    where To : DialogNode
    {
        var from = fst.direction == Direction.Output ? fst : snd;
        var to = fst.direction == Direction.Input ? fst : snd;

        if (from == to) return false;

        return from.node is From && to.node is To;
    }
}
