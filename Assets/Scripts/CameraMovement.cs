using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 0.1F; // How responsive the camera is to mouse movement
    public float maxAngle = 15f;    // Maximum angle the camera can rotate (in degrees)

    private float currentXRotation = 0f;
    private float currentYRotation = 0f;

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Accumulate the rotation values
        currentXRotation -= mouseY * sensitivity;
        currentYRotation += mouseX * sensitivity;

        // Clamp the rotations to prevent over-rotation
        currentXRotation = Mathf.Clamp(currentXRotation, -maxAngle, maxAngle);
        currentYRotation = Mathf.Clamp(currentYRotation, -maxAngle, maxAngle);

        // Apply the rotation to the camera
        transform.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
    }
}
