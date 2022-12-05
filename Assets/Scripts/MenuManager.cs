using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    public PlayerController Player;
    public GlobalCtx GlobalCtx;
    public GameObject ContainerMenuPrefab;
    public GameObject DialogMenuPrefab;
    public GameObject InventoryMenuPrefab;

    private MenuState _state;
    private GameObject _currentMenu;

    private enum MenuState
    {
        Closed,
        Open,
    }

    void Start()
    {
        GlobalCtx = new GlobalCtx(GetComponent<Inventory>());
    }

    public void ExitMenu()
    {
        if (_state == MenuState.Closed) return;

        _state = MenuState.Closed;
        Time.timeScale = 1;
        Player.EnableControls();
        Destroy(_currentMenu);
    }

    public void OpenContainer(Inventory inventory, string containerName)
    {
        if (_state == MenuState.Open)
            return;
        OpenMenu();
        _currentMenu = Instantiate(ContainerMenuPrefab, new Vector3(), new Quaternion(), this.transform);
        _currentMenu.GetComponent<ContainerMenu>().LoadInventory(inventory, GetComponent<Inventory>(), containerName);
    }

    public void OpenInventory()
    {
        if (_state == MenuState.Open)
            return;
        OpenMenu();
        _currentMenu = Instantiate(InventoryMenuPrefab, new Vector3(), new Quaternion(), this.transform);
        _currentMenu.GetComponent<InventoryMenu>().LoadInventory(GetComponent<Inventory>(), this.Player);
    }

    public void OpenDialog(Dialog dialog, string npcName)
    {
        if (_state == MenuState.Open)
            return;
        OpenMenu();
        _currentMenu = Instantiate(DialogMenuPrefab, new Vector3(), new Quaternion(), this.transform);
        _currentMenu.GetComponent<DialogMenu>().LoadDialog(dialog, npcName, GlobalCtx);
    }

    private void OpenMenu()
    {
        _state = MenuState.Open;
        Time.timeScale = 0;
        Player.DisableControls();
    }
}
