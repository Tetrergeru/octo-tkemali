using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRenderer : MonoBehaviour
{
    public GameObject Anchor;
    public Shader UnlitItemShader;

    private GameObject _currentObject;

    public void SetGameObject(GameObject prefab)
    {
        if (_currentObject != null)
        {
            Destroy(_currentObject);
        }

        _currentObject = Instantiate(prefab);
        _currentObject.transform.SetParent(Anchor.transform);

        var (min, max) = FindBoundsSize(_currentObject);

        var bounds = max - min;
        var maxBounds = Mathf.Max(bounds.x, bounds.y, bounds.z);
        var scaleFactor = 5 / maxBounds;
        _currentObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        _currentObject.transform.localRotation = new Quaternion();

        var offset = (max + min) / 2;
        _currentObject.transform.localPosition = -offset * scaleFactor;

        var layer = LayerMask.NameToLayer("UIObject");
        foreach (Transform child in _currentObject.transform)
        {
            child.gameObject.layer = layer;
        }

        foreach (var renderer in _currentObject.GetComponentsInChildren<Renderer>())
        {
            renderer.material.shader = UnlitItemShader;
        }
    }

    public void Rotate(Vector2 delta)
    {
        Anchor.transform.Rotate(new Vector3(delta.y, -delta.x, 0), Space.World);
    }

    private static (Vector3, Vector3) FindBoundsSize(GameObject obj)
    {
        var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            var b = renderer.bounds;

            min.x = Mathf.Min(b.min.x, min.x);
            min.y = Mathf.Min(b.min.y, min.y);
            min.z = Mathf.Min(b.min.z, min.z);

            max.x = Mathf.Max(b.max.x, max.x);
            max.y = Mathf.Max(b.max.y, max.y);
            max.z = Mathf.Max(b.max.z, max.z);
        }
        return (min, max);
    }
}
