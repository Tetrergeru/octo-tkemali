using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public Transform Player;
    public Transform Eye;
    public Path Path;
    public float ViewDistance;
    public float FieldOfView;
    public float DetectionPerSecond;

    private int _currentMarkerId;
    private Vector3 _currentMarker;
    private float _detectionPercent;
    private PlayerVisibility _playerVisibility;

    void Start()
    {
        _playerVisibility = Player.gameObject.GetComponent<PlayerVisibility>();
        if (Path != null)
        {
            SetCurrentMarker(Path.GetNearestMarker(transform.position));
        }
    }

    void Update()
    {
        if (Path != null && Vector3.Distance(transform.position, _currentMarker) < 1.5f)
        {
            SetCurrentMarker(Path.NextMarker(_currentMarkerId));
        }
        if (DoISee(Player))
        {
            _detectionPercent += Time.deltaTime
                * DetectionPerSecond
                * _playerVisibility.Visibility;
        }
        else
        {
            _detectionPercent -= Time.deltaTime * DetectionPerSecond;
        }
        _detectionPercent = Mathf.Clamp01(_detectionPercent);

        _playerVisibility.UpdateVisibility(_detectionPercent);
    }

    private bool DoISee(Transform @object)
    {
        var eyePos = Eye.position;
        var playerPos = Player.position;
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

        return obj.transform.IsChildOf(@object) || obj.transform == @object;
    }

    private void SetCurrentMarker(int id)
    {
        _currentMarkerId = id;
        _currentMarker = Path[_currentMarkerId];
        NavMeshAgent.destination = _currentMarker;
    }
}
