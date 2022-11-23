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

        MakeStartNode();
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

    public void AddNode()
    {
        var node = new DialogNode
        {
            title = "Stage",
            ID = Guid.NewGuid().ToString(),
            Start = false,
        };

        node.SetPosition(new Rect(0, 0, 100, 200));

        var port = MakePort(node, Direction.Input);
        port.portName = "Input";
        node.inputContainer.Add(port);

        var button = new Button(() => MakeOutput(node));
        button.text = "Add Output";
        node.titleContainer.Add(button);

        var textField = new TextField
        {
            name = "",
            value = "Answer",
        };
        node.extensionContainer.Add(textField);

        node.RefreshExpandedState();
        node.RefreshPorts();

        AddElement(node);
    }

    private void MakeStartNode()
    {
        var node = new DialogNode
        {
            title = "Start",
            ID = Guid.NewGuid().ToString(),
            Start = true,
        };

        node.SetPosition(new Rect(200, 200, 100, 200));

        var button = new Button(() => MakeOutput(node));
        button.text = "Add Output";
        node.titleContainer.Add(button);

        node.RefreshExpandedState();
        node.RefreshPorts();

        AddElement(node);
    }

    private Port MakePort(DialogNode node, Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(bool));
    }

    private void MakeOutput(DialogNode node)
    {
        var element = new VisualElement();
        element.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

        var output = MakePort(node, Direction.Output);

        var count = 0;
        node.outputContainer.Query("connector").ForEach((_) => count++);
        var name = $"Output {count}";
        output.portName = "";
        output.name = name;

        var textField = new TextField
        {
            name = "",
            value = name,
        };
        textField.RegisterValueChangedCallback(e => output.name = e.newValue);
        node.contentContainer.Add(textField);

        var button = new Button(() =>
        {
            DeleteElements(output.connections);
            node.outputContainer.Remove(element);
        });
        button.text = "x";

        element.Add(button);
        element.Add(textField);
        element.Add(output);

        node.outputContainer.Add(element);

        node.RefreshExpandedState();
        node.RefreshPorts();
    }
}
