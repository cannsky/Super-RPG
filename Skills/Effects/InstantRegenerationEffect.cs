using System;
using UnityEngine;

[Serializable]
public class InstantRegenerationEffect : Effect
{
    [SerializeField] RegenerationStatTypes regenerationType;
    [SerializeField] int baseConstantRegeneration;

    public override void Execute(Stats executer,string skillName ,int skillLevel)
    {
        baseEffect = Mathf.RoundToInt(executer.GetStatValue(variableEffectType) * variableEffectMultiplier) + baseConstantRegeneration;

        finalEffect = Mathf.RoundToInt(baseEffect * SkillAlgorithmManager.BaseStatAlgorithm(
                executer.GetStatValue((AllStatTypes)baseStatType),
                baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier));

        GetEffectManager(executer)?.Heal(finalEffect);
    }

    public override void Execute(string consumableName)
    {
        baseEffect = Mathf.RoundToInt(Player.Instance.GetStatValue(variableEffectType) * variableEffectMultiplier) + baseConstantRegeneration;
        if (regenerationType == RegenerationStatTypes.HealthRegen)
            GetEffectManager(Player.Instance)?.Heal(baseEffect);
        else
            GetEffectManager(Player.Instance)?.HealEnergy(baseEffect);
    }
}
