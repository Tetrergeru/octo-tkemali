using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public float MaxFatigue = 100;
    public ControlsSettings ControlsSettings;
    public GaugeComponent FatigueGauge;

    private float _fatigue;

    public float Fatigue
    {
        get => _fatigue;
        set
        {
            _fatigue = Mathf.Clamp(value, 0, MaxFatigue);
            FatigueGauge.Value = _fatigue / MaxFatigue;
        }
    }

    void Start()
    {
        Fatigue = MaxFatigue;
    }

    void Update()
    {
        Fatigue += ControlsSettings.FatigueRegenPerSecond * Time.deltaTime;
    }
}
