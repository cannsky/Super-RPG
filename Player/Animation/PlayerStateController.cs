using System.Collections;
using UnityEngine;
using System.Linq;

public class PlayerStateController : MonoBehaviour
{
    [SerializeField] Animator[] animators;
    PlayerAnimationEventController eventController;
    PlayerMovement playerMovement;
    static PlayerStateController instance;
    public static PlayerStateController Instance { get => instance; }
    #region States

    //Configuration Fields
    float timeElapsed = 0;
    int weaponLayerIndex;
    int baseLayerIndex;
    string newAnimationName = "";
    Animator realAnimator;
    bool chance;
    #region Movement

    private bool _idle=true;
    public bool idle
    {
        get { return _idle; }
        set
        {
            _idle = value;

            if(value==false)
                timeIdled = 0;
        }
    }

    public bool holdingWeapon { get; set; } = false;
    private bool _moving;
    public bool moving
    {
        get { return _moving; }
        set
        {
            _moving = value;
            if (value == false)
            {
                jogging = false;
                running = false;
                runningFast = false;
                speed = 0;
                if (combat == false)
                    idle = true;
            }
            else
            {
                idle = false;
                combat = false;
            }
        }
    }
    private bool _jogging;
    public bool jogging
    {
        get { return _jogging; }
        set
        {
            _jogging = value;
            if (value)
            {
                moving = true;
                speed = playerMovement.joggingSpeed;
            }
            else timeJogged = 0;
        }
    }
    private bool _running;
    public bool running
    {
        get { return _running; }
        set
        {
            _running = value;
            if (value) speed = playerMovement.runningSpeed;
        }
    }
    private bool _runningFast;
    public bool runningFast
    {
        get { return _runningFast; }
        set
        {
            _runningFast = value;
            if(value)
            {
                speed = playerMovement.runningFastSpeed;
                timeFastRun = 0;
            }
        }
    }

    //Movement Configuration Fields
    [Header("Movement Configuration Parameters")]
    [SerializeField] float joggingTime = 0.7f;
    [SerializeField] float fastRunLimit = 8f;
    [SerializeField] float idleTimeToRemoveWeapon = 5f;
    public bool shifting { private get; set; }
    float timeJogged = 0;
    float timeFastRun = 0;
    float speed;
    float _timeIdled = 0;
    
    public float timeIdled
    {
        get { return _timeIdled; }
        set
        {
            _timeIdled = value;
            if(value>=idleTimeToRemoveWeapon)
            {
                _timeIdled = 0;
                holdingWeapon = !holdingWeapon;
            }
        }
    }

    #endregion

    #region Combat

    private bool _combat;
    public bool combat
    {
        get { return _combat; }
        set
        {
            _combat = value;
            if (value == false)
            {
                _attack = false;
                _meleeAttack = false;
                _runningAttack = false;
                _combo = false;
                _skillAttack = false;
                _defense = false;
                _damage = false;
                _knockDown = false;
                _knockDownStandup = false;
                _dodge = false;
                if (moving == false)
                    idle = true;
            }
            else
            {
                if (!dodge) playerMovement.isRotating = true;//that has a flaw(we don't know if the assigned field is being set to true for the first time)
                idle = false;
                moving = false;
            }
        }
    }

    #region Attack

