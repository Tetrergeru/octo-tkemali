#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UniqueId))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var uniqueId = (UniqueId)target;
        if (GUILayout.Button("Regenerate ID"))
        {
            uniqueId.RegenerateId();
        }
    }
}

#endif