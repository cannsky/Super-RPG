using System;
using UnityEngine;

[Serializable]
public class WeakeningEffect : BuffDebuffEffect
{
    [SerializeField] BuffDebuffStatTypes weakeningStatType;
    [SerializeField] float baseConstantWeakening;

    public override void Execute(Stats executer,string skillName ,int skillLevel)
    {
        var result = Calculate(executer, skillLevel, weakeningStatType, baseConstantWeakening);
        GetEffectManager(executer)?.AddEffect(skillName, weakeningStatType, -result.firstElement, result.secondElement);
    }

    public override void Execute(string consumableName)
    {
        var result = Calculate(Player.Instance, weakeningStatType, baseConstantWeakening);
        GetEffectManager(Player.Instance)?.AddEffect(consumableName, weakeningStatType, -result.firstElement, result.secondElement);
    }
}