
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class AnswerNode : DialogNode
{
    public Port Input;
    public Port Output;
    public Port Action;
    private TextField _textField;

    public string Text
    {
        get => _textField.value;
        set
        {
            _textField.value = value;
        }
    }

    public List<string> GetOutputs()
    {
        var list = new List<string>();
        foreach (var connection in Output.connections)
        {
            var node = (DialogNode)connection.input.node;
            list.Add(node.Id);
        }
        return list;
    }


    public string GetAction()
    {
        var nextNode = (DialogNode)Action.connections.FirstOrDefault()?.input.node;
        return nextNode?.Id;
    }

    public override ITopic Save()
    {
        var topic = new ExactAnswer();
        topic.Id = this.Id;
        topic.Text = this.Text;
        topic.Position = this.GetPosition().position;
        topic.Action = GetAction();

        foreach (var id in this.GetOutputs())
            topic.NextTopicIds.Add(id);

        return topic;
    }

    public AnswerNode(DialogGraphView parent, Vector2 position = new Vector2()) : this(parent, position, Guid.NewGuid().ToString())
    {
    }

    public AnswerNode(DialogGraphView parent, Vector2 position, string id) : base(parent, position, id)
    {
        var color = new Color(0.4f, 0.4f, 0.4f);
        this.title = "Answer";
        this.titleContainer.style.backgroundColor = new StyleColor(color);

        Input = this.MakePort(Direction.Input);
        Input.portName = "";
        this.inputContainer.Add(Input);

        Output = this.MakePort(Direction.Output, Port.Capacity.Multi);
        Output.portName = "";
        this.outputContainer.Add(Output);

        Action = this.MakePort<string>(Direction.Output);
        Action.portName = "Action";
        this.outputContainer.Add(Action);

        _textField = new TextField
        {
            name = "",
            value = "",
            multiline = true,
        };
        _textField.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
        _textField.style.maxWidth = new StyleLength(200);
        _textField.style.width = new StyleLength(200);
        this.mainContainer.style.backgroundColor = new StyleColor(color);
        this.mainContainer.Add(_textField);

        this.RefreshExpandedState();
        this.RefreshPorts();
    }
}