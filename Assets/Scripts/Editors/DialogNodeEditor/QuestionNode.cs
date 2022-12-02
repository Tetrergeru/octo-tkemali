
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionNode : DialogNode
{
    private TextField _textField;
    public Port Output;
    public Port Input;

    public string Text
    {
        get => _textField.value;
        set
        {
            _textField.value = value;
        }
    }

    public string GetOutput()
    {
        var nextNode = (DialogNode)Output.connections.FirstOrDefault()?.input.node;
        return nextNode?.Id;
    }

    public override ITopic Save()
    {
        var topic = new Question();
        topic.PropId = this.Id;
        topic.Text = this.Text;
        topic.PropPosition = this.GetPosition().position;

        topic.AnswerId = GetOutput();

        return topic;
    }

    public QuestionNode(DialogGraphView parent) : this(parent, new Vector2(), Guid.NewGuid().ToString())
    {
    }

    public QuestionNode(DialogGraphView parent, Vector2 position, string id) : base(parent, position, id)
    {
        var color = new Color(0.4f, 0.4f, 0.7f);
        this.title = "Question";
        this.titleContainer.style.backgroundColor = new StyleColor(color);

        Input = this.MakePort(Direction.Input);
        Input.portName = "";
        this.inputContainer.Add(Input);

        Output = this.MakePort(Direction.Output);
        Output.portName = "";
        this.outputContainer.Add(Output);

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