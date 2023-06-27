using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueText
{
    public int id;
    [TextArea(3, 10)]
    public string text;
    public DialogueEffect dialogueEffect;
    [TextArea(3, 10)]
    public string[] options = new string[2];
}
