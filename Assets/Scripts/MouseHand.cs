using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening; // Import DOTween namespace

public class MouseHand : MonoBehaviour
{
    public GameObject playerCursor; // Reference to the 3D cursor object
    private Camera mainCam;

    // Adjustable offsets for the cursor
    public float zOffset = 0.23f; // Depth from the camera
    public float xOffset = 0f; // Horizontal offset
    public float yOffset = 0f; // Vertical offset
    public float moveSpeed = 1f; // Speed for Z-axis movement
    public float maxForwardDistance = 2f; // Maximum distance the cursor can move forward
    public float returnDelay = 1f; // Time to wait before returning to original depth
    public float returnDuration = 0.5f; // Duration of the smooth return

    public float remainingReturnTime = 0f; // Time remaining before the cursor returns to original position

    private bool isMovingForward = false; // Tracks if left click is held
    private bool hasClicked = false; // Tracks if the mouse has been clicked (to trigger return timer)

    private float currentZOffset; // Tracks the current Z offset of the cursor

    private Coroutine returnCoroutine; // To keep track of the return coroutine

    public float handspeedoffset = 1.5f;

    public Animator handAnimator;
    void Start()
    {
        // Ensure references are assigned
        if (playerCursor == null)
            playerCursor = GameObject.Find("playerCursor");

        handAnimator = playerCursor.GetComponentInChildren<Animator>();

        mainCam = Camera.main;

        if (mainCam == null)
            Debug.LogError("Main Camera not found!");

        // Initialize the Z offset
        currentZOffset = zOffset;
    }

    void Update()
    {
        // Update the cursor position every frame
        UpdateCursorPosition();
        /*
        // Handle Z-axis movement when left mouse button is held
        if (isMovingForward)
        {
            MoveCursorForward();
        }*/

        // Check if left click is pressed or released
        HandleInput();
    }

    private void UpdateCursorPosition()
    {

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            playerCursor.transform.position = hit.point;
        }
        /*
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        playerCursor.transform.position = mousePosition;*/
        /*
        // Read mouse position from Input System
        Vector2 mousePosition = handspeedoffset * Mouse.current.position.ReadValue();

        // Convert screen space to world space with the updated Z offset
        Vector3 cursorPosition = mainCam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, currentZOffset));

        // Apply the X and Y offsets
        cursorPosition.x += xOffset;
        cursorPosition.y += yOffset;

        // Update the 3D cursor's position
        playerCursor.transform.position = cursorPosition;
        */
    }

    private void MoveCursorForward()
    {
        // Move the Z offset forward until it reaches the max distance
        float step = moveSpeed * Time.deltaTime; // Calculate the movement step
        if (currentZOffset < zOffset + maxForwardDistance)
        {
            currentZOffset += step;
            currentZOffset = Mathf.Min(currentZOffset, zOffset + maxForwardDistance); // Clamp to max distance
        }
    }

    private void HandleInput()
    {
        // Check if the left mouse button is pressed
        if (Mouse.current.leftButton.isPressed)
        {
            handAnimator.SetTrigger("Grab");
            isMovingForward = true;
            hasClicked = true; // Mark that the button has been clicked

            // If the left mouse button was just pressed, stop any active reset coroutines
            if (returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine); // Stop any existing coroutines
                returnCoroutine = null; // Clear coroutine reference
            }

            // Kill any DOTween animations if the cursor is moving forward again
            DOTween.Kill(this); // Stop animations specific to this script
        }
        else
        {
            isMovingForward = false; // Stop moving forward when button is released

            // Start the return coroutine only if the mouse was clicked
            if (hasClicked && returnCoroutine == null)
            {
                returnCoroutine = StartCoroutine(ReturnToOriginalDepthAfterDelay());
            }
        }
    }

    private IEnumerator ReturnToOriginalDepthAfterDelay()
    {
        remainingReturnTime = returnDelay; // Set the remaining time to the return delay

        // Wait for the specified return delay, updating the remaining time
        while (remainingReturnTime > 0f)
        {
            remainingReturnTime -= Time.deltaTime; // Decrease the remaining time
            yield return null; // Wait for the next frame
        }

        // Smoothly reset the Z offset back to its original value using DOTween
        DOTween.To(() => currentZOffset, x => currentZOffset = x, zOffset, returnDuration).SetEase(Ease.OutQuad);

        remainingReturnTime = 0f; // Reset remaining time once the return is complete
        returnCoroutine = null; // Reset the coroutine reference
        hasClicked = false; // Reset the click tracker
    }
}

