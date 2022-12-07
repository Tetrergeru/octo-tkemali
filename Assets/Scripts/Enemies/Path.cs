using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]
    private Color _linkColor = Color.white;

    [SerializeField]
    private List<Transform> PathMarkers = new List<Transform>();

    [SerializeField]
    private PatrolingType _patrolingType = PatrolingType.Loop;

    public enum PatrolingType
    {
        Loop,
        BackAndForth,
    }

    public Vector3 LastMarkerLocalPosition => PathMarkers.Count == 0
        ? new Vector3()
        : PathMarkers[PathMarkers.Count - 1].localPosition;

    public Vector3 this[int value] => PathMarkers[value].position;

    public int NextMarker(int currentMarker, int prevMarker = -1)
    {
        if (prevMarker == -1)
        {
            return (currentMarker + 1) % PathMarkers.Count;
        }

        return currentMarker;
    }

    public int GetNearestMarker(Vector3 position)
    {
        if (PathMarkers.Count == 0)
            throw new Exception("Path contains no markers");

        var min = float.MaxValue;
        var iMin = -1;
        for (var i = 0; i < PathMarkers.Count; i++)
        {
            var dist = Vector3.Distance(PathMarkers[i].position, position);
            if (dist < min)
            {
                min = dist;
                iMin = i;
            }
        }

        return iMin;
    }

#if UNITY_EDITOR
    public void AddNewMarker(GameObject prefab)
    {
        var marker = (GameObject)PrefabUtility.InstantiatePrefab(prefab, transform);
        marker.transform.localPosition = LastMarkerLocalPosition + new Vector3(0.1f, 0.1f, 0.1f);
        marker.name = $"Path Marker {PathMarkers.Count}";

        PathMarkers.Add(marker.transform);

        EditorUtility.SetDirty(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _linkColor;

        for (var i = 0; i < PathMarkers.Count - 1; i++)
        {
            Gizmos.DrawLine(PathMarkers[i].position, PathMarkers[i + 1].position);
        }

        if (_patrolingType == PatrolingType.Loop && PathMarkers.Count >= 2)
        {
            Gizmos.DrawLine(PathMarkers[0].position, PathMarkers[PathMarkers.Count - 1].position);
        }
    }
#endif
}
