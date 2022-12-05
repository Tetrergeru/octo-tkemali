using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public ItemListComponent ItemList;
    public ItemRenderer ItemRenderer;

    private Inventory _playerInventory;
    private PlayerController _playerController;
    private ListComponent _panels;

    public void LoadInventory(Inventory inventory, PlayerController playerController)
    {
        _playerInventory = inventory;
        _playerController = playerController;
        ItemList.OnHover = item => ItemRenderer.SetGameObject(item.Item.Prefab);
        RerenderItems();
    }

    private void RerenderItems()
    {
        ItemList.SetItems(_playerInventory.Items);
        ItemList.OnClick = item =>
        {
            if (item.Item.Consume(_playerController))
                _playerInventory.RemoveItems(item.Item, 1);
            RerenderItems();
        };
    }
}
