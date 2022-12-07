using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    [SerializeField]
    private GaugeComponent VisibilityGauge;

    private float _previousVisibility = -1;
    private float _visibility = 0;

    void LateUpdate()
    {
        if (_previousVisibility != _visibility)
        {
            VisibilityGauge.Value = _visibility;
        }

        _previousVisibility = _visibility;
        _visibility = 0;
    }

    public float Visibility => 1.0f;

    public void UpdateVisibility(float value)
    {
        _visibility = Mathf.Max(Mathf.Clamp01(value), _visibility);
    }
}
