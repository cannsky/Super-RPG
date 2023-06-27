using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    Stats stats;
    float deltaTime;
    List<object> removalList = new List<object>();

    //Repeating fields
    DictionaryNode<string, BuffDebuffStatTypes> buffDebuffKeyCache;
    DictionaryNode<string, RegenerationStatTypes> regenKeyCache;
    DictionaryNode<string, DamageStatTypes> damageOverTimeKeyCache;
    TrictionaryNode<float, float, float> movementValueCache;
    TrictionaryNode<int, float, float> buffDebuffValueCache;
    TrictionaryNode<int, int, float> regenValueCache;
    TrictionaryNode<int, int, float> damageOverTimeValueCache;

    public Dictionary<string, TrictionaryNode<float, float, float>> movementEffects = new Dictionary<string, TrictionaryNode<float, float, float>>();
    public Dictionary<DictionaryNode<string, BuffDebuffStatTypes>, TrictionaryNode<int, float, float>> buffDebuffEffects = new Dictionary<DictionaryNode<string, BuffDebuffStatTypes>, TrictionaryNode<int, float, float>>();
    public Dictionary<DictionaryNode<string, RegenerationStatTypes>, TrictionaryNode<int, int, float>> regenerationEffects = new Dictionary<DictionaryNode<string, RegenerationStatTypes>, TrictionaryNode<int, int, float>>();
    public Dictionary<DictionaryNode<string, DamageStatTypes>, TrictionaryNode<int, int, float>> damageOverTimeEffects = new Dictionary<DictionaryNode<string, DamageStatTypes>, TrictionaryNode<int, int, float>>();

    protected virtual void Start()
    {
        stats = GetComponent<Stats>(); 
    }

    void Update()
    {
        deltaTime = Time.deltaTime;

        foreach (var effect in buffDebuffEffects)
        {
            buffDebuffValueCache = effect.Value;
            buffDebuffValueCache.thirdElement += deltaTime;

            if (buffDebuffValueCache.secondElement > buffDebuffValueCache.thirdElement)
                buffDebuffEffects[effect.Key] = buffDebuffValueCache;
            else
                removalList.Add(effect.Key);
        }

        foreach (var removal in removalList)
            RemoveEffect((DictionaryNode<string, BuffDebuffStatTypes>)removal);

        removalList.Clear();

        foreach (var effect in regenerationEffects)
        {
            regenValueCache = effect.Value;
            regenValueCache.thirdElement += deltaTime;

            if (regenValueCache.secondElement > regenValueCache.thirdElement)
                regenerationEffects[effect.Key] = regenValueCache;
            else
                removalList.Add(effect.Key);
        }

        foreach (var removal in removalList)
            RemoveEffect((DictionaryNode<string, RegenerationStatTypes>)removal);

        removalList.Clear();

        foreach (var effect in damageOverTimeEffects)
        {
            damageOverTimeValueCache = effect.Value;
            damageOverTimeValueCache.thirdElement += deltaTime;

            if(damageOverTimeValueCache.thirdElement>=1f)
            {
                stats.TakeDamage(effect.Key.secondElement, damageOverTimeValueCache.firstElement);
                damageOverTimeValueCache.secondElement--;
                damageOverTimeValueCache.thirdElement = 0;
            }
            if (damageOverTimeValueCache.secondElement >= 0)
                damageOverTimeEffects[effect.Key] = damageOverTimeValueCache;
            else
                removalList.Add(effect.Key);
        }

        foreach (var removal in removalList)
            RemoveEffect((DictionaryNode<string, DamageStatTypes>)removal);

        removalList.Clear();

        foreach (var effect in movementEffects)
        {
            movementValueCache = effect.Value;
            movementValueCache.thirdElement += deltaTime;

            if (movementValueCache.secondElement > movementValueCache.thirdElement)
                movementEffects[effect.Key] = movementValueCache;
            else
                removalList.Add(effect.Key);
        }

        foreach (var removal in removalList)
            RemoveEffect((string)removal);

        removalList.Clear();
    }

    #region Movement Effect Methods
    public virtual void AddEffect(string skillName,float movementChange,float time)
    {
        movementValueCache = new TrictionaryNode<float, float, float>(movementChange, time, 0);

        try
        {
            movementEffects.Add(skillName, movementValueCache);
            stats.MovementMultiplier += (movementChange / 100f);
            /*
             * we may also do it like stats.MovementMultiplier = (movementChange*stats.MovementMultiplier/100f);
             * But that would mean the speed growth will increase with stacked movement effects which may even vary within themselves which is not 
             * a perfect application in my very opinion
             */
        }
        catch (ArgumentException)
        {
            movementEffects[skillName] = movementValueCache;
        }
    }

    public void RemoveEffect(string skillName)
    {
        stats.MovementMultiplier -= (movementEffects[skillName].firstElement / 100f);
        movementEffects.Remove(skillName);
    }
    #endregion

    #region Buff-Debuff Effect Methods
    public virtual void AddEffect(string skillName,BuffDebuffStatTypes buffDebuffType,int change,float time)
    {
        buffDebuffKeyCache = new DictionaryNode<string, BuffDebuffStatTypes>(skillName, buffDebuffType);
        buffDebuffValueCache = new TrictionaryNode<int, float, float>(change, time, 0);

        try
        {
            buffDebuffEffects.Add(buffDebuffKeyCache, buffDebuffValueCache);
            ChangeStat((AllStatTypes)buffDebuffType, change);
        }
        catch (ArgumentException)
        {
            buffDebuffEffects[buffDebuffKeyCache] =buffDebuffValueCache;
        }
    }

    public void RemoveEffect(string skillName,BuffDebuffStatTypes buffDebuffType) => RemoveEffect(new DictionaryNode<string, BuffDebuffStatTypes>(skillName, buffDebuffType));

    public void RemoveEffect(DictionaryNode<string, BuffDebuffStatTypes> key)
    {
        ChangeStat((AllStatTypes)key.secondElement, - buffDebuffEffects[key].firstElement);
        buffDebuffEffects.Remove(key);
    }
    #endregion

    #region Regeneration Effect Methods
    public virtual void AddEffect(string skillName,RegenerationStatTypes regenType,int regeneration,int time)
    {
        regenKeyCache = new DictionaryNode<string, RegenerationStatTypes>(skillName, regenType);
        regenValueCache = new TrictionaryNode<int, int, float>(regeneration, time, 0);
        try
        {
            regenerationEffects.Add(regenKeyCache, regenValueCache);
            ChangeStat((AllStatTypes)regenType, regeneration);
        }
        catch (ArgumentException)
        {
            regenerationEffects[regenKeyCache] = regenValueCache;
        }
    }

    public void RemoveEffect(string skillName, RegenerationStatTypes regenType) => RemoveEffect(new DictionaryNode<string, RegenerationStatTypes>(skillName, regenType));

    public void RemoveEffect(DictionaryNode<string, RegenerationStatTypes> key)
    {
        ChangeStat((AllStatTypes)key.secondElement, -regenerationEffects[key].firstElement);
        regenerationEffects.Remove(key);
    }
    #endregion

    #region Damage OverTime Effect Methods
    public virtual void AddEffect(string skillName,DamageStatTypes damageType,int damage,int time)
    {
        damageOverTimeKeyCache = new DictionaryNode<string, DamageStatTypes>(skillName, damageType);
        damageOverTimeValueCache = new TrictionaryNode<int, int, float>(damage, time, 0);
        try
        {
            damageOverTimeEffects.Add(damageOverTimeKeyCache, damageOverTimeValueCache);
        }
        catch (ArgumentException)
        {
            damageOverTimeEffects[damageOverTimeKeyCache] = damageOverTimeValueCache;
        }
    }

    public void RemoveEffect(string skillName, DamageStatTypes damageType) => RemoveEffect(new DictionaryNode<string, DamageStatTypes>(skillName, damageType));

    public void RemoveEffect(DictionaryNode<string, DamageStatTypes> key)
    {
        damageOverTimeEffects.Remove(key);
    }
    #endregion

    public void ChangeStat(AllStatTypes stat,int amount)
    {
        switch (stat)
        {
            case AllStatTypes.Strength:
                stats.allStats.strength += amount;
                break;
            case AllStatTypes.Vitality:
                stats.allStats.vitality += amount;
                break;
            case AllStatTypes.Dexterity:
                stats.allStats.dexterity += amount;
                break;
            case AllStatTypes.Focus:
                stats.allStats.focus += amount;
                break;
            case AllStatTypes.Attack:
                stats.allStats.attack += amount;
                break;
            case AllStatTypes.Defence:
                stats.allStats.defence += amount;
                break;
            case AllStatTypes.Armour:
                stats.allStats.armour += amount;
                break;
            case AllStatTypes.PhysicalResistance:
                stats.allStats.physicalResistance += amount;
                break;
            case AllStatTypes.MagicalResistance:
                stats.allStats.magicalResistance += amount;
                break;
            case AllStatTypes.PoisonResistance:
                stats.allStats.poisonResistance += amount;
                break;
            case AllStatTypes.MagicDamage:
                stats.allStats.magicDamage += amount;
                break;
            case AllStatTypes.SlashDamage:
                stats.allStats.slashDamage += amount;
                break;
            case AllStatTypes.PiercingDamage:
                stats.allStats.piercingDamage += amount;
                break;
            case AllStatTypes.CrushingDamage:
                stats.allStats.crushingDamage += amount;
                break;
            case AllStatTypes.PoisonDamage:
                stats.allStats.poisonDamage += amount;
                break;
            case AllStatTypes.HealthRegen:
                stats.allStats.healthRegeneration += amount;
                break;
            case AllStatTypes.EnergyRegen:
                stats.allStats.manaRegeneration += amount;
                break;
            case AllStatTypes.Health:
                stats.allStats.health += amount;
                break;
            case AllStatTypes.Mana:
                stats.allStats.energy += amount;
                break;
        }
    }

    public virtual void Heal(int regen)
    {
        stats.Heal(regen);
    }

    public virtual void HealEnergy(int regen)
    {
        stats.HealEnergy(regen);
    }
}