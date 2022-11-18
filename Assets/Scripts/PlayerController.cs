using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Player;
    public Rigidbody PlayerBody;
    public ControlsSettings ControlsSettings;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateRotation();
        UpdatePosition();
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
            velocity *= 1.5f;
        }

        PlayerBody.velocity = new Vector3(velocity.x, PlayerBody.velocity.y, velocity.z);

        if (Input.GetKey(KeyCode.Space))
        {
            BroadcastMessage("Jump", ControlsSettings.JumpForce);
        }
    }
}
