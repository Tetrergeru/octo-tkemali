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
        if (dialog.Topics == null)
        {
            dialog.Topics = new List<Topic>();
        }

        var inputs = new Dictionary<string, (Port, DialogNode)>();
        foreach (var topic in dialog.Topics)
        {
            var node = AddNodeWithParams(topic.Position, topic.Id);
            var port = node.inputContainer.Query<Port>().First();
            node.Answer = topic.Answer;

            inputs[topic.Id] = (port, node);
        }

        foreach (var topic in dialog.Topics)
        {
            var (_, node) = inputs[topic.Id];
            foreach (var question in topic.Questions)
            {
                var output = node.MakeOutput(question.Text);
                var (input, _) = inputs[question.NextTopicId];

                var edge = input.ConnectTo(output);
                AddElement(edge);

                node.RefreshExpandedState();
                node.RefreshPorts();
            }
        }
    }

    public void Save(Dialog dialog)
    {
        dialog.Topics = new List<Topic>();
        this.Query<DialogNode>().ForEach(node =>
        {
            var topic = new Topic();
            topic.Id = node.Id;
            topic.Answer = node.Answer;
            topic.Position = node.GetPosition().position;

            foreach (var (text, id) in node.GetOutputs())
                topic.Questions.Add(new Topic.Question { Text = text, NextTopicId = id });

            dialog.Topics.Add(topic);
        });
    }

    public void AddNode()
    {
        new DialogNode(this);
    }

    private DialogNode AddNodeWithParams(Vector2 position, string id)
    {
        return new DialogNode(this, position, id);
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
