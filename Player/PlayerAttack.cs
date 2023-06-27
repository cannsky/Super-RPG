using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] int queueLimit;
    Skill currentSkill;
    int _remainingQueueAttacks = 0;
    public int remainingQueueAttacks
    {
        get { return _remainingQueueAttacks; }
        set { _remainingQueueAttacks = value < 0 ? 0 : value > queueLimit ? queueLimit : value; }
    }

    PlayerStateController stateController;
    Player player;
    Settings settings;
    static PlayerAttack instance;
    public static PlayerAttack Instance { get => instance; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }
    private void Start()
    {
        stateController = PlayerStateController.Instance;
        player = Player.Instance;
        settings = Settings.Instance;
    }

    public void Attack(bool isCombo=false,bool isRunningAttack=false)
    {
        player.TryHit(player.targetStats);
    }

    public bool RequestMeleeAttack(bool queuable)
    {
        /*
         *If attack request is not successful (you should return if attack request is confirmed in the PlayerStateController)
         * then increase remainingQueueAttacks by one for every single attack request till it's equal to queueLimit,
         * then some events inside the PlayerStateController will call back that the current attack is over and the next attack request will be delivered
         * right away so is the queue system working flawlessly (haha hope so)
         */
        bool requestAccepted = stateController.UpdateAttackInfo();

        if (queuable && settings.meleeAttackQueueing && !requestAccepted)
            remainingQueueAttacks++;

        return requestAccepted;
    }

    public void TryDropFromQueue()
    {
        if (remainingQueueAttacks > 0)
            if (RequestMeleeAttack(this))
                remainingQueueAttacks--;
    }

    public void RequestSkillAttack(Skill skill)
    {
        currentSkill = skill;
        stateController.UpdateAttackInfo(skill.animationNumber);
    }

    public void SkillAttack()
    {
        currentSkill.ExecuteSkill(player);
        currentSkill = null;
    }
}