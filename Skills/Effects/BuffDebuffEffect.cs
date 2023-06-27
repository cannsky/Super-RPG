using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffDebuffEffect : TimedEffect
{
    protected enum BuffDebuffMode
    {
        ConstantValue,
        Percentage
    }

    [SerializeField] protected BuffDebuffMode buffDebuffMode;

    public override void Execute(Stats executer,string skillName,int skillLevel)
    {
        base.Execute(executer,skillName,skillLevel);
    }

    public override void Execute(string consumableName)
    {
        base.Execute(consumableName);
    }

    protected DictionaryNode<int,float> Calculate(Stats executer,int skillLevel,BuffDebuffStatTypes statType,float baseConstantEffect)
    {
        baseEffect1 = executer.GetStatValue(variableEffectType) * variableEffectMultiplier + baseConstantEffect;
        baseTime1 = baseEffectTime;

        finalEffect1 = timedEffectType == TimedEffectTypes.TimeBoosted ? baseEffect1 :
            baseEffect1 * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
            baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier);

        finalEffect1 = timedEffectType == TimedEffectTypes.TimeBoosted ? finalEffect1 :
             buffDebuffMode == BuffDebuffMode.ConstantValue ? finalEffect1 :
             executer.GetStatValue((AllStatTypes)statType) * finalEffect1 / 100f;

        finalTime1 = timedEffectType == TimedEffectTypes.EffectBoosted ? baseTime1 :
            baseTime1 * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
            baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier);

        return new DictionaryNode<int, float>(Mathf.RoundToInt(finalEffect1), finalTime1);
    }

    protected DictionaryNode<int,float> Calculate(Stats executer,BuffDebuffStatTypes statType,float baseConstantEffect)
    {
        baseEffect1 = executer.GetStatValue(variableEffectType) * variableEffectMultiplier + baseConstantEffect;

        return new DictionaryNode<int, float>(Mathf.RoundToInt(baseEffect1), baseEffectTime);
    }
}