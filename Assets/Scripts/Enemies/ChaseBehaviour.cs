using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;

    private Transform _player;
    private bool _active = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetActive()
    {
        _active = true;
        NavMeshAgent.destination = _player.position;
    }

    public void SetInactive()
    {
        _active = false;
    }

    void Update()
    {
        if (!_active) return;

        NavMeshAgent.destination = _player.position;
    }

}
