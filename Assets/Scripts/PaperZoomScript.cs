using UnityEngine;
using DG.Tweening;

public class PaperZoomScript : MonoBehaviour
{
    [Header("References")]
    public Transform paper; // The paper object
    public Camera mainCamera; // The main camera
    public Transform zoomPosition; // Position where the paper should zoom to in front of the camera
    public float zoomDuration = 1f; // Duration of the zoom effect
    public float zoomFOV = 30f; // Field of View when zoomed in
    public float originalFOV = 60f; // The camera's original Field of View

    private bool isZoomedIn = false; // To track if zoom is currently active

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Get the main camera if it's not set
        }
    }

    void Update()
    {
        // Detect right mouse button click (button index 1)
        if (Input.GetMouseButtonDown(1)) // Right mouse button click (button index 1)
        {
            ToggleZoom();
        }
    }

    // Toggles the zoom effect on right-click
    private void ToggleZoom()
    {
        if (!isZoomedIn)
        {
            // Zoom the camera in by changing FOV and move the paper towards the camera
            mainCamera.DOFieldOfView(zoomFOV, zoomDuration).SetEase(Ease.InOutQuad); // Camera zoom
            paper.DOMove(zoomPosition.position, zoomDuration).SetEase(Ease.InOutQuad); // Move paper towards the camera
            paper.DOScale(Vector3.one * 3f, zoomDuration).SetEase(Ease.OutBack); // Scale up the paper

            isZoomedIn = true;
        }
        else
        {
            // Reset camera FOV and move the paper back to its original position
            mainCamera.DOFieldOfView(originalFOV, zoomDuration).SetEase(Ease.InOutQuad); // Reset camera zoom
            paper.DOMove(paper.position, zoomDuration).SetEase(Ease.InOutQuad); // Return paper to original position
            paper.DOScale(Vector3.one, zoomDuration).SetEase(Ease.InBack); // Reset paper scale

            isZoomedIn = false;
        }
    }
}


