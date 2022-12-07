using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public float MaxFatigue = 100;
    public float MaxHealth = 100;
    public ControlsSettings ControlsSettings;
    public GaugeComponent FatigueGauge;
    public GaugeComponent HealthGauge;

    private float _fatigue;
    private float _health;

    public float Fatigue
    {
        get => _fatigue;
        set
        {
            _fatigue = Mathf.Clamp(value, 0, MaxFatigue);
            FatigueGauge.Value = _fatigue / MaxFatigue;
        }
    }

    public float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            HealthGauge.Value = _health / MaxHealth;
            if (_health == 0)
            {
                this.GetComponent<PlayerController>().Die();
                Health = MaxHealth / 2;
            }
        }
    }

    void Start()
    {
        Fatigue = MaxFatigue;
        Health = MaxHealth;
    }

    void Update()
    {
        Fatigue += ControlsSettings.FatigueRegenPerSecond * Time.deltaTime;
        Health += ControlsSettings.HealthRegenPerSecond * Time.deltaTime;
    }
}
