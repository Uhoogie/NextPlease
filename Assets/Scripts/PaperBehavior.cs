using UnityEngine;
using DG.Tweening; // Ensure DOTween is installed

public class PaperBehavior : MonoBehaviour
{
    public Transform zoomPosition; // Position to zoom to (now based on the camera)
    private Camera mainCamera; // Reference to the camera
    private bool isZoomedIn = false; // To track if the paper is zoomed in
    private Vector3 originalPosition; // Store the original position for returning
    private Vector3 originalScale; // Store the original scale for returning

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
        originalPosition = transform.position; // Store the original position
        originalScale = transform.localScale; // Store the original scale

        // You can adjust this to make the paper move a little further in front of the camera
        zoomPosition = new GameObject("ZoomPosition").transform;
        zoomPosition.position = mainCamera.transform.position + mainCamera.transform.forward * 2f; // Adjust this distance
    }

    void Update()
    {
        // Detect mouse click on the paper
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Debug: Draw the ray in the Scene view
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f); // Ray is drawn in the scene view for 1 second

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) // Check if the hit object is this paper
                {
                    ToggleZoom();
                }
            }
        }
    }

    // Toggles the zoom effect when the paper is clicked
    private void ToggleZoom()
    {
        if (!isZoomedIn)
        {
            // Zoom to the specified position and scale up the paper
            transform.DOMove(zoomPosition.position, 1f).SetEase(Ease.InOutQuad);
            transform.DOScale(Vector3.one * 3f, 1f).SetEase(Ease.OutBack); // Smooth scaling effect
            isZoomedIn = true;
        }
        else
        {
            // Return to original position and scale down the paper
            transform.DOMove(originalPosition, 1f).SetEase(Ease.InOutQuad);
            transform.DOScale(originalScale, 1f).SetEase(Ease.InBack); // Smooth scaling effect
            isZoomedIn = false;
        }
    }
}
