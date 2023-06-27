using UnityEngine;
using UnityEditor;

public class QuestManager : MonoBehaviour
{
    public static Quest quest;
    
    public static DialogueTree dialogueTree;
    
    public static bool CheckQuestAvailability(Player player, Quest quest, int npcID){
        if(quest.npcID != npcID) return false;
        if(quest.questID == player.playerQuest.questID + 1) return true;
        return false;
    }
    
    public static void GetNextQuest(Player player){
        quest = null;
        string[] questsInfo = AssetDatabase.FindAssets("t:" + typeof(Quest).Name);
        for(int i = 0; i < questsInfo.Length; i++){
            string path = AssetDatabase.GUIDToAssetPath(questsInfo[i]);
            Quest newQuest = AssetDatabase.LoadAssetAtPath<Quest>(path);
            if(player.playerQuest.questID + 1 == newQuest.questID){
                quest = newQuest;
                break;
            }
        }
        if(quest == null) return;
        string[] dialogueTreeInfo = AssetDatabase.FindAssets("t:" + typeof(DialogueTree).Name);
        for(int i = 0; i < dialogueTreeInfo.Length; i++){
            string path = AssetDatabase.GUIDToAssetPath(dialogueTreeInfo[i]);
            DialogueTree newDialogueTree = AssetDatabase.LoadAssetAtPath<DialogueTree>(path);
            if(quest.questID == newDialogueTree.dialogueTreeID){
                dialogueTree = newDialogueTree;
                break;
            }
        }
    }
}
