#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Edit"))
        {
            var editor = EditorWindow.GetWindow<DialogGraph>();
            editor.TargetObject = (Dialog)target;
        }
    }
}

#endif