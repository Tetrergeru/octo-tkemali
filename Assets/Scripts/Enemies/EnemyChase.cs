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

    private int _currentMarkerId;
    private Vector3 _currentMarker;
    private float _detectionPrecent;

    public float DetectionPercent => _detectionPrecent;


    void Start()
    {
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
        Debug.Log($"{(DoISee(Player) ? "See" : "Don't see")}");
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
