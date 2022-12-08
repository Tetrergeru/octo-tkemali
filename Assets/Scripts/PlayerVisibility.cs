using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    [SerializeField]
    private GaugeComponent VisibilityGauge;

    private float _previousVisibility = -1;
    private float _visibility = 0;


    // Temporary crutch
    public float InvisibilityTime { get; set; }
    public float MaxInvisibilityTime => 10;

    void Update()
    {
        InvisibilityTime -= Time.deltaTime;
        if (InvisibilityTime < 0)
            InvisibilityTime = 0;

        if (InvisibilityTime != 0)
        {
            VisibilityGauge.SetColor(Color.blue);
            VisibilityGauge.Value = InvisibilityTime / MaxInvisibilityTime;
        }
        else
        {
            VisibilityGauge.SetColor(Color.white);
        }
    }

    void LateUpdate()
    {
        if (InvisibilityTime != 0)
            return;

        if (_previousVisibility != _visibility)
        {
            VisibilityGauge.Value = _visibility;
        }

        _previousVisibility = _visibility;
        _visibility = 0;
    }

    public float Visibility => InvisibilityTime != 0 ? 0 : 1.0f;

    public void UpdateVisibility(float value)
    {
        _visibility = Mathf.Max(Mathf.Clamp01(value), _visibility);
    }
}
