using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public float MaxFatigue = 100;
    public ControlsSettings ControlsSettings;

    private float _fatigue;

    public float Fatigue
    {
        get => _fatigue;
        set
        {
            _fatigue = Mathf.Min(Mathf.Max(value, 0), MaxFatigue);
            RedrawGauge();
        }
    }

    public GameObject FatigueGauge;

    void Start()
    {
        _fatigue = MaxFatigue;
        RedrawGauge();
    }

    void Update()
    {
        Fatigue += ControlsSettings.FatigueRegenPerSecond * Time.deltaTime;
    }

    private void RedrawGauge()
    {
        var transform = FatigueGauge.GetComponent<RectTransform>();
        transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200 * Fatigue / MaxFatigue);

        var position = transform.localPosition;
        var rotation = transform.localRotation;
        transform.SetLocalPositionAndRotation(new Vector3(-100 + 100 * Fatigue / MaxFatigue, position.y, position.z), rotation);
    }
}
