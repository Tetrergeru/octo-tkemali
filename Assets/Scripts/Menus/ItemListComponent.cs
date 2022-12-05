using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemListComponent : MonoBehaviour
{
    public ListComponent ItemList;
    public RectTransform ContentTransform;
    public Action<Inventory.QuantifiedItem> OnHover = _ => { };
    public Action<Inventory.QuantifiedItem> OnClick = _ => { };

    public void SetItems(List<Inventory.QuantifiedItem> items)
    {
        ItemList.DestroyAll();

        ContentTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, items.Count * 50);

        for (var i = 0; i < items.Count; i++)
        {
            ItemList.AddElement(AddItem(items[i]));
        }
    }

    private GameObject AddItem(Inventory.QuantifiedItem item)
    {
        var panelObject = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer));

        var transform = panelObject.GetComponent<RectTransform>();
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        var colorChanger = panelObject.AddComponent<OnHoverChangeColor>();
        colorChanger.DefaultColor = new Color(1, 1, 1, 0.1f);
        colorChanger.HoveredtColor = new Color(1, 1, 1, 0.5f);
        colorChanger.OnHoverStart = () => OnHover(item);

        var button = panelObject.AddComponent<Button>();
        button.onClick.AddListener(() => OnClick(item));

        ContainerMenu.AddText(new Vector3(-100, 0, 0), item.Item.Name, panelObject);
        ContainerMenu.AddText(new Vector3(240, 0, 0), $"{item.Quantity}", panelObject);

        return panelObject;
    }
}
