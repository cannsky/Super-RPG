using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerQuest
{
    public bool isActive = false;
    public bool isSideQuest = false;
    public int questID;
    public string questTitle;
    public string questDescription;
    public int experienceReward;
    public int goldReward;
    public int repoputationReward;
    public QuestGoal goal;
    public PlayerQuest(bool isActive, int questID, string questTitle, string questDescription, int experienceReward, int goldReward, int repoputationReward, QuestGoal questGoal){
        this.isActive = isActive;
        this.isSideQuest = false;
        this.questID = questID;
        this.questTitle = questTitle;
        this.questDescription = questDescription;
        this.experienceReward = experienceReward;
        this.goldReward = goldReward;
        this.repoputationReward = repoputationReward;
        this.goal = questGoal;
    }
}
