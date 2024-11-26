using UnityEngine;

public class Stamp : MonoBehaviour
{
    public enum StampType { Pass, Decline }
    public StampType stampType;  // Type of the stamp (Pass or Decline)

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit by the stamp has the "Paper" tag
        if (collision.collider.CompareTag("Paper"))
        {
            Debug.Log("Stamp hit the paper!");

            // Attempt to get the PaperManager component from the collided object
            PaperManager paperManager = collision.collider.GetComponent<PaperManager>();
            if (paperManager != null)
            {
                paperManager.HandleStamp(stampType);  // Call HandleStamp on the PaperManager
            }
            else
            {
                Debug.LogError("PaperManager component not found on the stamped object!");
            }
        }
    }
}
