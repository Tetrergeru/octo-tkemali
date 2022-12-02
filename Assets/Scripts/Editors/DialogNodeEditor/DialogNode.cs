using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DialogNode : Node
{
    public string Id;
    protected DialogGraphView _parent;

    public abstract ITopic Save();

    public DialogNode(DialogGraphView parent) : this(parent, new Vector2(), Guid.NewGuid().ToString()) { }

    public DialogNode(DialogGraphView parent, Vector2 position, string id)
    {
        _parent = parent;
        this.title = "Stage";
        this.Id = id;

        this.SetPosition(new Rect(position.x, position.y, 100, 200));
    }

    protected Port MakePort(Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return MakePort<bool>(direction, capacity);
    }

    protected Port MakePort<T>(Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return this.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(T));
    }
}
