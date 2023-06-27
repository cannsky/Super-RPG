using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //Player Stats Data
    public int level;
    public int health;
    public float[] position;
    //Player Quest Data
    public bool isQuestActive;
    public int questID;
    public string questTitle;
    public string questDescription;
    public int questExperienceReward;
    public int questGoldReward;
    public int questGoal;
    
    public PlayerData(Player player){
        this.level = player.level;
        this.health = player.health;
        
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        
        this.isQuestActive = player.playerQuest.isActive;
        this.questID = player.playerQuest.questID;
        this.questTitle = player.playerQuest.questTitle;
        this.questDescription = player.playerQuest.questDescription;
        this.questExperienceReward = player.playerQuest.experienceReward;
        this.questGoldReward = player.playerQuest.goldReward;
        this.questGoal = Player.questGoal;
    }
}
