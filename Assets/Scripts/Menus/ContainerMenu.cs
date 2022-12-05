using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerMenu : MonoBehaviour
{
    public ItemListComponent ItemList;

    public GameObject PlayerNameButton;
    public GameObject ContainerNameButton;
    public GameObject ItemRenderer;

    private ItemRenderer _itemRenderer;
    private TransferDirection _transferDirection;
    private Inventory _chestInventory;
    private Inventory _playerInventory;
    private ListComponent _panels;

    private enum TransferDirection
    {
        FromContainerToPlayer,
        FromPlayerToContainer,
    }


    public void LoadInventory(Inventory chestInventory, Inventory playerInventory, string containerName)
    {
        _itemRenderer = ItemRenderer.GetComponent<ItemRenderer>();
        ItemList.OnHover = item => _itemRenderer.SetGameObject(item.Item.Prefab);

        _chestInventory = chestInventory;
        _playerInventory = playerInventory;

        var inactiveColor = Color.white;
        var activeColor = Color.green;

        var playerNameButton = PlayerNameButton.GetComponent<Button>();
        var playerNameImage = PlayerNameButton.GetComponent<Image>();
        var playerNameText = PlayerNameButton.GetComponentInChildren<TextMeshProUGUI>();
        playerNameText.text = "Player";

        var containerNameButton = ContainerNameButton.GetComponent<Button>();
        var containerNameImage = ContainerNameButton.GetComponent<Image>();
        var containerNameText = ContainerNameButton.GetComponentInChildren<TextMeshProUGUI>();
        containerNameText.text = containerName;

        playerNameButton.onClick.AddListener(() =>
        {
            _transferDirection = TransferDirection.FromPlayerToContainer;
            playerNameImage.color = activeColor;
            containerNameImage.color = inactiveColor;
            RerenderItems();
        });

        containerNameButton.onClick.AddListener(() =>
        {
            _transferDirection = TransferDirection.FromContainerToPlayer;
            playerNameImage.color = inactiveColor;
            containerNameImage.color = activeColor;
            RerenderItems();
        });

        playerNameImage.color = inactiveColor;
        containerNameImage.color = activeColor;

        RerenderItems();
    }

    private void RerenderItems()
    {
        var (from, to) = _transferDirection == TransferDirection.FromContainerToPlayer
            ? (_chestInventory, _playerInventory)
            : (_playerInventory, _chestInventory);

        ItemList.SetItems(from.Items);
        ItemList.OnClick = item =>
        {
            from.RemoveItems(item.Item, item.Quantity);
            to.AddItems(item.Item, item.Quantity);
            RerenderItems();
        };
    }

    public static void AddText(Vector3 position, string content, GameObject parent)
    {
        var textObject = new GameObject("Text", typeof(RectTransform), typeof(CanvasRenderer));

        var transform = textObject.GetComponent<RectTransform>();
        transform.SetParent(parent.transform);
        transform.SetLocalPositionAndRotation(position, new Quaternion());

        var text = textObject.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = 20;
        text.color = Color.black;
        text.verticalAlignment = VerticalAlignmentOptions.Middle;
    }
}
