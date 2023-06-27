using System;
using UnityEngine;

[CreateAssetMenu(fileName ="Experience Points Balancer",menuName ="Experience",order =0)]
public class MobExperiencePointBalancer : ScriptableObject
{
    //Config Params
    public float tier1;
    public float tier2;
    public float tier3;
    public float tier4;
    public float tier5;
    public float tier6;
    public float tier7;
    public float tier8;
    public float tier9;
    public float tier10;
    public float subTier1;
    public float subTier2;
    public float subTier3;
    public float subTier4;
    public float subTier5; 

    public void GiveExperienceToPlayer(PlayerLevelController player, int mobLevel,float mobExperienceConstant)
    {
        float levelDifferenceMultiplier =
            (player.currentLevel - mobLevel >= 9) ? 0 :
            (player.currentLevel - mobLevel > 5 && player.currentLevel - mobLevel < 9) ? 0.001f * (player.currentLevel - mobLevel) :
            (player.currentLevel - mobLevel == 5) ? 0.143f :
            (player.currentLevel - mobLevel == 4) ? 0.297f :
            (player.currentLevel - mobLevel == 3) ? 0.447f :
            (player.currentLevel - mobLevel == 2) ? 0.614f :
            (player.currentLevel - mobLevel == 1) ? 0.799f :
            (player.currentLevel - mobLevel == 0) ? 1f :
            (player.currentLevel - mobLevel == -1) ? 1.083f :
            (player.currentLevel - mobLevel == -2) ? 1.156f :
            (player.currentLevel - mobLevel == -3) ? 1.233f :
            (player.currentLevel - mobLevel == -4) ? 1.314f :
            (player.currentLevel - mobLevel == -5) ? 1.444f :
            (1.444f + (mobLevel-player.currentLevel - 5) * 0.05f);

        int mobTier = (int)((mobLevel - 1) / 5) + 1;

        float tierMultiplier =
            mobTier == 1 ? tier1 :
            mobTier == 2 ? tier2 :
            mobTier == 3 ? tier3 :
            mobTier == 4 ? tier4 :
            mobTier == 5 ? tier5 :
            mobTier == 6 ? tier6 :
            mobTier == 7 ? tier7 :
            mobTier == 8 ? tier8 :
            mobTier == 9 ? tier9 :
            tier10;

        int mobSubtier = ((mobLevel - 1) % 5) + 1;

        float subTierMultiplier =
            mobSubtier == 1 ? subTier1 :
            mobSubtier == 2 ? subTier2 :
            mobSubtier == 3 ? subTier3 :
            mobSubtier == 4 ? subTier4 :
            subTier5;

        player.GainXp((int)Mathf.Round(levelDifferenceMultiplier * tierMultiplier * subTierMultiplier * mobExperienceConstant));
    }


}
