using UnityEngine;
using UnityEngine.UI;

public class HideImageOnClick : MonoBehaviour
{
    public Image displayImage;  // Reference to the UI Image

    void Start()
    {
        // Ensure the image is visible at the start
        displayImage.enabled = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left mouse click
        {
            // Hide the image when clicked
            displayImage.enabled = false;
        }
    }
}
