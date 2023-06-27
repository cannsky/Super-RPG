using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public Text npcNameText;
    public Text dialogueText;
    public Text leftButtonText;
    public Text rightButtonText;
    public Player player;
    private int npcID;
    private TreeNode root;
    private bool dialogueEnded = false;
    private bool questAvaliable = false;
    private DialogueTree dialogueTree;

    public void StartDialogue(string npcName, int npcID)
    {
        dialogueEnded = false;
        questAvaliable = false;
        dialogueUI.SetActive(!dialogueUI.activeSelf);
        npcNameText.text = npcName;
        this.npcID = npcID;
        if(!player.playerQuest.isActive) QuestManager.GetNextQuest(player);
        else GetNPCDialogue();
        if(QuestManager.quest != null && QuestManager.CheckQuestAvailability(player, QuestManager.quest, npcID)){
            questAvaliable = true;
            this.root = new TreeNode(QuestManager.dialogueTree.dialogueTexts[0].id, QuestManager.dialogueTree.dialogueTexts[0]);
            for(int i = 1; i < QuestManager.dialogueTree.dialogueTexts.Count; i++){
                root.addNode(root, new TreeNode(QuestManager.dialogueTree.dialogueTexts[i].id, QuestManager.dialogueTree.dialogueTexts[i]));
            }
        }
        else GetNPCDialogue();
        DisplayNextSentence(0);
    }
    
    void GetNPCDialogue(){
        string[] dialogueTreeInfo = AssetDatabase.FindAssets("t:" + typeof(DialogueTree).Name);
        for(int i = 0; i < dialogueTreeInfo.Length; i++){
            string path = AssetDatabase.GUIDToAssetPath(dialogueTreeInfo[i]);
            DialogueTree newDialogueTree = AssetDatabase.LoadAssetAtPath<DialogueTree>(path);
            if(npcID + 1000 == newDialogueTree.dialogueTreeID){
                dialogueTree = newDialogueTree;
                break;
            }
        }
        if(dialogueTree != null){
            this.root = new TreeNode(dialogueTree.dialogueTexts[0].id, dialogueTree.dialogueTexts[0]);
            for(int i = 1; i < dialogueTree.dialogueTexts.Count; i++){
                root.addNode(root, new TreeNode(dialogueTree.dialogueTexts[i].id, dialogueTree.dialogueTexts[i]));
            }
        }
    }

    public void DisplayNextSentence(int option)
    {
        if(dialogueEnded) EndDialogue();
        else{
            if(option > 0) root = (option == 1) ? root.leftNode : root.rightNode;
            this.UpdateUI(root.dialogueText);
        }
    }
    
    void UpdateUI(DialogueText dialogueText){
        this.dialogueText.text = dialogueText.text;
        if(dialogueText.options.Length == 0){
            dialogueEnded = true;
            this.leftButtonText.text = "I'll be going.";
            this.rightButtonText.text = "Thanks!";
            return;
        }
        this.leftButtonText.text = dialogueText.options[0];
        this.rightButtonText.text = dialogueText.options[1];
    }

    void EndDialogue()
    {
        if(questAvaliable){
            player.playerQuest = new PlayerQuest(true, QuestManager.quest.questID, QuestManager.quest.questTitle, QuestManager.quest.questDescription, QuestManager.quest.experienceReward, QuestManager.quest.goldReward, QuestManager.quest.repoputationReward, QuestManager.quest.goal);
            Player.questGoal = Player.ConvertQuestGoalToInteger(QuestManager.quest.goal.goalType);
        }
        dialogueUI.SetActive(!dialogueUI.activeSelf);
    }

}
