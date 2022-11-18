using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    public Rigidbody PawnBody;

    private int _ground = 0;

    void OnTriggerEnter(Collider other)
    {
        _ground += 1;
    }

    void OnTriggerExit(Collider other)
    {
        _ground -= 1;
        if (_ground < 0)
            _ground = 0;
    }

    public bool Jump(float force)
    {
        if (_ground > 0)
        {
            PawnBody.AddForce(Vector3.up * force);
            _ground = 0;
            return true;
        }
        return false;
    }
}
