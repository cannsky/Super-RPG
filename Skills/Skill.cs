using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill", order = 0)]
public class Skill : ScriptableObject,IHotBar
{
    PlayerAttack playerAttack;
    public static readonly int MaxSkillLevel = 10;
    public Sprite skillIcon;
    public string skillName;
    [TextArea]
    public string skillDescription;
    private int _skillLevel = 0;
    public int skillLevel
    {
        get { return _skillLevel; }
        set
        {
            if (value < 1)
                _skillLevel = 1;
            else if (value > MaxSkillLevel)
                _skillLevel = MaxSkillLevel;
            else _skillLevel = value;
        }
    }
    public int animationNumber = 0;

    List<Effect> effects;

    [Header(header: "Please do not use negative values for any of the fields unless you know what you're really doing!!")]

    public InstantDamageEffect[] instantDamageEffects;
    public DamageOverTimeEffect[] damageOverTimeEffects;
    public InstantRegenerationEffect[] instantRegenerationEffects;
    public RegenerationOverTimeEffect[] regenerationOverTimeEffects;
    public BuffEffect[] buffEffects;
    public WeakeningEffect[] weakeningEffects;
    public MovementBuffEffect[] movementBuffEffects;
    public MovementWeakeningEffect[] movementWeakeningEffects;

    public void RequestSkillExecution(Stats requester)
    {
        if (animationNumber == 0)
            ExecuteSkill(requester);
        else if (requester.TryGetComponent<PlayerAttack>(out playerAttack))
            playerAttack.RequestSkillAttack(this);
        else
        {
            //enemy animated skill
            //not implemented yet
        }
    }

    public void ExecuteSkill(Stats executer)
    {
        if(effects==null)
        {
            effects = new List<Effect>();
            effects.AddRange(instantDamageEffects);
            effects.AddRange(damageOverTimeEffects);
            effects.AddRange(instantRegenerationEffects);
            effects.AddRange(regenerationOverTimeEffects);
            effects.AddRange(buffEffects);
            effects.AddRange(weakeningEffects);
            effects.AddRange(movementBuffEffects);
            effects.AddRange(movementWeakeningEffects);
        }

        foreach (var effect in effects)
            effect.Execute(executer,skillName,skillLevel);
    }

    public void Use() => RequestSkillExecution(Player.Instance);

    public Sprite GetImage() => skillIcon;

    public int GetAmount() => 1;
}