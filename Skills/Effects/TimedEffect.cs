using System;
using UnityEngine;


public abstract class TimedEffect : Effect
{
    [SerializeField] protected TimedEffectTypes timedEffectType;
    [SerializeField] protected float baseEffectTime;
    
    //Timed Effect Execution Caches
    protected int baseTime = 0;
    protected int finalTime = 0;

    protected float baseTime1 = 0;
    protected float finalTime1 = 0;

    public override void Execute(Stats executer,string skillName ,int skillLevel)
    {
        throw new NotImplementedException();
    }

    public override void Execute(string consumableName)
    {
        throw new NotImplementedException();
    }

    protected DictionaryNode<float,float> CalculateMovementEffect(Stats executer,int skillLevel,float baseConstantEffect)
    {
        baseEffect1 = executer.GetStatValue(variableEffectType) * variableEffectMultiplier + baseConstantEffect;
        baseTime1 = baseEffectTime;

        finalEffect1 = timedEffectType == TimedEffectTypes.TimeBoosted ? baseEffect1 :
            baseEffect1 * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
            baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier);

        finalTime1 = timedEffectType == TimedEffectTypes.EffectBoosted ? baseTime1 :
            baseTime1 * SkillAlgorithmManager.BaseStatAlgorithm(executer.GetStatValue((AllStatTypes)baseStatType),
            baseStatEffectivenessMultiplier) * SkillAlgorithmManager.SkillLevelAlgorithm(skillLevel, baseSkillLevelEffectivenessMultiplier);

        return new DictionaryNode<float, float>(finalEffect1,finalTime1);
    }

    protected DictionaryNode<float,float> CalculateMovementEffect(Stats executer,float baseConstantEffect)
    {
        baseEffect1 = executer.GetStatValue(variableEffectType) * variableEffectMultiplier + baseConstantEffect;
        return new DictionaryNode<float, float>(finalEffect1, baseEffectTime);
    }
}