using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public Dialog Dialog;
    public string NPCName;

    public void Activate(GameObject sender)
    {
        var menuManager = sender.GetComponent<MenuManager>();
        menuManager.OpenDialog(Dialog, NPCName);
    }
}
