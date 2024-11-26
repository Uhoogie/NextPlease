using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingScript : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask interactableLayer;  // Set the layer for interactable objects
    public float grabHeight = 0f;        // Height offset when grabbing
    public float grabSideOffset = 0f;    // Side offset when grabbing
    public float grabDistance = 5f;      // Maximum grab distance
    public float followSpeed = 10f;      // Speed of object following

    private Camera mainCamera;           // Reference to the main camera
    private Rigidbody grabbedObject;     // The object currently grabbed
    private Vector3 targetPosition;      // Target position for the grabbed object

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            ReleaseObject();
        }

        if (grabbedObject)
        {
            MoveGrabbedObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, interactableLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false; // Disable gravity while grabbing
                grabbedObject.drag = 10f;        // Add drag for stability
            }
        }
    }

    void MoveGrabbedObject()
    {
        // Raycast from the camera through the mouse cursor
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 basePosition = ray.origin + ray.direction * grabDistance;

        // Offset the base position for "up" and "side" movement
        Vector3 offset = mainCamera.transform.forward * grabHeight + mainCamera.transform.right * grabSideOffset;
        targetPosition = basePosition + offset;

        // Smoothly move the grabbed object toward the target position
        Vector3 newPosition = Vector3.Lerp(grabbedObject.position, targetPosition, Time.deltaTime * followSpeed);
        grabbedObject.MovePosition(newPosition);
    }

    void ReleaseObject()
    {
        if (grabbedObject)
        {
            grabbedObject.useGravity = true; // Re-enable gravity
            grabbedObject.drag = 0f;        // Reset drag
            grabbedObject.velocity = Vector3.zero; // Reset velocity
            grabbedObject = null;           // Release the object
        }
    }
}