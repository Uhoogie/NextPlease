using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import for scene management
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    public List<GameObject> npcPrefabs; // List of NPC prefabs to spawn
    private GameObject currentNPC;
    private NavMeshAgent navAgent;

    // Time delay for NPC behavior after reaching desk (e.g., time for player to interact)
    public float npcInteractionTime = 3f;

    // A common list of patrol points (assuming all NPCs share common points, adjust as necessary)
    public List<Transform> commonPatrolPoints;

    private int npcIndex = 0; // Keep track of which NPC to spawn next

    void Start()
    {
        // Start by spawning the first NPC
        SpawnNextNPC();
    }

    // Method to spawn the next NPC in the list (forcing the spawn)
    public void SpawnNextNPC()
    {
        if (npcPrefabs.Count == 0)
        {
            Debug.LogWarning("No NPC prefabs assigned!");
            return;
        }

        if (npcIndex >= npcPrefabs.Count)
        {
            Debug.Log("All NPCs have been spawned! Transitioning to EndScreen...");
            LoadEndScreen(); // Load the EndScreen scene when all NPCs are spawned
            return;
        }

        // Instantiate the NPC from the prefab list using npcIndex
        currentNPC = Instantiate(npcPrefabs[npcIndex], transform.position, Quaternion.identity);
        currentNPC.SetActive(true); // Ensure the NPC is active in the scene

        // Get the EnemyNav component to assign patrol points and set destination
        EnemyNav enemyNav = currentNPC.GetComponent<EnemyNav>();
        if (enemyNav != null)
        {
            // Assign patrol points to the NPC
            enemyNav.patrolPoints = new List<Transform>(commonPatrolPoints);

            // Ensure the NPC starts its path immediately
            if (enemyNav.navAgent != null && enemyNav.patrolPoints.Count > 0)
            {
                enemyNav.navAgent.SetDestination(enemyNav.patrolPoints[0].position);
            }
            else
            {
                Debug.LogError("NavMeshAgent or patrol points missing on NPC: " + currentNPC.name);
            }
        }
        else
        {
            Debug.LogError("EnemyNav script missing on NPC: " + currentNPC.name);
        }

        // Increment npcIndex to move to the next NPC for the next spawn
        npcIndex++;
    }

    // Method to dismiss the current NPC (move to exit and destroy it)
    public void DismissNPC()
    {
        if (currentNPC != null)
        {
            // Move the NPC to the exit position
            navAgent = currentNPC.GetComponent<NavMeshAgent>();
            if (navAgent != null)
            {
                // Add your exit logic here if needed
            }

            // After some time (npcInteractionTime), destroy the NPC
            StartCoroutine(DestroyNPCAfterDelay(npcInteractionTime));
            currentNPC = null;
        }
        else
        {
            SpawnNextNPC(); // Immediately spawn the next NPC if there's no current NPC
        }
    }

    // Coroutine to destroy NPC after a delay
    private IEnumerator DestroyNPCAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for interaction time
        Destroy(currentNPC); // Destroy the NPC
        SpawnNextNPC(); // After destroying the current NPC, spawn the next NPC
    }

    // Debug to manually spawn an NPC by pressing 'S'
    void Update()
    {
        // Press 'S' to spawn the next NPC manually for testing
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnNextNPC();
        }
    }

    // Method to load the EndScreen scene
    private void LoadEndScreen()
    {
        SceneManager.LoadScene("EndScreen"); // Replace "EndScreen" with your scene's actual name
    }
}