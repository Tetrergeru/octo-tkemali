using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavoiur : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public Path Path;

    private int _currentMarkerId;
    private Vector3 _currentMarker;
    private Transform _player;
    private bool _active = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetActive()
    {
        _active = true;
        NavMeshAgent.stoppingDistance = 0;

        if (Path == null) return;

        SetCurrentMarker(Path.GetNearestMarker(transform.position));
    }

    public void SetInactive()
    {
        _active = false;
    }

    void Update()
    {
        if (!_active) return;
        if (Path == null) return;

        if (Vector3.Distance(transform.position, _currentMarker) < 1.5f)
        {
            SetCurrentMarker(Path.NextMarker(_currentMarkerId));
        }
    }

    private void SetCurrentMarker(int id)
    {
        _currentMarkerId = id;
        _currentMarker = Path[_currentMarkerId];
        NavMeshAgent.destination = _currentMarker;
    }
}
