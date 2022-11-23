using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<Topic> Topics;
}
