using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class PathEditor : Editor
{
    public GameObject PathMarkerPrefab;

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Add Marker"))
        {
            var path = (Path)target;
            path.AddNewMarker(PathMarkerPrefab);
        }
        DrawDefaultInspector();
    }
}
