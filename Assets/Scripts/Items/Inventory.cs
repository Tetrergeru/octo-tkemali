using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public List<QuantifiedItem> Items = new List<QuantifiedItem>();

    [Serializable]
    public struct QuantifiedItem
    {
        public Item Item;
        public int Quantity;

        public QuantifiedItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }

    public void AddItems(Item item, int quantity)
    {
        var idx = Items.FindIndex((it) => it.Item.Id == item.Id);
        if (idx == -1)
        {
            Items.Add(new QuantifiedItem(item, quantity));
        }
        else
        {
            var it = Items[idx];
            it.Quantity += quantity;
            Items[idx] = it;
        }
    }

    // Returns the number of items that were actually removed
    public int RemoveItems(Item item, int quantity)
    {
        var idx = Items.FindIndex((it) => it.Item.Id == item.Id);
        if (idx == -1)
        {
            return 0;
        }

        var it = Items[idx];
        var toBeRemoved = Math.Min(quantity, it.Quantity);

        if (toBeRemoved == it.Quantity)
        {
            Items.RemoveAt(idx);
        }
        else
        {
            it.Quantity -= quantity;
            Items[idx] = it;
        }
        return toBeRemoved;
    }

    public int HowMany(Item item)
    {
        var idx = Items.FindIndex((it) => it.Item.Id == item.Id);
        
        return idx == -1 ? 0 : Items[idx].Quantity;
    }
}
