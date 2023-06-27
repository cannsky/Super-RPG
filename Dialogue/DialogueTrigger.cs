using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int npcID;
    public string npcName;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(npcName, npcID);
    }
}
