
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ConditionNode : DialogNode
{
    public Port Input;
    public Port Other;

    public List<(string text, string id)> GetOutputs()
    {
        var list = new List<(string, string)>();
        this.outputContainer.Query("block").ForEach(node =>
        {
            var text = node.Query<TextField>().First();
            var output = node.Query<Port>().First();
            var nextNode = (DialogNode)output.connections.FirstOrDefault()?.input.node;
            list.Add((text.value, nextNode.Id));
        });
        return list;
    }

    public override ITopic Save()
    {
        var topic = new Condition();
        topic.Id = this.Id;
        topic.Position = this.GetPosition().position;
        topic.Cases = GetOutputs()
            .Select(it => new Condition.Case { Condition = it.text, NextId = it.id })
            .ToList();
        topic.Default = new Condition.Case { NextId = (Other.connections.FirstOrDefault()?.input.node as DialogNode).Id, };

        return topic;
    }

    public ConditionNode(DialogGraphView parent, Vector2 position = new Vector2()) : this(parent, position, Guid.NewGuid().ToString())
    {
    }

    public ConditionNode(DialogGraphView parent, Vector2 position, string id) : base(parent, position, id)
    {
        var color = new Color(0.2f, 0.5f, 0.2f);
        this.title = "CASE";
        this.titleContainer.style.backgroundColor = new StyleColor(color);

        Input = this.MakePort(Direction.Input);
        Input.portName = "";
        this.inputContainer.Add(Input);

        Other = this.MakePort(Direction.Output);
        Other.portName = "Default";
        this.outputContainer.Add(Other);

        var button = new Button(() => this.MakeCondition());
        button.text = "Add Output";
        this.titleContainer.Add(button);

        this.RefreshExpandedState();
        this.RefreshPorts();
    }

    public Port MakeCondition(string text = "")
    {
        var element = new VisualElement
        {
            name = "block",
        };
        element.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

        var output = MakePort(Direction.Output);

        var count = this.outputContainer.childCount;
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
        textField.style.width = new StyleLength(100);
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

        this.outputContainer.Insert(this.outputContainer.childCount - 1, element);

        this.RefreshExpandedState();
        this.RefreshPorts();

        return output;
    }
}