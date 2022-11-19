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
    }
}
