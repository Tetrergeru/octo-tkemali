using System.Collections.Generic;
using UnityEngine;

public class ListComponent : MonoBehaviour
{
    public float PaddingBetweenElements = 0;
    public float PaddingFromBorder = 0;

    private List<GameObject> _elements = new List<GameObject>();

    public void AddElement(GameObject element, int index = -1)
    {
        if (index < 0)
            index = _elements.Count;

        var transform = element.GetComponent<RectTransform>();
        transform.SetParent(GetComponent<RectTransform>());

        _elements.Insert(index, element);

        for (int i = index; i < _elements.Count; i++)
        {
            FixPosition(i);
        }

        this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotalHeight());
    }

    public void DestroyAll()
    {
        foreach (var element in _elements)
        {
            Destroy(element);
        }
        _elements = new List<GameObject>();
    }

    private float GetElementBottom(int idx)
    {
        if (_elements.Count == 0 || idx < 0)
            return 0;

        var transform = _elements[idx].GetComponent<RectTransform>();
        return transform.localPosition.y - transform.sizeDelta.y / 2;
    }

    private float TotalHeight()
    {
        return -GetElementBottom(_elements.Count - 1) + PaddingFromBorder;
    }

    // Assumes that items with indices less than idx are positioned correctly
    private void FixPosition(int idx)
    {
        var transform = _elements[idx].GetComponent<RectTransform>();
        var prevBottom = GetElementBottom(idx - 1);

        transform.anchorMin = new Vector2(0, 1);
        transform.anchorMax = new Vector2(1, 1);

        transform.SetLocalPositionAndRotation(
            new Vector3(
                transform.sizeDelta.x / 2,
                prevBottom - transform.sizeDelta.y / 2 - PaddingBetweenElements,
                0
            ),
            new Quaternion()
        );

        transform.offsetMin = new Vector2(PaddingFromBorder, transform.offsetMin.y);
        transform.offsetMax = new Vector2(-PaddingFromBorder, transform.offsetMax.y);
    }

}