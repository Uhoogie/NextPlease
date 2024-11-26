using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public string initialDialogue;
    public string approvalReaction;
    public string rejectionReaction;

    public void SpeakInitialDialogue()
    {
        Debug.Log(initialDialogue); // Replace with UI dialogue display
    }

    public void ReactToDecision(bool approved)
    {
        Debug.Log(approved ? approvalReaction : rejectionReaction); // Replace with UI dialogue display
    }
}
