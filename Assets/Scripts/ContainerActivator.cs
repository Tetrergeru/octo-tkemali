using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerActivator : MonoBehaviour
{
    public Inventory Inventory;
    public string ContainerName;

    public void Activate(GameObject sender)
    {
        var menuManager = sender.GetComponent<MenuManager>();
        menuManager.OpenInventory(Inventory, ContainerName);
    }
}
