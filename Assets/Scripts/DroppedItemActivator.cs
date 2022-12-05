using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemActivator : MonoBehaviour
{
    public DroppedItem DroppedItem;

    public void Activate(GameObject sender)
    {
        var inventory = sender.GetComponent<Inventory>();
        inventory.AddItems(DroppedItem.Item.Item, DroppedItem.Item.Quantity);
        Destroy(this.gameObject);
    }
}
