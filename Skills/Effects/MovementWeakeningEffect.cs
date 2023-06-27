using System;
using UnityEngine;

[Serializable]
public class MovementWeakeningEffect : TimedEffect
{
    [SerializeField] float baseConstantMovementWeakening;
    
    public override void Execute(Stats executer,string skillName,int skillLevel)
    {
        var result = CalculateMovementEffect(executer, skillLevel, baseConstantMovementWeakening);
        GetEffectManager(executer)?.AddEffect(skillName, -result.firstElement, result.secondElement);
    }

    public override void Execute(string consumableName)
    {
        var result = CalculateMovementEffect(Player.Instance, baseConstantMovementWeakening);
        GetEffectManager(Player.Instance)?.AddEffect(consumableName, -result.firstElement, result.secondElement);
    }
}
