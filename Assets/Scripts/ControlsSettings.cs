using UnityEngine;

[CreateAssetMenu(fileName = "ControlsSettings", menuName = "ScriptableObjects/ControlsSettings", order = 1)]
public class ControlsSettings : ScriptableObject
{
    public float MouseSensetivity = 3;
    public float MoovingSpeed = 10;
    public float JumpForce = 100;
}