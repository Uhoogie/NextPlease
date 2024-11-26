using UnityEngine;
using DG.Tweening;

public class PaperManager : MonoBehaviour
{
    [Header("References")]
    public Transform paper; // The paper object (assign in Inspector or dynamically at runtime)
    public Transform deskPosition; // Position where the paper sits on the desk
    public Transform zoomPosition; // Position in front of the camera when zoomed

    [Header("Settings")]
    public float destructionDelay = 5f; // Delay before destroying the paper

    public bool isStamped = false; // Tracks if the paper has been stamped
    public bool isPassed = false; // Tracks the result (Pass or Decline)

    void Start()
    {
        // Validate references
        if (paper == null)
        {
            Debug.LogError("Paper reference is missing! Please assign it in the Inspector.");
            return;
        }

        // Check if the assigned paper object has a Collider
        Collider paperCollider = paper.GetComponent<Collider>();
        if (paperCollider == null)
        {
            Debug.LogError($"The assigned paper object '{paper.name}' is missing a Collider!");
        }
        else
        {
            Debug.Log($"Paper '{paper.name}' has a Collider assigned and ready.");
        }

        // Optional: Check for Rigidbody (if necessary for physics interactions)
        Rigidbody paperRigidbody = paper.GetComponent<Rigidbody>();
        if (paperRigidbody == null)
        {
            Debug.LogWarning($"The assigned paper object '{paper.name}' does not have a Rigidbody. Physics interactions might not work as expected.");
        }
    }

    /// <summary>
    /// Called when the paper is stamped.
    /// </summary>
    /// <param name="stampType">Type of the stamp (Pass or Decline).</param>
    public void HandleStamp(Stamp.StampType stampType)
    {
        if (isStamped)
        {
            Debug.LogWarning("Paper is already stamped. Ignoring further stamps.");
            return;
        }

        // Mark the paper as stamped
        Debug.Log($"Handling Stamp: {stampType}");
        isStamped = true;
        isPassed = (stampType == Stamp.StampType.Pass);
        if (isPassed)
        {
            FindFirstObjectByType<EnemyNav>().ShowPassedDialogue();
            paper.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            FindFirstObjectByType<EnemyNav>().ShowDeniedDialogue();
            paper.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        // Log the result for debugging
        Debug.Log(isPassed ? "Paper marked as Passed." : "Paper marked as Declined.");

        // Destroy the paper after the specified delay
        if (paper != null)
        {
            Debug.Log($"Destroying paper '{paper.name}' after {destructionDelay} seconds.");
            Destroy(paper.gameObject, destructionDelay);
        }
        else
        {
            Debug.LogError("Paper reference is missing or null! Cannot destroy.");
        }
    }
}