using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuManager))]
public class PlayerController : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Player;
    public Rigidbody PlayerBody;
    public ControlsSettings ControlsSettings;
    public PlayerAttributes PlayerAttributes;
    public GameObject Coursor;
    public JumpController JumpController;

    private bool _contolsActive = true;
    private MenuManager _menuManager;

    void Start()
    {
        _menuManager = GetComponent<MenuManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (_contolsActive)
        {
            UpdateRotation();
            UpdatePosition();
            UpdateActivation();
        }
        UpdateInventoryKeys();
    }

    public void DisableControls()
    {
        Cursor.lockState = CursorLockMode.None;
        _contolsActive = false;
    }

    public void EnableControls()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _contolsActive = true;
    }

    void UpdateInventoryKeys()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            _menuManager.OpenInventory();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _menuManager.ExitMenu();
        }
    }

    void UpdateRotation()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        Player.transform.Rotate(new Vector3(0, ControlsSettings.MouseSensetivity * mouseX, 0));
        MainCamera.transform.Rotate(new Vector3(-ControlsSettings.MouseSensetivity * mouseY, 0, 0));

        var angle = Mathf.Repeat(MainCamera.transform.localRotation.eulerAngles.x + 180, 360) - 180;
        if (angle > ControlsSettings.VerticalAngleLowerConstraint)
        {
            MainCamera.transform.Rotate(new Vector3(ControlsSettings.VerticalAngleLowerConstraint - angle, 0, 0));
        }
        if (angle < ControlsSettings.VerticalAngleUpperConstraint)
        {
            MainCamera.transform.Rotate(new Vector3(ControlsSettings.VerticalAngleUpperConstraint - angle, 0, 0));
        }
    }

    void UpdatePosition()
    {
        var forward = Input.GetAxis("Vertical");
        var right = Input.GetAxis("Horizontal");

        var forward_speed = ControlsSettings.MoovingSpeed * forward;
        var right_speed = ControlsSettings.MoovingSpeed * right;

        var velocity = PlayerBody.transform.forward * forward_speed + PlayerBody.transform.right * right_speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (PlayerAttributes.Fatigue > ControlsSettings.RunFatiguePerSecond * Time.deltaTime)
            {
                velocity *= ControlsSettings.RunningSpeed / ControlsSettings.MoovingSpeed;
            }
            PlayerAttributes.Fatigue -= ControlsSettings.RunFatiguePerSecond * Time.deltaTime;
        }

        PlayerBody.velocity = new Vector3(velocity.x, PlayerBody.velocity.y, velocity.z);

        if (Input.GetKey(KeyCode.Space))
        {
            var jumpMulitplyer = PlayerAttributes.Fatigue > ControlsSettings.JumpFatigue ? 1.0f : 0.5f;
            if (JumpController.Jump(ControlsSettings.JumpForce * jumpMulitplyer))
            {
                PlayerAttributes.Fatigue -= ControlsSettings.JumpFatigue;
            };
        }
    }

    void UpdateActivation()
    {
        var obj = FindWAILA();
        if (obj == null)
        {
            Coursor.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            return;
        }

        Coursor.GetComponent<Image>().color = Color.red;

        if (Input.GetKeyDown(KeyCode.E))
        {
            obj.SendMessageUpwards("Activate", Player);
        }
    }

    GameObject FindWAILA()
    {
        var start = MainCamera.transform.position;
        var direction = MainCamera.transform.forward;
        var maxLength = 3;

        var ok = Physics.Raycast(start, direction, out var hit, maxLength);

        if (!ok)
        {
            Coursor.GetComponent<Image>().color = Color.red;
            return null;
        };

        var obj = hit.collider.gameObject;
        if (!obj.CompareTag("Activator"))
            return null;

        return obj;
    }
}
