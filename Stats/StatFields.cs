using System;

[Serializable]
public struct StatFields
{
    [NonSerialized] public int vitality;
    [NonSerialized] public int strength;
    [NonSerialized] public int focus;
    [NonSerialized] public int dexterity;

    public int rawVitality;
    public int rawStrength;
    public int rawFocus;
    public int rawDexterity;

    public int health;
    public int energy;

    [NonSerialized] public int currentHealth;
    [NonSerialized] public int currentEnergy;

    public int attack;
    public int defence;
    public int armour;

    public int slashDamage;
    public int piercingDamage;
    public int crushingDamage;
    public int poisonDamage;
    public int magicDamage;

    public int physicalResistance;
    public int magicalResistance;
    public int poisonResistance;

    public int healthRegeneration;
    public int manaRegeneration;

    public int CalculateTotalPhysicalDamage()
    {
        ConvertRawStats();

        return ((-4) * strength) + slashDamage + piercingDamage + crushingDamage;
    }

    public void ConvertRawStats()
    {
        strength += rawStrength;
        focus += rawFocus;
        vitality += rawVitality;
        dexterity += rawDexterity;

        health += (rawVitality * 12);
        energy += (rawFocus * 12);
        attack += (rawStrength + (rawDexterity * 2));
        defence += (rawDexterity * 2);
        magicDamage += (rawFocus * 3);
        slashDamage += (rawStrength * 2);
        piercingDamage += (rawStrength * 2);
        crushingDamage += (rawStrength * 2);

        rawDexterity = 0;
        rawFocus = 0;
        rawStrength = 0;
        rawVitality = 0;
    }

    //I've created new StatsFields variables inside operator overloading methods because I'm not yet sure whether this should be a struct or a class

    public static StatFields operator +(StatFields a, StatFields b)
    {
        StatFields result = new StatFields();

        result.vitality = a.vitality + b.vitality;
        result.strength = a.strength + b.strength;
        result.focus = a.focus + b.focus;
        result.dexterity = a.dexterity + b.dexterity;
        result.rawVitality = a.rawVitality + b.rawVitality;
        result.rawStrength = a.rawStrength + b.rawStrength;
        result.rawFocus = a.rawFocus + b.rawFocus;
        result.rawDexterity = a.rawDexterity + b.rawDexterity;
        result.armour = a.armour + b.armour;
        result.attack = a.attack + b.attack;
        result.crushingDamage = a.crushingDamage + b.crushingDamage;
        result.defence = a.defence + b.defence;
        result.energy = a.energy + b.energy;
        result.health = a.health + b.health;
        result.currentEnergy = a.currentEnergy + b.currentEnergy;
        result.currentHealth = a.currentHealth + b.currentHealth;
        result.healthRegeneration = a.healthRegeneration + b.healthRegeneration;
        result.magicalResistance = a.magicalResistance + b.magicalResistance;
        result.magicDamage = a.magicDamage + b.magicDamage;
        result.manaRegeneration = a.manaRegeneration + b.manaRegeneration;
        result.physicalResistance = a.physicalResistance + b.physicalResistance;
        result.piercingDamage = a.piercingDamage + b.piercingDamage;
        result.poisonDamage = a.poisonDamage + b.poisonDamage;
        result.poisonResistance = a.poisonResistance + b.poisonResistance;
        result.slashDamage = a.slashDamage + b.slashDamage;

        return result;
    }

    public static StatFields operator *(StatFields a, int b)
    {
        StatFields result = new StatFields();

        result.vitality = a.vitality * b;
        result.strength = a.strength * b;
        result.focus = a.focus * b;
        result.dexterity = a.dexterity * b;
        result.rawVitality = a.rawVitality * b;
        result.rawStrength = a.rawStrength * b;
        result.rawFocus = a.rawFocus * b;
        result.rawDexterity = a.rawDexterity * b;
        result.armour = a.armour * b;
        result.attack = a.attack * b;
        result.crushingDamage = a.crushingDamage * b;
        result.defence = a.defence * b;
        result.energy = a.energy * b;
        result.health = a.health * b;
        result.currentEnergy = a.currentEnergy * b;
        result.currentHealth = a.currentHealth * b;
        result.healthRegeneration = a.healthRegeneration * b;
        result.magicalResistance = a.magicalResistance * b;
        result.magicDamage = a.magicDamage * b;
        result.manaRegeneration = a.manaRegeneration * b;
        result.physicalResistance = a.physicalResistance * b;
        result.piercingDamage = a.piercingDamage * b;
        result.poisonDamage = a.poisonDamage * b;
        result.poisonResistance = a.poisonResistance * b;
        result.slashDamage = a.slashDamage * b;

        return result;
    }

    public static StatFields operator *(int b, StatFields a)
    {
        return a * b;
    }

    public static StatFields operator -(StatFields a, StatFields b)
    {
        return a + ((-1) * b);
    }
}