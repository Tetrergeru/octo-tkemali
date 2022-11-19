using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public string Name;

    public ItemType Type;

    public float Weight;

    public float Price;

    public string Id;

    void Awake()
    {
        if (Id == null || Id == "")
            Id = UniqueId.RandomId(10);
    }
}

public enum ItemType
{
    Gold,
    Food,
    Misk,
}