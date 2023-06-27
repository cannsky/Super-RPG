using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class DamageOverTimeEffect : TimedEffect
{
    [SerializeField] DamageStatTypes damageType;
    [SerializeField] int baseConstantDamage;
    
    public override void Execute(Stats executer,string skillName ,int skillLevel)
    {
        baseEffect = Mathf.RoundToInt(executer.GetStatValue(variableEffectType) * variableEffectMultiplier) + baseConstantDamage;
        baseTime = Mathf.RoundToInt(baseEffectTime);

        finalEffect = timedEffectType == TimedEffectTypes.TimeBoosted ? baseEffect :
            Mathf.RoundToInt(baseEffect * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
                baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier));

        finalTime = timedEffectType == TimedEffectTypes.EffectBoosted ? baseTime :
            Mathf.RoundToInt(baseTime * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
            baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier));

        GetEffectManager(executer)?.AddEffect(skillName, damageType, finalEffect, finalTime);
    }

    public override void Execute(string consumableName)
    {
        baseEffect = Mathf.RoundToInt(Player.Instance.GetStatValue(variableEffectType) * variableEffectMultiplier) + baseConstantDamage;
        baseTime = Mathf.RoundToInt(baseEffectTime);
        GetEffectManager(Player.Instance)?.AddEffect(consumableName, damageType, baseEffect, baseTime);
    }
}