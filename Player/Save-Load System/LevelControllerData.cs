using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelControllerData
{
    public int currentExp;
    public int totalXpRequiredForLevelup;
    public int currentLevel;

    public LevelControllerData(PlayerLevelController player)
    {
        currentExp = player.currentExp;
        totalXpRequiredForLevelup = player.totalXpRequiredForLevelup;
        currentLevel=player.currentLevel;
    }
}
