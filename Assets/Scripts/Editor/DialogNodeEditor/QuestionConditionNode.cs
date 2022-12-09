using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionConditionNode : DialogNode
{
    public Port Output;

    private TextField _textField;

    public string Text
    {
        get => _textField.value;
        set
        {
            _textField.value = value;
        }
    }

    public string GetQuestion()
    {
        var nextNode = (DialogNode)Output.connections.FirstOrDefault()?.input.node;
        return nextNode?.Id;
    }

    public override Topic Save()
    {
        var topic = new QuestionCondition();

        topic.Id = this.Id;
        topic.Condition = this.Text;
        topic.QuestionId = GetQuestion();
        topic.Position = this.GetPosition().position;

        return topic;
    }

    public QuestionConditionNode(DialogGraphView parent, Vector2 position = new Vector2()) : this(parent, position, Guid.NewGuid().ToString())
    {
    }

    public QuestionConditionNode(DialogGraphView parent, Vector2 position, string id) : base(parent, position, id)
    {
        var color = new Color(0.5f, 0.7f, 0.5f);
        this.title = "IF";
        this.titleContainer.style.color = new StyleColor(Color.black);
        this.titleContainer.style.backgroundColor = new StyleColor(color);

        Output = this.MakePort<string>(Direction.Output);
        Output.portName = "Input";
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