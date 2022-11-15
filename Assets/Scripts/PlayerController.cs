using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Player;
    public Rigidbody PlayerBody;
    public ControlsSettings ControlsSettings;

    private bool _ground = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        Player.transform.Rotate(new Vector3(0, ControlsSettings.MouseSensetivity * mouseX, 0));
        MainCamera.transform.Rotate(new Vector3(-ControlsSettings.MouseSensetivity * mouseY, 0, 0));

        var forward = Input.GetAxis("Vertical");
        var right = Input.GetAxis("Horizontal");

        var forward_speed = ControlsSettings.MoovingSpeed * forward;
        var right_speed = ControlsSettings.MoovingSpeed * right;

        var velocity = PlayerBody.transform.forward * forward_speed + PlayerBody.transform.right * right_speed;

        PlayerBody.velocity = new Vector3(velocity.x, PlayerBody.velocity.y, velocity.z);

        if (Input.GetKeyDown(KeyCode.Space) && _ground)
        {
            PlayerBody.AddForce(Vector3.up * ControlsSettings.JumpForce);
            _ground = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CheckGrondContact(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        CheckGrondContact(collision);
    }

    void CheckGrondContact(Collision collision)
    {

        foreach (var contact in collision.contacts)
        {
            if (contact.point.y <= Player.transform.position.y + 0.5)
            {
                _ground = true;
            }
        }
    }
}
