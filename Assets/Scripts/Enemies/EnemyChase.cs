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
        DoISee(Player);
    }

    private bool DoISee(Transform transform)
    {
        var eyePos = Eye.position;
        var playerPos = Player.position;
        var direction = playerPos - eyePos;

        Debug.DrawLine(eyePos, playerPos, Color.red, 0.1f);

        var ray = new Ray(eyePos, direction);
        var hit = Physics.Raycast(ray, out var obj, ViewDistance);
        if (!hit)
        {
            Debug.Log($"DoISee NoHit");
            return false;
        }

        if (!obj.transform.IsChildOf(transform) && obj.transform != transform)
        {
            Debug.Log($"DoISee IsNotChild (tag = {obj.transform.gameObject.name})");
            return false;
        }

        var angle = Vector3.Angle(direction, transform.forward);

        if (angle >= FieldOfView)
        {
            Debug.Log($"DoISee angle ({angle}) >= FieldOfView ({FieldOfView})");
        }
        return angle < FieldOfView;
    }

    private void SetCurrentMarker(int id)
    {
        _currentMarkerId = id;
        _currentMarker = Path[_currentMarkerId];
        NavMeshAgent.destination = _currentMarker;
    }
}
