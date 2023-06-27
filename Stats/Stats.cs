using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public delegate void TakeDamage(float health,float currentHealth);

public abstract class Stats : MonoBehaviour
{
    [NonSerialized] public Stats targetStats;
    [NonSerialized] public EffectManager effectManager;
    [NonSerialized] public StatFields allStats;

    [SerializeField] public Elements element;
    [SerializeField] protected float deathTime;
    [SerializeField] protected float deathEffectTime;
    [SerializeField] protected Renderer meshRenderer;
    [SerializeField] protected Material deathMaterial;

    float movementMultiplier = 1f;
    public virtual float MovementMultiplier
    {
        get => movementMultiplier;
        set => movementMultiplier = value;
    }

    [SerializeField] float regenTime = 3f;
    float regenTimer = 0;

    public event TakeDamage healthChange;

    public virtual void Start()
    {
        effectManager = GetComponent<EffectManager>();
        allStats.ConvertRawStats();
        allStats.currentEnergy = allStats.energy;
        allStats.currentHealth = allStats.health;
    }

    public Vector3 GetTargetPosition()
    {
        return targetStats? targetStats.transform.position : transform.position;
    }

    protected virtual void Update()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer >= regenTime)
        {
            if (allStats.currentHealth < allStats.health)
            {
                allStats.currentHealth += allStats.manaRegeneration;
                allStats.currentHealth = (allStats.currentHealth > allStats.health) ? allStats.health : allStats.currentHealth;
                regenTimer = 0;
            }
            if (allStats.currentEnergy < allStats.energy)
            {
                allStats.currentEnergy += allStats.manaRegeneration;
                allStats.currentEnergy = (allStats.currentEnergy > allStats.energy) ? allStats.energy : allStats.currentEnergy;
                regenTimer = 0;
            }
            OnHealthChanged();
        }
    }

    public virtual bool TryHit(Stats damageReceiver, bool isCombo = false, bool isRunningAttack = false, float otherDamageMultiplier = 1f)
    {
        float hitChance = (float)allStats.attack / ((float)allStats.attack + (float)damageReceiver.allStats.defence);

        if (UnityEngine.Random.Range(0, 1) > hitChance)
            return false;

        int reducedDamage = 0;

        CalculateReducedDamage(allStats.CalculateTotalPhysicalDamage(), damageReceiver.allStats.physicalResistance, ref reducedDamage);
        CalculateReducedDamage(allStats.poisonDamage, damageReceiver.allStats.poisonResistance, ref reducedDamage);
        CalculateReducedDamage(allStats.magicDamage, damageReceiver.allStats.magicalResistance, ref reducedDamage);

        if (damageReceiver.allStats.armour < 1453) reducedDamage -= Mathf.RoundToInt((float)reducedDamage * (float)damageReceiver.allStats.armour / 1453f);
        else reducedDamage = Mathf.RoundToInt(reducedDamage * 0.02f);
        ExertDamage(damageReceiver, reducedDamage);
        return true;
    }

    private void CalculateReducedDamage(float damage,float resistance,ref int currentReducedDamage)
    {
        float temp = damage - resistance;
        temp = temp < damage * 0.3f ? damage * 0.3f : temp;
        currentReducedDamage += Mathf.RoundToInt(temp);
    }

    private void ExertDamage(Stats damageReceiver, int damage) => damageReceiver?.TakeDamage(damage);

    public void TakeDamage(DamageStatTypes damageType,int damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        allStats.currentHealth -= damage;
        Debug.Log("dmg : " + damage);
        OnHealthChanged();

        if (allStats.currentHealth <= 0)
            Die();
    }

    public void Heal(int regen)
    {
        allStats.currentHealth += regen;
        allStats.currentHealth = (allStats.currentHealth > allStats.health) ? allStats.health : allStats.currentHealth;
        OnHealthChanged();
    }

    public void HealEnergy(int regen)
    {
        allStats.currentEnergy += regen;
        allStats.currentEnergy = (allStats.currentEnergy > allStats.energy) ? allStats.energy : allStats.currentEnergy;
    }

    protected virtual void OnHealthChanged()
    {
        healthChange?.Invoke(allStats.health,allStats.currentHealth);
    }

    public int GetStatValue(AllStatTypes type)
    {
        int value = 0;

        switch (type)
        {
            case AllStatTypes.Strength:
                value = allStats.strength;
                break;
            case AllStatTypes.Vitality:
                value = allStats.vitality;
                break;
            case AllStatTypes.Dexterity:
                value = allStats.dexterity;
                break;
            case AllStatTypes.Focus:
                value = allStats.focus;
                break;
            case AllStatTypes.Attack:
                value = allStats.attack;
                break;
            case AllStatTypes.Defence:
                value = allStats.defence;
                break;
            case AllStatTypes.Armour:
                value = allStats.armour;
                break;
            case AllStatTypes.PhysicalResistance:
                value = allStats.physicalResistance;
                break;
            case AllStatTypes.MagicalResistance:
                value = allStats.magicalResistance;
                break;
            case AllStatTypes.PoisonResistance:
                value = allStats.poisonResistance;
                break;
            case AllStatTypes.MagicDamage:
                value = allStats.magicDamage;
                break;
            case AllStatTypes.SlashDamage:
                value = allStats.slashDamage;
                break;
            case AllStatTypes.PiercingDamage:
                value = allStats.piercingDamage;
                break;
            case AllStatTypes.CrushingDamage:
                value = allStats.crushingDamage;
                break;
            case AllStatTypes.PoisonDamage:
                value = allStats.poisonDamage;
                break;
            case AllStatTypes.HealthRegen:
                value = allStats.healthRegeneration;
                break;
            case AllStatTypes.EnergyRegen:
                value = allStats.manaRegeneration;
                break;
            case AllStatTypes.Health:
                value = allStats.health;
                break;
            case AllStatTypes.Mana:
                value = allStats.energy;
                break;
        }
        return value;
    }

    protected abstract void Die();
    protected abstract void Destruction();

    protected abstract IEnumerator DeathAnimation();
    protected abstract IEnumerator DeathEffect();
}