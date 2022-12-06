using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public Transform Player;
    public GameObject Leg;
    public AnimationCurve AnimationCurve;

    void Start()
    {

    }


    void Update()
    {
        NavMeshAgent.destination = Player.position;
    }
}
