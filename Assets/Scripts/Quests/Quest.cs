using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quest", order = 1)]
public class Quest : ScriptableObject
{
    [SerializeField]
    public MonoScript Script;

    public string Test()
    {
        return ((QuestScript)Activator.CreateInstance(Script.GetClass())).Test();
    }
}