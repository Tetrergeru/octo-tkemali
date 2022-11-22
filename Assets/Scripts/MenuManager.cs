using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public PlayerController Player;

    public GameObject InventoryMenuPrefab;

    private MenuState _state;
    private GameObject _currentMenu;

    private enum MenuState
    {
        Closed,
        Open,
    }

    void Update()
    {
        if (_state == MenuState.Closed) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _state = MenuState.Closed;
            Time.timeScale = 1;
            Player.EnableControls();
            Destroy(_currentMenu);
        }
    }

    public void OpenInventory(Inventory inventory, string containerName)
    {
        _state = MenuState.Open;
        Time.timeScale = 0;
        Player.DisableControls();

        _currentMenu = Instantiate(InventoryMenuPrefab, new Vector3(), new Quaternion(), this.transform);
        _currentMenu.GetComponent<InventoryMenu>().LoadInventory(inventory, GetComponent<Inventory>(), containerName);
    }
}