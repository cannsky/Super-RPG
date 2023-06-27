using System;
using UnityEngine;

[Serializable]
public class InstantDamageEffect : Effect
{
    [SerializeField] DamageStatTypes damageType;
    [SerializeField] int baseConstantDamage;

    public override void Execute(Stats executer,string skillName,int skillLevel)
    {
        baseEffect = Mathf.RoundToInt(executer.GetStatValue(variableEffectType) * variableEffectMultiplier) + baseConstantDamage;

        finalEffect = Mathf.RoundToInt(baseEffect * SkillAlgorithmManager.BaseStatAlgorithm(
                executer.GetStatValue((AllStatTypes)baseStatType),
                baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier));

        GetStats(executer)?.TakeDamage(damageType, finalEffect);
    }

    public override void Execute(string consumableName)
    {
        baseEffect = Mathf.RoundToInt(Player.Instance.GetStatValue(variableEffectType) * variableEffectMultiplier) + baseConstantDamage;
        GetStats(Player.Instance)?.TakeDamage(damageType, baseEffect);
    }
}
