using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public GameObject Content;
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
        _panels = Content.GetComponent<ListComponent>();
        _itemRenderer = ItemRenderer.GetComponent<ItemRenderer>();

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

        _panels.DestroyAll();

        var numberOfItems = from.Items.Count;

        var rect = Content.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, numberOfItems * 50);

        for (var i = 0; i < numberOfItems; i++)
        {
            _panels.AddElement(AddItem(i, from, to));
        }
    }

    private GameObject AddItem(int idx, Inventory from, Inventory to)
    {
        var item = from.Items[idx];

        var panelObject = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));

        var transform = panelObject.GetComponent<RectTransform>();
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var colorChanger = panelObject.AddComponent<OnHoverChangeColor>();
        colorChanger.DefaultColor = new Color(1, 1, 1, 0.1f);
        colorChanger.HoveredtColor = new Color(1, 1, 1, 0.5f);
        colorChanger.OnHoverStart = () => _itemRenderer.SetGameObject(item.Item.Prefab);

        var button = panelObject.AddComponent<Button>();
        button.onClick.AddListener(() =>
        {
            from.RemoveItems(item.Item, item.Quantity);
            to.AddItems(item.Item, item.Quantity);
            RerenderItems();
        });

        AddText(new Vector3(-100, 0, 0), item.Item.Name, panelObject);
        AddText(new Vector3(240, 0, 0), $"{item.Quantity}", panelObject);

        return panelObject;
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
