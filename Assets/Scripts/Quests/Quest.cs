using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class Quest : ScriptableObject
{
    [SerializeField] private QuestScript _questInstance;

#if UNITY_EDITOR
    [SerializeField] private MonoScript _script;

    void OnValidate()
    {
        if (_script != null)
        {
            _questInstance = ((QuestScript)Activator.CreateInstance(_script.GetClass()));
        }
    }
#endif

    public string Test()
    {
        return _questInstance?.Test();
    }
}