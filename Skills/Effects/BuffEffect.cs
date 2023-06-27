using System;
using UnityEngine;

[Serializable]
public class BuffEffect : BuffDebuffEffect
{
    [SerializeField] BuffDebuffStatTypes buffStatType;
    [SerializeField] float baseConstantBuff;
    public override void Execute(Stats executer,string skillName ,int skillLevel)
    {
        var result = Calculate(executer, skillLevel, buffStatType, baseConstantBuff);
        GetEffectManager(executer)?.AddEffect(skillName, buffStatType, result.firstElement, result.secondElement);
    }

    public override void Execute(string consumableName)
    {
        var result = Calculate(Player.Instance,buffStatType, baseConstantBuff);
        GetEffectManager(Player.Instance)?.AddEffect(consumableName, buffStatType, result.firstElement, result.secondElement);
    }
}