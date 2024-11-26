using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;
using UnityEngine.UI; // DOTween namespace

public class EnemyNav : MonoBehaviour
{
    public NavMeshAgent navAgent;           // The NavMeshAgent for pathfinding
    public List<Transform> patrolPoints = new List<Transform>(); // Patrol points for the NPC
    public int patrolIndex = 0;             // Current patrol point
    public float waitTime = 3f;             // Time to wait at each patrol point
    public GameObject secondSprite;         // Optional second sprite for behavior

    public GameObject paperPrefab;          // The paper prefab to instantiate
    public Transform paperSpawnPosition;    // The spawn position for the paper (e.g., on the table)
    public Transform paperZoomPosition;     // The zoom position for the paper when clicked

    private bool isWaiting = false;         // Flag to check if NPC is waiting
    private bool isPausedAtPoint2 = false;  // Flag to check if NPC is paused at patrol point 2
    private float waitTimer = 0f;           // Timer to wait at patrol point

    public GameObject dialogueBox;
    public TextMeshProUGUI textGUI;

    public string introDialogueString;
    public string acceptedDialogueString;
    public string deniedDialogueString;


    void Start()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        if (patrolPoints.Count > 0)
        {
            navAgent.SetDestination(patrolPoints[patrolIndex].position);
        }
        dialogueBox = GameObject.FindGameObjectWithTag("Dialogue");
        textGUI = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();

        DisableDialogueBox();
    }

    void Update()
    {
        if (patrolPoints.Count == 0) return;

        // Check if the NPC is at patrol point 2
        if (patrolIndex == 3 && navAgent.remainingDistance < 0.5f && !navAgent.pathPending)
        {
            if (!isPausedAtPoint2)
            {
                // Pause at patrol point 2
                isPausedAtPoint2 = true;
                navAgent.isStopped = true;

                textGUI.text = introDialogueString;
                EnableDialogueBox();
                // Instantiate the paper
                InstantiatePaper();
            }

            // Wait for player to press 'D' to continue
            if (Input.GetKeyDown(KeyCode.D))
            {
                ContinuePatrol();
            }
        }

        // Handle normal patrol movement
        if (!isPausedAtPoint2 && navAgent.remainingDistance < 0.5f && !navAgent.pathPending)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitAtPoint());
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueBox.activeSelf == true)
            {
                DisableDialogueBox();
            }
        }
    }

    private void ContinuePatrol()
    {
        isPausedAtPoint2 = false;
        navAgent.isStopped = false; // Resume NPC movement
        patrolIndex++;
        if (patrolIndex < patrolPoints.Count)
        {
            navAgent.SetDestination(patrolPoints[patrolIndex].position);
        }
        else
        {
            Destroy(gameObject); // Destroy NPC if patrol is finished
        }
    }

    private IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        waitTimer = waitTime;

        while (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            yield return null;
        }

        patrolIndex++;
        if (patrolIndex < patrolPoints.Count)
        {
            navAgent.SetDestination(patrolPoints[patrolIndex].position);
        }
        else
        {
            Destroy(gameObject);
        }

        isWaiting = false;
    }

    private void InstantiatePaper()
    {
        if (paperPrefab != null && paperSpawnPosition != null)
        {
            Debug.Log("Instantiating paper at position: " + paperSpawnPosition.position); // Debug: Check spawn position
            GameObject paper = Instantiate(paperPrefab, paperSpawnPosition.position, Quaternion.identity);

            // Get the PaperBehavior script and assign the zoom position
            PaperBehavior paperBehavior = paper.GetComponent<PaperBehavior>();
            if (paperBehavior != null)
            {
                // Set the zoom position after the paper is instantiated
                paperBehavior.zoomPosition = paperZoomPosition;
                Debug.Log("PaperBehavior assigned zoom position."); // Debug: Confirm assignment
            }
            else
            {
                Debug.LogWarning("Paper prefab is missing the PaperBehavior script!"); // Debug: Warning if missing script
            }
        }
        else
        {
            Debug.LogWarning("Paper prefab or spawn position is not set!"); // Debug: Warning if either is missing
        }
    }

    private void OnDestroy()
    {
        DisableDialogueBox();

        if (secondSprite != null)
        {
            secondSprite.SetActive(true);
        }
    }

    public void ShowPassedDialogue()
    {
        EnableDialogueBox();
        textGUI.text = acceptedDialogueString;
    }

    public void ShowDeniedDialogue()
    {
        EnableDialogueBox();

        textGUI.text = deniedDialogueString;
    }

    private void EnableDialogueBox()
    {
        dialogueBox.GetComponent<Image>().enabled = true;
        textGUI.enabled = true;
    }

    private void DisableDialogueBox()
    {
        dialogueBox.GetComponent<Image>().enabled = false;
        textGUI.enabled = false;
    }
}
