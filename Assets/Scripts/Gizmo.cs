using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public enum GizmoType
    {
        Cube,
        Sphere,
    }
    public Color Color = Color.white;

    public bool Enabled = true;

    public GizmoType Type;

    public Vector3 Scale = new Vector3(1, 1, 1);

    private Transform _transform = null;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Enabled)
        {
            return;
        }

        if (_transform == null)
        {
            _transform = GetComponent<Transform>();
        }
        Gizmos.color = Color;
        switch (Type)
        {
            case GizmoType.Cube:
                Gizmos.DrawCube(_transform.position, Scale);
                break;
            case GizmoType.Sphere:
                Gizmos.DrawSphere(_transform.position, Scale.x);
                break;
        }
    }
#endif
}
