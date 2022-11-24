using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : Node
{
    public string Id;
    private DialogGraphView _parent;
    private TextField _answerField;

    public string Answer
    {
        get => _answerField.value;
        set
        {
            _answerField.value = value;
        }
    }

    public List<(string text, string id)> GetOutputs()
    {
        var list = new List<(string, string)>();
        this.outputContainer.Query("block").ForEach(node => {
            var text = node.Query<TextField>().First();
            var output = node.Query<Port>().First();
            var nextNode = (DialogNode)output.connections.FirstOrDefault()?.input.node;
            list.Add((text.value, nextNode.Id));
        });
        return list;
    }

    public DialogNode(DialogGraphView parent) : this(parent, new Vector2(), Guid.NewGuid().ToString()) { }

    public DialogNode(DialogGraphView parent, Vector2 position, string id)
    {
        _parent = parent;
        this.title = "Stage";
        this.Id = id;

        this.SetPosition(new Rect(position.x, position.y, 100, 200));

        var port = this.MakePort(Direction.Input);
        port.portName = "Input";
        this.inputContainer.Add(port);

        var button = new Button(() => this.MakeOutput());
        button.text = "Add Output";
        this.titleContainer.Add(button);

        var textField = new TextField
        {
            name = "",
            value = "Answer",
            multiline = true,
        };
        textField.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
        textField.style.maxWidth = new StyleLength(250);
        _answerField = textField;
        this.extensionContainer.Add(textField);

        this.RefreshExpandedState();
        this.RefreshPorts();

        parent.AddElement(this);
    }

    private Port MakePort(Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return this.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(bool));
    }

    public Port MakeOutput(string text = "")
    {
        var element = new VisualElement{
            name = "block",
        };
        element.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

        var output = MakePort(Direction.Output);

        var count = 0;
        this.outputContainer.Query("connector").ForEach((_) => count++);
        output.portName = "";
        output.name = $"Output {count}";

        var textField = new TextField
        {
            name = "",
            value = text,
            multiline = true,
        };
        textField.RegisterValueChangedCallback(e => output.name = e.newValue);
        textField.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
        textField.style.maxWidth = new StyleLength(100);
        this.contentContainer.Add(textField);

        var button = new Button(() =>
        {
            _parent.DeleteElements(output.connections);
            this.outputContainer.Remove(element);
        });
        button.text = "x";

        element.Add(button);
        element.Add(textField);
        element.Add(output);

        this.outputContainer.Add(element);

        this.RefreshExpandedState();
        this.RefreshPorts();

        return output;
    }
}
