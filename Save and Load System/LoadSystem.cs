using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class LoadSystem : MonoBehaviour
{
    //Debug Settings
    [SerializeField] bool debugMode = false;
    static bool _debugMode = false;
    
    private static BinaryFormatter formatter;
    private static FileStream stream;

    private static Player player;
    private static EffectManager effectManager;
    private static PlayerLevelController levelController;
    private static BarUIManager manager;

    private static PlayerData playerData;
    private static EffectManagerData effectManagerData;
    private static LevelControllerData levelData;
    private static BarUIManagerData barUiData;

    private void Start()
    {
        _debugMode = debugMode;
        LoadEverything();
    }

    public static void LoadEverything()
    {
        if(_debugMode)
        {
            player = Player.Instance;
            effectManager = player.GetComponent<EffectManager>();
            levelController = PlayerLevelController.Instance;
            manager = FindObjectOfType<BarUIManager>();

            if (!manager.isStarted)
                manager.Start();
            if (!PlayerLevelController.isStarted)
                levelController.Awake();
            if (!Player.isStarted)
                player.Start();

            LoadPlayer();
            LoadPlayerLevel();
            LoadBarUIManager();
            manager.Load();
        }       
    }

    public static void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Player.data";
        if (File.Exists(path))
        {
            formatter = new BinaryFormatter();
            stream = new FileStream(path, FileMode.Open);
            playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            if(playerData!=null)
            {
                player.quest.isActive = playerData.isQuestActive;
                player.quest.questID = playerData.questID;
                player.quest.questTitle = playerData.questTitle;
                player.quest.questDescription = playerData.questDescription;
                player.quest.experienceReward = playerData.questExperienceReward;
                player.quest.goldReward = playerData.questGoldReward;
                player.ConvertIntegerToQuestGoal(playerData.questGoal);
                Player.ConvertQuestGoalToInteger(player.quest.goal.goalType);
                player.transform.position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);

                player.allStats.armour = playerData.allStats[0];
                player.allStats.attack = playerData.allStats[1];
                player.allStats.crushingDamage = playerData.allStats[2];
                player.allStats.currentEnergy = playerData.allStats[3];
                player.allStats.currentHealth = playerData.allStats[4];
                player.allStats.defence = playerData.allStats[5];
                player.allStats.dexterity = playerData.allStats[6];
                player.allStats.energy = playerData.allStats[7];
                player.allStats.focus = playerData.allStats[8];
                player.allStats.health = playerData.allStats[9];
                player.allStats.healthRegeneration = playerData.allStats[10];
                player.allStats.magicalResistance = playerData.allStats[11];
                player.allStats.magicDamage = playerData.allStats[12];
                player.allStats.manaRegeneration = playerData.allStats[13];
                player.allStats.physicalResistance = playerData.allStats[14];
                player.allStats.piercingDamage = playerData.allStats[15];
                player.allStats.poisonDamage = playerData.allStats[16];
                player.allStats.poisonResistance = playerData.allStats[17];
                player.allStats.rawDexterity = playerData.allStats[18];
                player.allStats.rawFocus = playerData.allStats[19];
                player.allStats.rawStrength = playerData.allStats[20];
                player.allStats.rawVitality = playerData.allStats[21];
                player.allStats.slashDamage = playerData.allStats[22];
                player.allStats.strength = playerData.allStats[23];
                player.allStats.vitality = playerData.allStats[24];

                player.idlestatPoints = playerData.idleStatPoints;

                player.element = (Elements)playerData.element;
            }
        }
        LoadPlayerEffects();
    }

    private static void LoadPlayerEffects()
    {
        string path = Application.persistentDataPath + "/PlayerEffects.data";

        if(File.Exists(path))
        {
            formatter = new BinaryFormatter();
            stream = new FileStream(path, FileMode.Open);
            effectManagerData = formatter.Deserialize(stream) as EffectManagerData;
            stream.Close();

            if(effectManagerData!=null)
            {
                string skillName;
                int type;
                float magnitude;
                float time;
                float currentTime;

                for (int i = 0; i < effectManagerData.movementEffectCount; i++)
                {
                    skillName = effectManagerData.movementEffectSkillNames[i];
                    magnitude = effectManagerData.movementEffectMagnitudes[i];
                    time = effectManagerData.movementEffectTimes[i];
                    currentTime = effectManagerData.movementEffectCurrentTimes[i];

                    effectManager.movementEffects.Add(skillName,
                        new TrictionaryNode<float, float, float>(magnitude, time, currentTime));
                }

                for (int i = 0; i < effectManagerData.buffDebuffEffectCount; i++)
                {
                    skillName = effectManagerData.buffDebuffEffectSkillNames[i];
                    type = effectManagerData.buffDebuffStatTypes[i];
                    magnitude = effectManagerData.buffDebuffEffectMagnitudes[i];
                    time = effectManagerData.buffDebuffEffectTimes[i];
                    currentTime = effectManagerData.buffDebuffEffectCurrentTimes[i];

                    effectManager.buffDebuffEffects.Add
                        (new DictionaryNode<string, BuffDebuffStatTypes>(skillName, (BuffDebuffStatTypes)type),
                        new TrictionaryNode<int, float, float>(Mathf.RoundToInt(magnitude), time, currentTime));
                }

                for (int i = 0; i < effectManagerData.regenerationEffectCount; i++)
                {
                    skillName = effectManagerData.regenerationEffectSkillNames[i];
                    type = effectManagerData.regenerationTypes[i];
                    magnitude = effectManagerData.regenerationEffectMagnitudes[i];
                    time = effectManagerData.regenerationEffectTimes[i];
                    currentTime = effectManagerData.regenerationEffectCurrentTimes[i];

                    effectManager.regenerationEffects.Add
                        (new DictionaryNode<string, RegenerationStatTypes>(skillName, (RegenerationStatTypes)type),
                        new TrictionaryNode<int, int, float>(Mathf.RoundToInt(magnitude), Mathf.RoundToInt(time), currentTime));
                }

                for (int i = 0; i < effectManagerData.damageOverTimeEffectCount; i++)
                {
                    skillName = effectManagerData.damageOverTimeEffectSkillNames[i];
                    type = effectManagerData.damageStatTypes[i];
                    magnitude = effectManagerData.damageOverTimeEffectMagnitudes[i];
                    time = effectManagerData.damageOverTimeEffectTimes[i];
                    currentTime = effectManagerData.damageOverTimeEffectCurrentTimes[i];

                    effectManager.damageOverTimeEffects.Add
                        (new DictionaryNode<string, DamageStatTypes>(skillName, (DamageStatTypes)type),
                        new TrictionaryNode<int, int, float>(Mathf.RoundToInt(magnitude), Mathf.RoundToInt(time), currentTime));
                }
            }
        }
    }

    public static void LoadPlayerLevel()
    {
        string path = Application.persistentDataPath + "/PlayerLevel.data";

        if (File.Exists(path))
        {
            formatter = new BinaryFormatter();
            stream = new FileStream(path, FileMode.Open);
            levelData = formatter.Deserialize(stream) as LevelControllerData;
            stream.Close();

            if(levelData!=null)
            {
                levelController.currentExp = levelData.currentExp;
                levelController.currentLevel = levelData.currentLevel;
                levelController.totalXpRequiredForLevelup = levelData.totalXpRequiredForLevelup;
            }
        }
    }

    public static void LoadBarUIManager()
    {
        string path = Application.persistentDataPath + "/BarUI.data";

        if (File.Exists(path))
        {
            formatter = new BinaryFormatter();
            stream = new FileStream(path, FileMode.Open);
            barUiData = formatter.Deserialize(stream) as BarUIManagerData;
            stream.Close();

            if (barUiData != null)
            {
                manager.expPeriod = barUiData.expPeriod;
            }
        }
    }
}
