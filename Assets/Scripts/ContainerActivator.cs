using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerActivator : MonoBehaviour
{
    public Inventory Inventory;

    public void Activate(GameObject sender)
    {
        var inventory = sender.GetComponent<Inventory>();
        Debug.Log($"sender.Inventory = {inventory.Items.Count}\nThis.Inventory = {Inventory.Items.Count}");  
    }
}
