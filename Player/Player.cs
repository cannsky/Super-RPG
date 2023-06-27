using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(CharacterController),typeof(PlayerAttack),typeof(PlayerMovement))]
public partial class Player : Stats
{
    #region Cached Fields
    PlayerStateController stateController;
    PlayerMovement playerMovement;
    PlayerAttack playerAttack;
    SelectTracer tracer;
    BarUIManager manager;
    Settings settings;
    #endregion

    #region Player Select-Attack
    public float interactionDistance = 2.5f;
    [NonSerialized] public bool _autoChase;
    public bool autoChase 
    {
        get => _autoChase;
        set
        {
            _autoChase = value;
        }
    }
    #endregion

    #region Others
    public static bool isStarted = false;

    #endregion

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    public override void Start()
    {
        if (isStarted)
            return;
        
        tracer = SelectTracer.Instance;
        playerAttack = PlayerAttack.Instance;
        playerMovement = PlayerMovement.Instance;
        stateController = PlayerStateController.Instance;
        settings = Settings.Instance;
        equipmentStats = new List<StatFields>();
        PlayerLevelController.Instance.levelup += LeveledUp;
        manager = FindObjectOfType<BarUIManager>();

        //GetCurrentEquipmentInfo();
        allStats = startingStats + GetTotalEquipmentStats();
        allStats.ConvertRawStats();
        base.Start();
        isStarted = true;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.L)) Cursor.visible = !Cursor.visible;
        if (targetStats is EnemyStats)
        {
            if (settings.autoChase && autoChase)
                playerMovement.AutoChase(GetTargetPosition());
            else if (settings.autoAttack && CheckInteractionAvailable())
                playerAttack.RequestMeleeAttack(false);
        }
        else if (targetStats is NPCStats)
        {
            if (settings.autoChase && autoChase)
                playerMovement.AutoChase(GetTargetPosition());
            else if (settings.autoInteract && CheckInteractionAvailable())
                InteractWithNPC();
        }
        else
        {
            if (tracer.enabled)
            {
                tracer.UnloadCompletely();
                autoChase = false;
            }
        }
        //manager.Load();
    }

    private void OnApplicationQuit()
    {
        if (isStarted)
        {
            SaveSystem.SavePlayer(this);
            isStarted = false;
        }
    }

    void LoadPlayerData()
    {
        LoadSystem.LoadPlayer();
    }

    public void AttackRequest()
    {
        if (CheckInteractionAvailable())
        {
            if (targetStats is EnemyStats)
                playerAttack.RequestMeleeAttack(true);
            else if (targetStats is NPCStats)
                InteractWithNPC();
        }
        else
            autoChase = true;
    }

    public void Interact()
    {
        if (targetStats is NPCStats && CheckInteractionAvailable())
            InteractWithNPC();
        else
            InteractWithItem();
    }

    public void InteractWithNPC() => targetStats.GetComponent<DialogueTrigger>().TriggerDialogue();

    private void InteractWithItem()
    {
        var items = Physics.OverlapSphere(transform.position, interactionDistance, LayerMask.GetMask("ItemModel"));

        if (items == null) return;

        items[0].GetComponent<Item>().Pickup();
    }

    public bool CheckInteractionAvailable() => interactionDistance + Mathf.Epsilon >= Helper.CalculateDistance(transform.position,GetTargetPosition());
}

[RequireComponent(typeof(CharacterController), typeof(PlayerAttack), typeof(PlayerMovement))]
public partial class Player : Stats
{
    public Quest quest;
    public static int questGoal;

    public void ConvertIntegerToQuestGoal(int goal)
    {
        switch (goal)
        {
            case 1:
                this.quest.goal.goalType = GoalType.Walk;
                break;
            default:
                Debug.Log("Quest Goal is not assigned.");
                break;
        }
    }

    public static void ConvertQuestGoalToInteger(GoalType goal)
    {
        switch (goal)
        {
            case GoalType.Walk:
                Player.questGoal = 1;
                break;
            default:
                Debug.Log("Quest Goal is not assigned.");
                break;
        }
    }
}

[RequireComponent(typeof(CharacterController), typeof(PlayerAttack), typeof(PlayerMovement))]
public partial class Player : Stats
{
    static Player instance;
    public static Player Instance { get => instance; }

    public int statPointsPerLevel;
    [SerializeField] StatFields startingStats;
    [NonSerialized] public List<StatFields> equipmentStats;
    [NonSerialized] public int idlestatPoints = 0;

    public void UpdateStats(int newStr, int newDex, int newFoc, int newVit, int newIdlePoints)
    {
        allStats.rawStrength = newStr - allStats.strength;
        allStats.rawFocus = newFoc - allStats.focus;
        allStats.rawDexterity = newDex - allStats.dexterity;
        allStats.rawVitality = newVit - allStats.vitality;

        allStats.ConvertRawStats();
        idlestatPoints = newIdlePoints;
    }

    public void GetCurrentEquipmentInfo()
    {
        if (equipmentStats is object)
            equipmentStats.Clear();
        Inventory.Instance.equipmentSlots.Where(s => s.item.GetType().IsSubclassOf(typeof(Equipment))).ToList()
            .ForEach(s => equipmentStats.Add(((Equipment)s.item).equipmentStats));
    }

    public StatFields GetTotalEquipmentStats()
    {
        StatFields total = new StatFields();

        foreach (var item in equipmentStats)
            total += item;

        return total;
    }

    public void EquipGear(Equipment equipment)
    {
        allStats = allStats + equipment.equipmentStats;
        allStats.ConvertRawStats();
    }

    public void UnequipGear(Equipment equipment)
    {
        allStats = allStats - equipment.equipmentStats;
        allStats.ConvertRawStats();
    }

    private void LeveledUp()
    {
        idlestatPoints += statPointsPerLevel;
        allStats.currentHealth = allStats.health;
        allStats.currentEnergy = allStats.energy;
        FindObjectOfType<BarUIManager>().LevelUpLoad();
    }

    protected override void Die()
    {

    }
    protected override void Destruction()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerator DeathAnimation()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerator DeathEffect()
    {
        throw new NotImplementedException();
    }
}