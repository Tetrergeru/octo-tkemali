using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "ScriptableObjects/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<Topic> Topics;
    public List<Dialog> Parents;
}
