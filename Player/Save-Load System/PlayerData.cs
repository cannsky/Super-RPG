
[System.Serializable]
public class PlayerData
{
    public float[] position;

    //Player Quest Data
    public bool isQuestActive;
    public int questID;
    public string questTitle;
    public string questDescription;
    public int questExperienceReward;
    public int questGoldReward;
    public int questGoal;

    //Player Stats Data
    public int[] allStats;
    public int element;
    public int idleStatPoints;

    public PlayerData(Player player)
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        allStats = new int[25];
        allStats[0] = player.allStats.armour;
        allStats[1] = player.allStats.attack;
        allStats[2] = player.allStats.crushingDamage;
        allStats[3] = player.allStats.currentEnergy;
        allStats[4] = player.allStats.currentHealth;
        allStats[5] = player.allStats.defence;
        allStats[6] = player.allStats.dexterity;
        allStats[7] = player.allStats.energy;
        allStats[8] = player.allStats.focus;
        allStats[9] = player.allStats.health;
        allStats[10] = player.allStats.healthRegeneration;
        allStats[11] = player.allStats.magicalResistance;
        allStats[12] = player.allStats.magicDamage;
        allStats[13] = player.allStats.manaRegeneration;
        allStats[14] = player.allStats.physicalResistance;
        allStats[15] = player.allStats.piercingDamage;
        allStats[16] = player.allStats.poisonDamage;
        allStats[17] = player.allStats.poisonResistance;
        allStats[18] = player.allStats.rawDexterity;
        allStats[19] = player.allStats.rawFocus;
        allStats[20] = player.allStats.rawStrength;
        allStats[21] = player.allStats.rawVitality;
        allStats[22] = player.allStats.slashDamage;
        allStats[23] = player.allStats.strength;
        allStats[24] = player.allStats.vitality;

        this.element = (int)player.element;
        idleStatPoints = player.idlestatPoints;

        this.isQuestActive = player.quest.isActive;
        this.questID = player.quest.questID;
        this.questTitle = player.quest.questTitle;
        this.questDescription = player.quest.questDescription;
        this.questExperienceReward = player.quest.experienceReward;
        this.questGoldReward = player.quest.goldReward;
        this.questGoal = Player.questGoal;
    }
}
