using UnityEngine;

public class GaugeComponent : MonoBehaviour
{
    [SerializeField]
    private RectTransform _background;

    [SerializeField]
    private RectTransform _gauge;

    [SerializeField]
    private StretchType _stretchType;

    private float _value;
    private float _maxWidth;

    public enum StretchType
    {
        Begin,
        Middle,
        End,
    }

    public float Value
    {
        get => _value;
        set => SetValue(value);
    }

    void Start()
    {
        _maxWidth = _background.sizeDelta.x;

        var (minX, maxX) = _stretchType switch
        {
            StretchType.Begin => (0, 0),
            StretchType.Middle => (0.5f, 0.5f),
            StretchType.End => (1, 1),
            _ => (0, 0),
        };
        _gauge.anchorMin = new Vector2(minX, 0);
        _gauge.anchorMax = new Vector2(maxX, 1);
        Value = 0;
    }

    private void SetValue(float value)
    {
        _value = Mathf.Clamp01(value);
        var width = _value * _maxWidth;

        var offset = _stretchType switch
        {
            StretchType.Begin => (width - _maxWidth) / 2,
            StretchType.Middle => 0,
            StretchType.End => (_maxWidth - width) / 2,
            _ => 0,
        };
        _gauge.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        _gauge.SetLocalPositionAndRotation(new Vector3(offset, 0, 0), new Quaternion());
    }
}
