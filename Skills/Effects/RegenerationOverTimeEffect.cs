using System;
using UnityEngine;

[Serializable]
public class RegenerationOverTimeEffect : TimedEffect
{
    [SerializeField] RegenerationStatTypes regenerationType;
    [SerializeField] float baseConstantRegeneration;

    public override void Execute(Stats executer,string skillName ,int skillLevel)
    {
        baseEffect1 = executer.GetStatValue(variableEffectType) * variableEffectMultiplier + baseConstantRegeneration;
         baseTime= Mathf.RoundToInt(baseEffectTime);

        finalEffect1 = timedEffectType == TimedEffectTypes.TimeBoosted ? baseEffect1 :
            baseEffect1 * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
            baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier);


        finalTime1 = timedEffectType == TimedEffectTypes.EffectBoosted ? baseTime1 :
            baseTime1 * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
            baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier);

        GetEffectManager(executer)?.AddEffect(skillName, regenerationType, Mathf.RoundToInt(finalEffect1), Mathf.RoundToInt(finalTime1));
    }

    public override void Execute(string consumableName)
    {
        baseEffect1 = Player.Instance.GetStatValue(variableEffectType) * variableEffectMultiplier + baseConstantRegeneration;

        GetEffectManager(Player.Instance)?.AddEffect(consumableName, Mathf.RoundToInt(baseEffect1), Mathf.RoundToInt(baseEffectTime));
    }
}
