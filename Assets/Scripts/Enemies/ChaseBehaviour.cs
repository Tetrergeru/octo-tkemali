using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public float DistanceFromPlayer = 5;
    public float DistanceFromPlayerToShoot = 10;
    public float ProjectileCooldown;
    public GameObject ProjectilePrefab;
    public Transform ProjectileStartPoint;

    private Transform _player;
    private bool _active = false;
    private float _projectileCooldown = 0;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SetActive()
    {
        _active = true;
        NavMeshAgent.destination = _player.position;
        NavMeshAgent.stoppingDistance = DistanceFromPlayer;
    }

    public void SetInactive()
    {
        _active = false;
    }

    void Update()
    {
        if (_projectileCooldown > 0)
        {
            _projectileCooldown -= Time.deltaTime;
            _projectileCooldown = Mathf.Clamp(_projectileCooldown, 0, ProjectileCooldown);
        }

        if (!_active) return;

        NavMeshAgent.destination = _player.position;
        if (_projectileCooldown == 0)
        {
            Instantiate(
                ProjectilePrefab,
                ProjectileStartPoint.position,
                Quaternion.LookRotation(_player.position - transform.position, ProjectileStartPoint.up)
            );
            _projectileCooldown = ProjectileCooldown;
        }
    }

}
