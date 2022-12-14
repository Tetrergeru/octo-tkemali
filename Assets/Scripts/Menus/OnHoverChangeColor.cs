using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OnHoverChangeColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color DefaultColor = Color.white;
    public Color HoveredtColor = Color.white;
    public Action OnHoverStart = () => {};
    public Action OnHoverFinish = () => {};

    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
        _image.color = DefaultColor;
    }

    public void ResetColor()
    {
        _image.color = DefaultColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _image.color = HoveredtColor;
        OnHoverStart();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = DefaultColor;
        OnHoverFinish();
    }
}
