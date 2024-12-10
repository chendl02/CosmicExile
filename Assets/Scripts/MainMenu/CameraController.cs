using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 100f; // Sensitivity of the mouse
    public Transform playerBody;    // Reference to the player body (if needed)
    private float xRotation = 0f;  // To limit vertical rotation

    // Start is called before the first frame update
    void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse movement inputs
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Adjust the vertical rotation (Y-axis)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -5f, 5f); // Limit vertical rotation

        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera up/down
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX); // Rotate player body horizontally
        }
        else
        {
            transform.Rotate(Vector3.up * mouseX); // Rotate camera horizontally if no player body
        }
    }
}
