using System;
using UnityEngine;

public delegate void Gain(float totalExpRequiredForLevelUp,float currentExp);
public delegate void LevelUp();
public class PlayerLevelController : MonoBehaviour
{
    //Cached Fields
    [NonSerialized]public int currentLevel;
    [NonSerialized]public const int START_LEVEL = 1;
    [NonSerialized]public int currentExp=0;
    [NonSerialized]public int totalXpRequiredForLevelup;
    [NonSerialized] public static bool isStarted = false;
    public const int MAX_LEVEL = 50;


    public event LevelUp levelup;
    public event Gain gainExp;
    static PlayerLevelController instance;
    public static PlayerLevelController Instance { get => instance; }
    public void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);

        currentLevel = START_LEVEL;
        CalculateTotalXpForNextLevel();
        isStarted = true;
    }

    //Whenever leveled up,this method will be called and new amount of xp that's required to level up again will be calculated within it
    public void CalculateTotalXpForNextLevel()
    {
        CheckIfMaxLevel(() =>
        {
            totalXpRequiredForLevelup = (int)(Mathf.Round((11.398232f * Mathf.Pow(currentLevel, 3) + currentLevel * 5.572330744f)*2.58905891f));
        });
    }

    //When something is done that results in gaining xp,this method will be called by the script the thing is done in
    public void GainXp(int ExpAmount)
    {
        CheckIfMaxLevel(() =>
        {
            currentExp += ExpAmount;
            if (currentExp >= totalXpRequiredForLevelup)
            {
                LevelUp();
            }
            else
                OnGainExperience(totalXpRequiredForLevelup,currentExp);
        });
    }

    //This is automatically called whenever currentxp>=totalXpRequiredForLevelup

    public void LevelUp()
    {
        CheckIfMaxLevel(() =>
        {
            currentExp = currentExp - totalXpRequiredForLevelup;
            currentLevel++;
            CalculateTotalXpForNextLevel();
            Onlevelup();
            if (currentExp >= totalXpRequiredForLevelup)
            {
                LevelUp();
            }
        });
    }


    //This method simply checks if the max level is reached or not,the way it's used help reduce redundancy
    public void CheckIfMaxLevel(Action method)
    {
        if (currentLevel !=MAX_LEVEL)
        {
            method();
        }
    }

    protected virtual void Onlevelup()
    {
        levelup?.Invoke();
    }

    protected virtual void OnGainExperience(float totalExp,float currentExp)
    {
        gainExp?.Invoke(totalExp,currentExp);
    }

    private void OnApplicationQuit()
    {
        if(isStarted)
        {
            SaveSystem.SaveLevelController(this);
            isStarted = false;
        }       
    }
}
