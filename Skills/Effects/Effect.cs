using System;
using UnityEngine;

public abstract class Effect
{
    protected enum EffectTarget
    {
        Self,
        TargetStats
    }

    [SerializeField] EffectTarget target;
    [SerializeField] protected BaseStatTypes baseStatType;

    [SerializeField] protected AllStatTypes variableEffectType;
    [SerializeField] protected float variableEffectMultiplier;

    [SerializeField] protected float baseStatEffectivenessMultiplier=1f;
    [SerializeField] protected float baseSkillLevelEffectivenessMultiplier=1f;

    //Effect Execution Caches
    protected int baseEffect = 0;
    protected int finalEffect = 0;

    protected float baseEffect1 = 0;
    protected float finalEffect1 = 0;
    
    public abstract void Execute(Stats executer,string skillName,int skillLevel);
    public abstract void Execute(string consumableName);

    /*
     * AoE damage?
     * TargetStats related skills?
     */

    protected EffectManager GetEffectManager(Stats executer)
    {
        return target == EffectTarget.Self ? executer.effectManager : executer.targetStats?.effectManager;
    }

    protected Stats GetStats(Stats executer)
    {
        return target == EffectTarget.Self ? executer : executer.targetStats;
    }
}