    private bool _attack;
    public bool attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
            if(value==false)
            {
                _meleeAttack = false;
                _runningAttack = false;
                _combo = false;
                _skillAttack = false;

                if (_defense == false) combat = false;
            }
            else
            {
                combat = true;
                holdingWeapon = true;
            }
        }
    }

    private bool _meleeAttack;
    public bool meleeAttack
    {
        get { return _meleeAttack; }
        set
        {
            _meleeAttack = value;
            if (value) attack = true;
            else if (!(_combo || _skillAttack || _runningAttack)) attack = false;
        }
    }
    private bool _runningAttack;
    public bool runningAttack
    {
        get { return _runningAttack; }
        set
        {
            _runningAttack = value;
            if (value) attack = true;
            else if (!(_combo || _skillAttack || _meleeAttack)) attack = false;
        }
    }
    private bool _combo;
    public bool combo
    {
        get { return _combo; }
        set
        {
            _combo = value;
            if (value) attack = true;
            else if (!(_runningAttack || _skillAttack || _meleeAttack)) attack = false;
        }
    }
    private bool _skillAttack;
    public bool skillAttack
    {
        get { return _skillAttack; }
        set
        {
            _skillAttack = value;
            if (value) attack = true;
            else if (!(_combo || _runningAttack || _meleeAttack)) attack = false;
        }
    }

    //Attack Configuration Fields
    string currentMovingAnimationName = "";
    bool canAttack;
    
    [Header("Attack Configuration Parameters")]
    [Range(0,1000)]
    [SerializeField] int comboChance=100;
    [Header("Melee Attack Interruption Possibilities")]
    [Range(0, 1000)]
    [SerializeField] int meleeAttackBeforeDamageInterruptionChanceByMovement = 50;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackAfterDamageInterruptionChanceByMovement = 90;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackBeforeDamageInterruptionChanceByAttack = 0;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackAfterDamageInterruptionChanceByAttack = 90;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackBeforeDamageInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackAfterDamageInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackBeforeDamageInterruptionChanceByDamage = 200;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackAfterDamageInterruptionChanceByDamage = 200;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackBeforeDamageInterruptionChanceByDodge = 700;
    [Range(0, 1000)]
    [SerializeField] int meleeAttackAfterDamageInterruptionChanceByDodge = 900;
    [Header("Running Attack Interruption Possibilities")]
    [Range(0, 1000)]
    [SerializeField] int runningAttackBeforeDamageInterruptionChanceByMovement = 0;
    [Range(0, 1000)]
    [SerializeField] int runningAttackAfterDamageInterruptionChanceByMovement = 80;
    [Range(0, 1000)]
    [SerializeField] int runningAttackBeforeDamageInterruptionChanceByAttack = 0;
    [Range(0, 1000)]
    [SerializeField] int runningAttackAfterDamageInterruptionChanceByAttack = 80;
    [Range(0, 1000)]
    [SerializeField] int runningAttackBeforeDamageInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int runningAttackAfterDamageInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int runningAttackBeforeDamageInterruptionChanceByDamage = 0;
    [Range(0, 1000)]
    [SerializeField] int runningAttackAfterDamageInterruptionChanceByDamage = 80;
    [Range(0, 1000)]
    [SerializeField] int runningAttackBeforeDamageInterruptionChanceByDodge = 700;
    [Range(0, 1000)]
    [SerializeField] int runningAttackAfterDamageInterruptionChanceByDodge = 900;

    #endregion

    #region Defense

    private bool _defense;
    public bool defense {
        get { return _defense; }
        set
        {
            _defense = value;
            if (value == false)
            {
                _damage = false;
                _knockDown = false;
                _knockDownStandup = false;
                _dodge = false;
                _die = false;

                if (_attack == false) combat = false;
            }
            else
                combat = true;
        }
    }

    private bool _damage;
    public bool damage
    {
        get { return _damage; }
        set
        {
            _damage = value;
            if (value) defense = true;
            else if (!(_knockDown || _dodge || _die || _knockDownStandup)) defense = false;
        }
    }
    private bool _knockDown;
    public bool knockDown
    {
        get { return _knockDown; }
        set
        {
            _knockDown = value;
            if (value)
            {
                defense = true;
                timeKnockedDown = 0;//we can actually set this to negative knockdown fly animation time
                invokeCall = false;
            }
            else if (!(_damage || _dodge || _die || _knockDownStandup)) defense = false;
        }
    }
    private bool _knockDownStandup;
    public bool knockDownStandup
    {
        get { return _knockDownStandup; }
        set
        {
            _knockDownStandup = value;
            if (value)
            {
                defense = true;
                damage = false;
            }
            else if (!(_knockDown || _dodge || _die || _damage)) defense = false;
        }
    }
    private bool _dodge;
    public bool dodge
    {
        get { return _dodge; }
        set
        {
            _dodge = value;
            if (value) defense = true;
            else if (!(_knockDown || _damage || _die || _knockDownStandup)) defense = false;
        }
    }
    private bool _die;
    public bool die
    {
        get { return _die; }
        set
        {
            _die = value;
            if (value) defense = true;
            else if (!(_knockDown || _dodge || _damage || _knockDownStandup)) defense = false;
        }
    }

    //Defense Configuration Fields
    [Header("Defense Configuration Parameters")]
    [SerializeField] float dodgeCooldownTime = 4f;
    [SerializeField] int dodgeTimes = 3;
    [Range(0,1000)]
    [SerializeField] int knockDownChance = 80;
    [SerializeField] float knockDownTime = 3.7f;
    [SerializeField] float knockDownExtraTime = 0.5f;
    [Range(0, 1)]
    [SerializeField] float noDamageRatio = 0.009f;
    [Range(0, 1)]
    [SerializeField] float smallDamageRatio = 0.09f;
    [Range(0, 1)]
    [SerializeField] float bigDamageRatio = 0.21f;
    [Range(0, 1000)]
    [SerializeField] int leastLikelyDamageChance = 70;
    [Header("Damage Interruption Possibilities")]
    [Range(0, 1000)]
    [SerializeField] int damageBeforeHalfwayInterruptionChanceByMovement = 50;
    [Range(0, 1000)]
    [SerializeField] int damageAfterHalfwayInterruptionChanceByMovement = 90;
    [Range(0, 1000)]
    [SerializeField] int damageBeforeHalfwayInterruptionChanceByAttack = 50;
    [Range(0, 1000)]
    [SerializeField] int damageAfterHalfwayInterruptionChanceByAttack = 90;
    [Range(0, 1000)]
    [SerializeField] int damageBeforeHalfwayInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int damageAfterHalfwayInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int damageBeforeHalfwayInterruptionChanceByDamage = 50;
    [Range(0, 1000)]
    [SerializeField] int damageAfterHalfwayInterruptionChanceByDamage = 90;
    [Range(0, 1000)]
    [SerializeField] int damageBeforeHalfwayInterruptionChanceByDodge = 500;
    [Range(0, 1000)]
    [SerializeField] int damageAfterHalfwayInterruptionChanceByDodge = 700;
    [Header("KnockDown Standup Interruption Possibilities")]
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupBeforeHalfwayInterruptionChanceByMovement = 50;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupAfterHalfwayInterruptionChanceByMovement = 90;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupBeforeHalfwayInterruptionChanceByAttack = 50;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupAfterHalfwayInterruptionChanceByAttack = 90;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupBeforeHalfwayInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupAfterHalfwayInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupBeforeHalfwayInterruptionChanceByDamage = 50;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupAfterHalfwayInterruptionChanceByDamage = 90;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupBeforeHalfwayInterruptionChanceByDodge = 800;
    [Range(0, 1000)]
    [SerializeField] int knockDownStandupAfterHalfwayInterruptionChanceByDodge = 950;
    [Header("Dodge Interruption Possibilities")]
    [Range(0, 1000)]
    [SerializeField] int dodgeBeforeHalfwayInterruptionChanceByMovement = 50;
    [Range(0, 1000)]
    [SerializeField] int dodgeAfterHalfwayInterruptionChanceByMovement = 90;
    [Range(0, 1000)]
    [SerializeField] int dodgeBeforeHalfwayInterruptionChanceByAttack = 50;
    [Range(0, 1000)]
    [SerializeField] int dodgeAfterHalfwayInterruptionChanceByAttack = 90;
    [Range(0, 1000)]
    [SerializeField] int dodgeBeforeHalfwayInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int dodgeAfterHalfwayInterruptionChanceBySkillAttack = 900;
    [Range(0, 1000)]
    [SerializeField] int dodgeBeforeHalfwayInterruptionChanceByDamage = 50;
    [Range(0, 1000)]
    [SerializeField] int dodgeAfterHalfwayInterruptionChanceByDamage = 90;
    [Range(0, 1000)]
    [SerializeField] int dodgeBeforeHalfwayInterruptionChanceByDodge = 800;
    [Range(0, 1000)]
    [SerializeField] int dodgeAfterHalfwayInterruptionChanceByDodge = 950;

    int _remainingDodgeTimes = 3;
    int remainingDodgeTimes
    {
        get { return _remainingDodgeTimes; }
        set
        {
            _remainingDodgeTimes = value;
            if(_remainingDodgeTimes<dodgeTimes && dodgeCoroutine==false)
                StartCoroutine(DodgeCooldown());
        }
    }
    bool dodgeCoroutine = false;
    float _timeKnockedDown = 0;
    float timeKnockedDown
    {
        get { return _timeKnockedDown; }
        set
        {
            _timeKnockedDown = value;
            if(value>=knockDownTime && !invokeCall)
            {
                Invoke("KnockDownExtraStandup", knockDownExtraTime);
                invokeCall = true;
            }
        }
    }
    bool invokeCall = false;
    int number1;
    int number2;
    float angle;
    bool bigDamage;
    bool generalDirection;

    #endregion

    #endregion

    #endregion

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this, 0.01f);
    }

    private void Start()
    {
        foreach (var item in animators)
        {
            if (item.gameObject.tag == "Animator1")
                realAnimator = item;
        }
        eventController = PlayerAnimationEventController.Instance;
        playerMovement = PlayerMovement.Instance;
    }

    public float UpdateMovementInfo(bool movementRequest,bool forward=false)
    {
        timeElapsed = Time.deltaTime;

        if (!runningFast)
            timeFastRun -= timeElapsed;

        if (movementRequest)
        {
            if (idle)
                jogging = true;
            else if (moving)
            {
                if (jogging)
                {
                    timeJogged += timeElapsed;
                    if (timeJogged + Mathf.Epsilon >= joggingTime)
                    {
                        jogging = false;
                        running = true;
                    }
                }
                else if (running && shifting)
                {
                    if (timeFastRun <= 0)
                    {
                        running = false;
                        runningFast = true;
                    }
                }
                else if (runningFast)
                {
                    timeFastRun += timeElapsed;

                    if (timeFastRun >= fastRunLimit || !shifting)
                    {
                        runningFast = false;
                        running = true;
                    }
                }
            }
            else if (attack)
            {
                if (meleeAttack && MeleeAttackInterruptionAvailable(AnimType.Movement))
                    Attack();
                else if (runningAttack && RunningAttackInterruptionAvailable(AnimType.Movement))
                    Attack();                    

                void Attack()
                {
                    eventController.StopSwordSlashes();
                    eventController.currentAttackDamageDone = false;
                    jogging = true;
                }
            }
            else if (defense)
            {
                if (damage && knockDown==false && DamageInterruptionAvailable(AnimType.Movement))
                {
                    eventController.currentDamageHalfway = false;
                    jogging = true;
                }
                else if (knockDown && timeKnockedDown >= knockDownTime)
                {
                    CancelInvoke("KnockDownExtraStandup");
                    knockDown = false;

                    knockDownStandup = true;
                    if (forward)
                        TriggerManager.knockDown_FW_Standup = true;
                    else
                        TriggerManager.knockDown_BW_Standup = true;
                }
                else if (knockDownStandup && KnockDownStandupInteractionAvailable(AnimType.Movement))
                {
                    eventController.currentStandupHalfway = false;
                    jogging = true;
                }
                else if(dodge && DodgeInteractionAvailable(AnimType.Movement))
                {
                    eventController.currentDodgeHalfway = false;
                    jogging = true;
                }
            }
        }
        else
            moving = false;

        if (knockDown)
            timeKnockedDown += timeElapsed;
        else if(idle)
            timeIdled += timeElapsed;

        StartCoroutine(UpdateParameters());

        return speed;
    }//we may autocorrect player's rotation to the enemy attacked here

    public bool UpdateAttackInfo()
    {
        canAttack = false;
        if (idle)
            DetermineAttackAnimation();
        else if (moving)
        {
            realAnimator.SetLayerWeight(weaponLayerIndex, 1);
            var currentAnimation = realAnimator.GetCurrentAnimatorStateInfo(baseLayerIndex);

            if (currentAnimation.IsName("Run_ver_A"))
                currentMovingAnimationName = "Run_ver_A";
            else if (currentAnimation.IsName("Run_To_Fast_ver_A"))
                currentMovingAnimationName = "Run_To_Fast_ver_A";
            else if (currentAnimation.IsName("Run_Fast_ver_A") || currentAnimation.IsName("Run_Fast_To_Idle_ver_D"))
                currentMovingAnimationName = "Run_Fast_ver_A";
            else if (currentAnimation.IsName("Jogging_8Way_verA_F"))
                currentMovingAnimationName = "Jogging_8Way_verA_F";

            realAnimator.SetBool(currentMovingAnimationName, true);
            Invoke("DisableWeaponLayer", currentAnimation.length);

            DetermineRunningAttackAnimation();
        }
        else if(attack)
        {
            if (meleeAttack && MeleeAttackInterruptionAvailable(AnimType.Attack))
            {
                eventController.StopSwordSlashes();
                eventController.currentAttackDamageDone = false;

                if (UnityEngine.Random.Range(1, 1001) > comboChance)
                {
                    newAnimationName = "";

                    void GetMeleeAttackName()
                    {
                        switch (UnityEngine.Random.Range(1, 6))
                        {
                            case 1:
                                newAnimationName = "MeleeAttack1";
                                break;
                            case 2:
                                newAnimationName = "MeleeAttack2";
                                break;
                            case 3:
                                newAnimationName = "MeleeAttack3";
                                break;
                            case 4:
                                newAnimationName = "MeleeAttack4";
                                break;
                            case 5:
                                newAnimationName = "MeleeAttack5";
                                break;
                        }
                    }

                    GetMeleeAttackName();
                    if (realAnimator.GetCurrentAnimatorStateInfo(baseLayerIndex).IsName(newAnimationName))
                        GetMeleeAttackName();

                    canAttack = true;
                    PlayAnimation();
                }
                else
                {
                    meleeAttack = false;
                    DetermineComboAnimation();
                }

                eventController.currentAttackDamageDone = false;
            }
            else if(runningAttack && RunningAttackInterruptionAvailable(AnimType.Attack))
            {
                eventController.StopSwordSlashes();
                eventController.currentAttackDamageDone = false;
                runningAttack = false;
                DetermineAttackAnimation();
            }
        }
        else if(defense)
        {
            if (damage && knockDown==false && DamageInterruptionAvailable(AnimType.Attack))
            {
                eventController.currentDamageHalfway = false;
                CancelDefenseAndAttack();
            }
            else if (knockDownStandup && KnockDownStandupInteractionAvailable(AnimType.Attack))
            {
                eventController.currentStandupHalfway = false;
                CancelDefenseAndAttack();
            }
            else if(dodge && DodgeInteractionAvailable(AnimType.Attack))
            {
                eventController.currentDodgeHalfway = false;
                CancelDefenseAndAttack();
            }

            void CancelDefenseAndAttack()
            {
                defense = false;
                DetermineAttackAnimation();
            }
        }

        return canAttack;

        void DetermineAttackAnimation()
        {
            if (UnityEngine.Random.Range(1, 1001) <= comboChance)
                DetermineComboAnimation();
            else
                DetermineMeleeAttackAnimation();
        }
        void DetermineMeleeAttackAnimation()
        {
            canAttack = true;
            meleeAttack = true;
            switch (UnityEngine.Random.Range(1, 6))
            {
                case 1:
                    TriggerManager.meleeAttack2 = false;
                    break;
                case 2:
                    TriggerManager.meleeAttack2 = true;
                    break;
                case 3:
                    TriggerManager.meleeAttack3 = true;
                    break;
                case 4:
                    TriggerManager.meleeAttack4 = true;
                    break;
                case 5:
                    TriggerManager.meleeAttack5 = true;
                    break;
            }
        }
        void DetermineComboAnimation()
        {
            canAttack = true;
            combo = true;
            switch (UnityEngine.Random.Range(1, 7))
            {
                case 1:
                case 2:
                case 3:
                    TriggerManager.combo2 = false;
                    break;
                case 4:
                case 5:
                    TriggerManager.combo2 = true;
                    break;
                case 6:
                    TriggerManager.combo3 = true;
                    break;
            }
        }
        void DetermineRunningAttackAnimation()
        {
            canAttack = true;
            runningAttack = true;
            switch (UnityEngine.Random.Range(1, 4))
            {
                case 1:
                case 2:
                    TriggerManager.runningAttack2 = false;
                    break;
                case 3:
                    TriggerManager.runningAttack2 = true;
                    break;
            }
        }
    }

    public void UpdateAttackInfo(int skillAnimationNumber)
    {
        if(idle)
        {
            idle = false;
            SetSkillTrigger();
        }
        else if(moving)
        {
            moving = false;
            SetSkillTrigger();
        }
        else if(attack)
        {
            if (meleeAttack && MeleeAttackInterruptionAvailable(AnimType.SkillAttack))
                StopAttackAndCastSkill();
            else if (runningAttack && RunningAttackInterruptionAvailable(AnimType.SkillAttack))
                StopAttackAndCastSkill();

            void StopAttackAndCastSkill()
            {
                attack = false;
                eventController.StopSwordSlashes();
                eventController.currentAttackDamageDone = false;
                SetSkillTrigger();
            }
        }
        else if(defense)
        {
            if(damage && knockDown==false && DamageInterruptionAvailable(AnimType.SkillAttack))
            {
                eventController.currentDamageHalfway = false;
                CancelDefenseAndCastSkill();
            }
            else if(knockDownStandup && KnockDownStandupInteractionAvailable(AnimType.SkillAttack))
            {
                eventController.currentStandupHalfway = false;
                CancelDefenseAndCastSkill();
            }
            else if(dodge && DodgeInteractionAvailable(AnimType.SkillAttack))
            {
                eventController.currentDodgeHalfway = false;
                CancelDefenseAndCastSkill();
            }

            void CancelDefenseAndCastSkill()
            {
                defense = false;
                SetSkillTrigger();
            }
        }

        void SetSkillTrigger()
        {
            skillAttack = true;
            switch (skillAnimationNumber)
            {
                case 1:
                    TriggerManager.skill_A = true;
                    break;
                case 2:
                    TriggerManager.skill_B = true;
                    break;
                case 3:
                    TriggerManager.skill_C = true;
                    break;
                case 4:
                    TriggerManager.skill_D = true;
                    break;
                case 5:
                    TriggerManager.skill_G = true;
                    break;
                case 6:
                    TriggerManager.skill_H = true;
                    break;
                default:
                    Debug.LogError("Invalid animation number detected!!!\nShutting down in 5,4,3,2,1....jk jk go fix it dumbhead :D",
                        FindObjectsOfType<Skill>().Where(s=>s.animationNumber==skillAnimationNumber).First());
                    skillAttack = false;
                    break;
            }
        }
    }

    public void UpdateDefenseInfo(Vector2 enemyPositionWithoutY,Vector2 playerPositionWithoutY ,float damageRatio = 0, bool knockDownRequired = false,bool isDead=false)
    {
        //The damage will be applied to player no matter what animation is played here, or even if no animations are played
        //knockdown might be required due to a special skill enemy casts
        if(isDead)
            die = true;
        else if(knockDownRequired)
        {
            if ((die || skillAttack))
                return;
            
            DetermineKnockDownAnimation();
        }
        else
        {
            if(idle)
                DetermineDefenseAnimation();
            else if(moving)
                DetermineDefenseAnimation();
            else if(attack)
            {
                if (meleeAttack && MeleeAttackInterruptionAvailable(AnimType.Damage))
                    CancelAttackAndTakeDamage();
                else if (runningAttack && RunningAttackInterruptionAvailable(AnimType.Damage))
                    CancelAttackAndTakeDamage();

                void CancelAttackAndTakeDamage()
                {
                    eventController.StopSwordSlashes();
                    eventController.currentAttackDamageDone = false;
                    attack = false;
                    DetermineDefenseAnimation();
                }
            }
            else if(defense)
            {
                if(damage && knockDown==false && DamageInterruptionAvailable(AnimType.Damage))
                {
                    eventController.currentDamageHalfway = false;

                    if (damageRatio <= noDamageRatio)
                        return;

                    angle = Vector2.SignedAngle(enemyPositionWithoutY, playerPositionWithoutY);

                    if (UnityEngine.Random.Range(1,1001)>knockDownChance)
                    {
                        void GetDamageName()
                        {
                            if (angle > -45 && angle < 45)
                            {
                                switch (UnityEngine.Random.Range(1, 4))
                                {
                                    case 1:
                                    case 2:
                                        newAnimationName = "Damage_Right_Small_ver_A";
                                        break;
                                    case 3:
                                        newAnimationName = "Damage_Right_Small_ver_B";
                                        break;
                                }
                            }
                            else if (angle >= 45 && angle <= 135)
                            {
                                bigDamage = false;

                                if (damageRatio <= smallDamageRatio)
                                    bigDamage = (number1 <= leastLikelyDamageChance && damageRatio / smallDamageRatio >= 0.667f);
                                else
                                    bigDamage = !(number1 <= leastLikelyDamageChance && damageRatio / bigDamageRatio <= 0.667f);

                                if (bigDamage)
                                {
                                    switch (UnityEngine.Random.Range(1, 11))
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                            newAnimationName = "Damage_Front_Big_ver_A";
                                            break;
                                        case 4:
                                        case 5:
                                            newAnimationName = "Damage_Front_Big_ver_B";
                                            break;
                                        case 6:
                                            newAnimationName = "Damage_Front_Big_ver_C";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (UnityEngine.Random.Range(1, 11))
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                            newAnimationName = "Damage_Front_Small_ver_A";
                                            break;
                                        case 4:
                                        case 5:
                                            newAnimationName = "Damage_Front_Small_ver_B";
                                            break;
                                        case 6:
                                            newAnimationName = "Damage_Front_Small_ver_C";
                                            break;
                                    }
                                }
                            }
                            else if (angle >= -135 && angle <= -45)
                            {
                                switch (UnityEngine.Random.Range(1, 4))
                                {
                                    case 1:
                                    case 2:
                                        newAnimationName = "Damage_Back_Small_ver_A";
                                        break;
                                    case 3:
                                        newAnimationName = "Damage_Back_Small_ver_B";
                                        break;
                                }
                            }
                            else
                            {
                                switch (UnityEngine.Random.Range(1, 4))
                                {
                                    case 1:
                                    case 2:
                                        newAnimationName = "Damage_Left_Small_ver_A";
                                        break;
                                    case 3:
                                        newAnimationName = "Damage_Left_Small_ver_B";
                                        break;
                                }
                            }
                        }

                        GetDamageName();
                        if (realAnimator.GetCurrentAnimatorStateInfo(baseLayerIndex).IsName(newAnimationName))
                            GetDamageName();

                        PlayAnimation();
                    }
                    else
                        DetermineKnockDownAnimation();

                    eventController.currentDamageHalfway = false;
                    
                }
                else if(knockDown)
                {
                    damage = true;
                }
                else if(knockDownStandup && KnockDownStandupInteractionAvailable(AnimType.Damage))
                {
                    eventController.currentStandupHalfway = false;
                    CancelDefenseAndDefenseAgain();
                }
                else if(dodge && DodgeInteractionAvailable(AnimType.Damage))
                {
                    eventController.currentDodgeHalfway = false;
                    CancelDefenseAndDefenseAgain();
                }

                void CancelDefenseAndDefenseAgain()
                {
                    defense = false;
                    DetermineDefenseAnimation();
                }
            }
        }

        void DetermineDefenseAnimation()
        {
            if (UnityEngine.Random.Range(1, 1001) <= knockDownChance)
                DetermineKnockDownAnimation();
            else
                DetermineDamageAnimation();
        }
        void DetermineDamageAnimation()
        {
            if (damageRatio <= noDamageRatio)
                return;

            damage = true;

            angle = Vector2.SignedAngle(enemyPositionWithoutY, playerPositionWithoutY);//dont change the order

            if (angle > -45 && angle < 45)
                TriggerManager.damageRight = true;
            else if (angle >= 45 && angle <= 135)
                TriggerManager.damageFront = true;
            else if (angle >= -135 && angle <= -45)
                TriggerManager.damageBack = true;
            else
                TriggerManager.damageLeft = true;

            if (TriggerManager.damageFront)
            {
                number1 = UnityEngine.Random.Range(1, 1001);
                bigDamage = false;

                if (damageRatio <= smallDamageRatio)
                    bigDamage = (number1 <= leastLikelyDamageChance && damageRatio / smallDamageRatio >= 0.667f);
                else
                    bigDamage = !(number1 <= leastLikelyDamageChance && damageRatio / bigDamageRatio <= 0.667f);

                if (bigDamage)
                    TriggerManager.bigDamage = true;
                else TriggerManager.smallDamage = true;

                switch (UnityEngine.Random.Range(1, 11))
                {
                    case 1:
                    case 2:
                    case 3:
                        TriggerManager.damageGrade_A = true;
                        break;
                    case 4:
                    case 5:
                        TriggerManager.damageGrade_B = true;
                        break;
                    case 6:
                        TriggerManager.damageGrade_C = true;
                        break;
                }
            }
            else
            {
                TriggerManager.smallDamage = false;

                switch (UnityEngine.Random.Range(1, 4))
                {
                    case 1:
                    case 2:
                        TriggerManager.damageGrade_A = true;
                        break;
                    case 3:
                        TriggerManager.damageGrade_B = true;
                        break;
                }
            }
        }
        void DetermineKnockDownAnimation()
        {
            if (damageRatio <= noDamageRatio)
                return;

            knockDown = true;

            angle = Vector2.SignedAngle(enemyPositionWithoutY, playerPositionWithoutY);//dont change the order

            if (angle >= 0)
                generalDirection = true;
            else
                generalDirection = false;

            if (!generalDirection)
                TriggerManager.knockDownBack = true;

            number2 = UnityEngine.Random.Range(1, 15);

            switch (number2)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    TriggerManager.damageGrade_A = false;
                    break;
                case 6:
                case 7:
                case 8:
                case 9:
                    TriggerManager.damageGrade_B = true;
                    break;
                case 10:
                case 11:
                case 12:
                    TriggerManager.damageGrade_C = true;
                    break;
                case 13:
                case 14:
                    TriggerManager.damageGrade_D = true;
                    break;
            }
        }
    }

    public void UpdateDefenseInfo(InputManager.DodgeType dodgeType)
    {
        if (idle)
            ActivateDodge();
        else if (moving)
            ActivateDodge();
        else if (attack)
        {
            if(meleeAttack && MeleeAttackInterruptionAvailable(AnimType.Dodge))
                CancelAttackAndDodge();
            else if(runningAttack && RunningAttackInterruptionAvailable(AnimType.Dodge))
                CancelAttackAndDodge();

            void CancelAttackAndDodge()
            {
                eventController.StopSwordSlashes();
                eventController.currentAttackDamageDone = false;
                attack = false;
                ActivateDodge();
            }
        }
        else if (defense)
        {
            if(damage && knockDown==false && DamageInterruptionAvailable(AnimType.Dodge))
            {
                eventController.currentDamageHalfway = false;
                CancelDefenseAndDodge();
            }
            else if(knockDownStandup && KnockDownStandupInteractionAvailable(AnimType.Dodge))
            {
                eventController.currentStandupHalfway = false;
                CancelDefenseAndDodge();
            }
            else if(dodge && DodgeInteractionAvailable(AnimType.Dodge))
            {
                eventController.currentDodgeHalfway = false;

                if (remainingDodgeTimes <= 0)
                    return;

                switch (dodgeType)
                {
                    case InputManager.DodgeType.Left:
                        newAnimationName = "Dodge_Left";
                        break;
                    case InputManager.DodgeType.Right:
                        newAnimationName = "Dodge_Right";
                        break;
                    case InputManager.DodgeType.Front:
                        newAnimationName = "Dodge_Front";
                        break;
                    case InputManager.DodgeType.Back:
                        newAnimationName = "Dodge_Back";
                        break;
                }

                PlayAnimation();
            }

            void CancelDefenseAndDodge()
            {
                defense = false;
                ActivateDodge();
            }
        }

        void ActivateDodge()
        {
            if (remainingDodgeTimes <= 0)
                return;
            dodge = true;

            switch (dodgeType)
            {
                case InputManager.DodgeType.Left:
                    TriggerManager.dodgeLeft = true;
                    break;
                case InputManager.DodgeType.Right:
                    TriggerManager.dodgeRight = true;
                    break;
                case InputManager.DodgeType.Front:
                    TriggerManager.dodgeFront = true;
                    break;
                case InputManager.DodgeType.Back:
                    TriggerManager.dodgeFront = false;
                    break;
            }
        }
    }

    public IEnumerator UpdateParameters()
    {
        yield return new WaitForEndOfFrame();
        foreach (var animator in animators)
        {
            animator.SetBool("Idle", idle);
            animator.SetBool("HoldingWeapon", holdingWeapon);
            animator.SetBool("Moving", moving);
            animator.SetBool("Jogging", jogging);
            animator.SetBool("Running", running);
            animator.SetBool("RunningFast", runningFast);

            animator.SetBool("Attack", attack);
            animator.SetBool("MeleeAttack", meleeAttack);
            animator.SetBool("RunningAttack", runningAttack);
            animator.SetBool("Combo", combo);
            animator.SetBool("SkillAttack", skillAttack);

            if (attack)
            {
                if (meleeAttack)
                {
                    if (TriggerManager.meleeAttack2) animator.SetTrigger("MeleeAttack2");
                    else if (TriggerManager.meleeAttack3) animator.SetTrigger("MeleeAttack3");
                    else if (TriggerManager.meleeAttack4) animator.SetTrigger("MeleeAttack4");
                    else if (TriggerManager.meleeAttack5) animator.SetTrigger("MeleeAttack5");
                }
                else if (runningAttack && TriggerManager.runningAttack2)
                    animator.SetTrigger("RunningAttack2");
                else if (combo)
                {
                    if (TriggerManager.combo2) animator.SetTrigger("Combo2");
                    else if (TriggerManager.combo3) animator.SetTrigger("Combo3");
                }
                else if (skillAttack)
                {
                    if (TriggerManager.skill_A) animator.SetTrigger("Skill_A");
                    else if (TriggerManager.skill_B) animator.SetTrigger("Skill_B");
                    else if (TriggerManager.skill_C) animator.SetTrigger("Skill_C");
                    else if (TriggerManager.skill_D) animator.SetTrigger("Skill_D");
                    else if (TriggerManager.skill_G) animator.SetTrigger("Skill_G");
                    else animator.SetTrigger("Skill_H");
                }
            }

            animator.SetBool("Defense", defense);
            animator.SetBool("Damage", damage);
            animator.SetBool("KnockDown", knockDown);
            animator.SetBool("KnockDownStandup", knockDownStandup);
            animator.SetBool("Dodge", dodge);
            animator.SetBool("Die", die);

            if (defense)
            {
                if (damage)
                {
                    if (TriggerManager.damageFront) animator.SetTrigger("DamageFront");
                    else if (TriggerManager.damageBack) animator.SetTrigger("DamageBack");
                    else if (TriggerManager.damageRight) animator.SetTrigger("DamageRight");
                    else animator.SetTrigger("DamageLeft");

                    if (TriggerManager.smallDamage) animator.SetTrigger("SmallDamage");
                    else animator.SetTrigger("BigDamage");

                    if (TriggerManager.damageGrade_A) animator.SetTrigger("Damage_Grade_A");
                    else if (TriggerManager.damageGrade_B) animator.SetTrigger("Damage_Grade_B");
                    else if (TriggerManager.damageGrade_C) animator.SetTrigger("Damage_Grade_C");
                    else animator.SetTrigger("Damage_Grade_D");

                }
                else if (TriggerManager.knockDownBack) animator.SetTrigger("KnockDownBack");
                else if (knockDownStandup)
                {
                    if (TriggerManager.knockDown_BW_Standup) animator.SetTrigger("KnockDown_BW_Standup");
                    else if (TriggerManager.knockDown_FW_Standup) animator.SetTrigger("KnockDown_FW_Standup");
                }
                else if (dodge)
                {
                    if (TriggerManager.dodgeFront) animator.SetTrigger("DodgeFront");
                    else if (TriggerManager.dodgeLeft) animator.SetTrigger("DodgeLeft");
                    else if(TriggerManager.dodgeRight) animator.SetTrigger("DodgeRight");
                }
            }
            animator.SetBool("Combat", combat);
        }
        TriggerManager.ResetTriggers();
    }

    IEnumerator DodgeCooldown()
    {
        dodgeCoroutine = true;
        yield return new WaitForSeconds(dodgeCooldownTime);
        dodgeCoroutine = false;
        remainingDodgeTimes++;
    }

    void KnockDownExtraStandup()
    {
        knockDown = false;
        knockDownStandup = true;
    }

    void DisableWeaponLayer()
    {
        realAnimator.SetLayerWeight(weaponLayerIndex, 0);
        realAnimator.SetBool(currentMovingAnimationName, false);
    }

    void ResetAnimator()
    {
        combat = false;
        moving = false;
        idle = true;
        holdingWeapon = false;
        TriggerManager.ResetTriggers();
        realAnimator.Play("Idle_ver_A", baseLayerIndex);
    }

    void PlayAnimation()
    {
        foreach (var anim in animators)
        {
            anim.Play(newAnimationName, baseLayerIndex);
        }
    }

    bool MeleeAttackInterruptionAvailable(AnimType type)
    {
        bool con = false;

        switch (type)
        {
            case AnimType.Movement:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= meleeAttackBeforeDamageInterruptionChanceByMovement)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= meleeAttackAfterDamageInterruptionChanceByMovement);
                break;
            case AnimType.Attack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= meleeAttackBeforeDamageInterruptionChanceByAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= meleeAttackAfterDamageInterruptionChanceByAttack);
                break;
            case AnimType.SkillAttack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= meleeAttackBeforeDamageInterruptionChanceBySkillAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= meleeAttackAfterDamageInterruptionChanceBySkillAttack);
                break;
            case AnimType.Damage:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= meleeAttackBeforeDamageInterruptionChanceByDamage)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= meleeAttackAfterDamageInterruptionChanceByDamage);
                break;
            case AnimType.Dodge:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= meleeAttackBeforeDamageInterruptionChanceByDodge)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= meleeAttackBeforeDamageInterruptionChanceByDodge);
                break;
        }
        return con;
    }

    bool RunningAttackInterruptionAvailable(AnimType type)
    {
        bool con = false;

        switch (type)
        {
            case AnimType.Movement:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= runningAttackBeforeDamageInterruptionChanceByMovement)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= runningAttackAfterDamageInterruptionChanceByMovement);
                break;
            case AnimType.Attack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= runningAttackBeforeDamageInterruptionChanceByAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= runningAttackAfterDamageInterruptionChanceByAttack);
                break;
            case AnimType.SkillAttack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= runningAttackBeforeDamageInterruptionChanceBySkillAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= runningAttackAfterDamageInterruptionChanceBySkillAttack);
                break;
            case AnimType.Damage:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= runningAttackBeforeDamageInterruptionChanceByDamage)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= runningAttackAfterDamageInterruptionChanceByDamage);
                break;
            case AnimType.Dodge:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= runningAttackBeforeDamageInterruptionChanceByDodge)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= runningAttackBeforeDamageInterruptionChanceByDodge);
                break;
        }
        return con;
    }

    bool DamageInterruptionAvailable(AnimType type)
    {
        bool con = false;

        switch (type)
        {
            case AnimType.Movement:
                con = (eventController.currentDamageHalfway == false && UnityEngine.Random.Range(1, 1001) <= damageBeforeHalfwayInterruptionChanceByMovement)
                || (eventController.currentDamageHalfway && UnityEngine.Random.Range(1, 1001) <= damageAfterHalfwayInterruptionChanceByMovement);
                break;
            case AnimType.Attack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= damageBeforeHalfwayInterruptionChanceByAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= damageAfterHalfwayInterruptionChanceByAttack);
                break;
            case AnimType.SkillAttack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= damageBeforeHalfwayInterruptionChanceBySkillAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= damageAfterHalfwayInterruptionChanceBySkillAttack);
                break;
            case AnimType.Damage:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= damageBeforeHalfwayInterruptionChanceByDamage)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= damageAfterHalfwayInterruptionChanceByDamage);
                break;
            case AnimType.Dodge:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= damageBeforeHalfwayInterruptionChanceByDodge)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= damageAfterHalfwayInterruptionChanceByDodge);
                break;
        }
        return con;
    }

    bool KnockDownStandupInteractionAvailable(AnimType type)
    {
        bool con = false;

        switch (type)
        {
            case AnimType.Movement:
                con = (eventController.currentStandupHalfway == false && UnityEngine.Random.Range(1, 1001) <= knockDownStandupBeforeHalfwayInterruptionChanceByMovement)
                || (eventController.currentStandupHalfway && UnityEngine.Random.Range(1, 1001) <= knockDownStandupAfterHalfwayInterruptionChanceByMovement);
                break;
            case AnimType.Attack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= knockDownStandupBeforeHalfwayInterruptionChanceByAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= knockDownStandupAfterHalfwayInterruptionChanceByAttack);
                break;
            case AnimType.SkillAttack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= knockDownStandupBeforeHalfwayInterruptionChanceBySkillAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= knockDownStandupAfterHalfwayInterruptionChanceBySkillAttack);
                break;
            case AnimType.Damage:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= knockDownStandupBeforeHalfwayInterruptionChanceByDamage)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= knockDownStandupAfterHalfwayInterruptionChanceByDamage);
                break;
            case AnimType.Dodge:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= knockDownStandupBeforeHalfwayInterruptionChanceByDodge)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= knockDownStandupAfterHalfwayInterruptionChanceByDodge);
                break;
        }
        return con;
    }

    bool DodgeInteractionAvailable(AnimType type)
    {
        bool con = false;

        switch (type)
        {
            case AnimType.Movement:
                con = (eventController.currentDodgeHalfway == false && UnityEngine.Random.Range(1, 1001) <= dodgeBeforeHalfwayInterruptionChanceByMovement)
                || (eventController.currentDodgeHalfway && UnityEngine.Random.Range(1, 1001) <= dodgeAfterHalfwayInterruptionChanceByMovement);
                break;
            case AnimType.Attack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= dodgeBeforeHalfwayInterruptionChanceByAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= dodgeAfterHalfwayInterruptionChanceByAttack);
                break;
            case AnimType.SkillAttack:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= dodgeBeforeHalfwayInterruptionChanceBySkillAttack)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= dodgeAfterHalfwayInterruptionChanceBySkillAttack);
                break;
            case AnimType.Damage:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= dodgeBeforeHalfwayInterruptionChanceByDamage)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= dodgeAfterHalfwayInterruptionChanceByDamage);
                break;
            case AnimType.Dodge:
                con = (eventController.currentAttackDamageDone == false && UnityEngine.Random.Range(1, 1001) <= dodgeBeforeHalfwayInterruptionChanceByDodge)
                || (eventController.currentAttackDamageDone && UnityEngine.Random.Range(1, 1001) <= dodgeAfterHalfwayInterruptionChanceByDodge);
                break;
        }
        return con;
    }

    enum AnimType
    {
        Movement,
        Attack,
        SkillAttack,
        Damage,
        Dodge
    }
}