using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionNode : DialogNode
{
    public Port Input;

    private TextField _textField;

    public string Text
    {
        get => _textField.value;
        set
        {
            _textField.value = value;
        }
    }

    public override Topic Save()
    {
        var topic = new DialogAction();

        topic.Id = this.Id;
        topic.Script = this.Text;
        topic.Position = this.GetPosition().position;

        return topic;
    }

    public ActionNode(DialogGraphView parent, Vector2 position = new Vector2()) : this(parent, position, Guid.NewGuid().ToString())
    {
    }

    public ActionNode(DialogGraphView parent, Vector2 position, string id) : base(parent, position, id)
    {
        var color = new Color(0.7f, 0.4f, 0.1f);
        this.title = "Action";
        this.titleContainer.style.backgroundColor = new StyleColor(color);

        Input = this.MakePort<string>(Direction.Input);
        Input.portName = "Input";
        this.inputContainer.Add(Input);

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