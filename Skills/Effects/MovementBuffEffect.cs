using System;
using UnityEngine;

[Serializable]
public class MovementBuffEffect : TimedEffect
{

    [SerializeField] float baseConstantMovementBuff;

    public override void Execute(Stats executer,string skillName,int skillLevel)
    {
        var result = CalculateMovementEffect(executer, skillLevel, baseConstantMovementBuff);
        GetEffectManager(executer)?.AddEffect(skillName, result.firstElement, result.secondElement);
    }

    public override void Execute(string consumableName)
    {
        var result = CalculateMovementEffect(Player.Instance, baseConstantMovementBuff);
        GetEffectManager(Player.Instance)?.AddEffect(consumableName, result.firstElement, result.secondElement);
    }
}