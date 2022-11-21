using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemRotator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerMoveHandler
{
    public ItemRenderer ItemRenderer;

    private bool _trackPointer = false;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        => _trackPointer = true;

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        => _trackPointer = false;

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        => _trackPointer = false;


    void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    {
        if (!_trackPointer)
            return;

        ItemRenderer.Rotate(eventData.delta);
    }
}
