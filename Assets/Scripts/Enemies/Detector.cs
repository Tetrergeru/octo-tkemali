using System;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Action OnPlayerDetected = () => {};
    public Action OnPlayerLost = () => {};

    [SerializeField]
    private Transform _eye;

    [SerializeField]
    private float ViewDistance;

    [SerializeField]
    private float FieldOfView;

    [SerializeField]
    private float DetectionPerSecond;

    [SerializeField]
    private float _detectionThreshold;

    private Transform _player;
    private float _detectionPercent;
    private PlayerVisibility _playerVisibility;
    private bool _detected = false;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerVisibility = _player.gameObject.GetComponent<PlayerVisibility>();
    }

    void Update()
    {
        if (DoISee(_player))
            _detectionPercent += Time.deltaTime * DetectionPerSecond * _playerVisibility.Visibility;
        else
            _detectionPercent -= Time.deltaTime * DetectionPerSecond;

        _detectionPercent = Mathf.Clamp01(_detectionPercent);

        if (!_detected && _detectionPercent > _detectionThreshold)
        {
            _detected = true;
            OnPlayerDetected();
        }
        if (_detected && _detectionPercent < _detectionThreshold)
        {
            _detected = false;
            OnPlayerLost();
        }

        _playerVisibility.UpdateVisibility(_detectionPercent);
    }

    private bool DoISee(Transform player)
    {
        var eyePos = _eye.position;
        var playerPos = player.position;
        var direction = playerPos - eyePos;

        Debug.DrawLine(eyePos, eyePos + direction, Color.red, 0.1f);
        Debug.DrawLine(eyePos, eyePos + transform.forward * ViewDistance, Color.blue, 0.1f);

        var angle = Vector3.Angle(direction, transform.forward);
        if (angle >= FieldOfView)
        {
            return false;
        }

        var ray = new Ray(eyePos, direction);
        var hit = Physics.Raycast(ray, out var obj, ViewDistance);
        if (!hit)
        {
            return false;
        }

        return obj.transform.IsChildOf(player) || obj.transform == player;
    }
}
