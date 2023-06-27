using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Dialogue", menuName= "Dialogue/Dialogue Tree")]

public class DialogueTree : ScriptableObject
{
    public int dialogueTreeID;
    public List<DialogueText> dialogueTexts = new List<DialogueText>();
}
