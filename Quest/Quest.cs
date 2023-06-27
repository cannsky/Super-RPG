using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Quest", menuName="Quest/New Quest")]

[System.Serializable]
public class Quest : ScriptableObject
{
    public bool isSideQuest = false;
    public int questID;
    public int npcID;
    public string questTitle;
    public string questDescription;
    public int experienceReward;
    public int goldReward;
    public int repoputationReward;
    public QuestGoal goal;
}
