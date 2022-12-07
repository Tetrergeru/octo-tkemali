using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Detector), typeof(ChaseBehaviour), typeof(PatrolBehavoiur))]
public class EnemyState : MonoBehaviour
{
    [SerializeField]
    private Detector _detector;

    [SerializeField]
    private ChaseBehaviour _chaseBehavoiur;

    [SerializeField]
    private PatrolBehavoiur _patrolBehaviour;

    [SerializeField]
    private MeshRenderer _body;

    [SerializeField]
    private Material _chaseMaterial;

    [SerializeField]
    private Material _patrolMaterial;

    void Start()
    {
        _detector.OnPlayerDetected = () =>
        {
            _patrolBehaviour.SetInactive();
            _chaseBehavoiur.SetActive();
            _body.material = _chaseMaterial;
        };
        _detector.OnPlayerLost = () =>
        {
            _patrolBehaviour.SetActive();
            _chaseBehavoiur.SetInactive();
            _body.material = _patrolMaterial;
        };
        _patrolBehaviour.SetActive();
    }
}
