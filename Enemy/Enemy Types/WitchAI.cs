using System;
using System.Collections;
using UnityEngine;

public delegate bool Predicate();
public class WitchAI : EnemyAI
{
    private class WitchAnimatorState
    {
        public Animator animator;
        public bool isWalking;
        public bool isMeleeAttack;
        public bool isRangedAttack;
        public bool isSkill;
        public bool isTakingDamage;
        public bool isDying;
        public WitchAnimatorState(Animator animator)
        {
            this.animator = animator;
        }

        public void SetEnemyAnimatorState(bool isWalking = false,
            bool isAttacking = false,
            bool isRangedAttack = false,
            bool isSkill = false,
            bool isTakingDamage = false,
            bool isDying = false)
        {
            this.isWalking = isWalking;
            this.isMeleeAttack = isAttacking;
            this.isRangedAttack = isRangedAttack;
            this.isSkill = isSkill;
            this.isTakingDamage = isTakingDamage;
            this.isDying = isDying;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetBool("Walk", isWalking);
            animator.SetBool("RangedAttack", isRangedAttack);
            animator.SetBool("Attack", isMeleeAttack);
            animator.SetBool("Buff", isSkill);
            animator.SetBool("TakeDamage", isTakingDamage);
            animator.SetBool("Death", isDying);
        }
    }

    private WitchAnimatorState animatorState;

    [SerializeField] float height;
    [SerializeField] float rangedAttackRange = 10f;
    bool playerInRangedAttackRange;

    #region Timers
    [SerializeField] float teleportCoolDown = 18f;
    float teleportTimer;
    public float TeleportTimer
    {
        get => teleportTimer;
        set
        {
            teleportTimer = value;
            if (teleportTimer >= teleportCoolDown)
                Teleport();
        }
    }

    [SerializeField] float minimumRangedAttackTime = 8f;
    float rangedAttackTimer;
    bool isRangedAttack;

    [SerializeField] float skillCoolDown = 16f;
    float skillTimer;
    public float SkillTimer
    {
        get => skillTimer;
        set
        {
            skillTimer = value;
        }
    }
    #endregion

    float deltaTime;

    [SerializeField] Skill skill;

    public bool IsAttacking
    {
        get => enemyState.isAttacking;
        set
        {
            enemyState.isAttacking = value;
            if (!value)
            {
                isRangedAttack = value;
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        animatorState = new WitchAnimatorState(animator);
    }

    protected override void Update()
    {
        deltaTime = Time.deltaTime;
        CheckRanges();
        UpdateChecks();
    }
    protected override void CheckRanges()
    {
        bool value;
        if (value = Physics.CheckSphere(transform.position, sightRange, playerLayer) || true)
        {
            if (value != playerInSightRange)
            {
                TeleportTimer = value ? teleportCoolDown : 0;
                SkillTimer = skillCoolDown - 3f;
            }
            playerInSightRange = value;
        }
        SkillTimer += deltaTime;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        playerInRangedAttackRange = Physics.CheckSphere(transform.position, rangedAttackRange, playerLayer);
        Debug.Log("attack : " + playerInAttackRange);
    }

    protected override void UpdateChecks()
    {
        if (playerInSightRange)
            TeleportTimer += deltaTime;


        if (IsAttacking)
        {
            if (isRangedAttack)
                rangedAttackTimer += deltaTime;

            return;
        }

        Debug.Log("passed");

        if (playerInRangedAttackRange) AttackPlayer();
        else if (playerInSightRange && !playerInRangedAttackRange) ChasePlayer();
        else Patrolling();
    }
    protected override void AttackPlayer()
    {
        void RangedAttack()
        {
            animatorState.SetEnemyAnimatorState(isRangedAttack: true);
            agent.isStopped = true;
            IsAttacking = true;
            isRangedAttack = true;
        }
        void Attack()
        {
            animatorState.SetEnemyAnimatorState(isAttacking: true);
            agent.isStopped = true;
            IsAttacking = true;
        }
        void Skill()
        {
            animatorState.SetEnemyAnimatorState(isSkill: true);
            SkillTimer = 0;
            IsAttacking = true;
            agent.isStopped = true;
        }

        if (enemyState.alreadyAttacked) return;
        enemyState.alreadyAttacked = true;
        if (SkillTimer>=skillCoolDown)
            attack = RotateTowardsPlayer(Skill);
        else if (rangedAttackTimer <= minimumRangedAttackTime || (playerInRangedAttackRange && !playerInAttackRange))
            attack = RotateTowardsPlayer(RangedAttack);
        else if (playerInAttackRange)
            attack = RotateTowardsPlayer(Attack);

        StartCoroutine(attack);
    }

    protected override IEnumerator RotateTowardsPlayer(Action action)
    {
        if (!playerInRangedAttackRange)
        {
            enemyState.isRotating = false;
        }
        else
        {
            enemyState.isRotating = true;
            Quaternion originalRotation;
            Quaternion newRotation;

            void DetermineRotations()
            {
                originalRotation = transform.rotation;
                transform.LookAt(player);
                newRotation = transform.rotation;
                transform.rotation = originalRotation;
            }

            DetermineRotations();

            while ((transform.rotation.eulerAngles - newRotation.eulerAngles).magnitude > 5f)
            {
                DetermineRotations();
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotateSpeedMultipler);
                yield return null;
            }

            enemyState.isRotating = false;
            action?.Invoke();
        }
    }
    protected override void ChasePlayer()
    {
        agent.isStopped = false;
        animatorState.SetEnemyAnimatorState(isWalking: true);
        agent.SetDestination(player.position);
    }

    protected override void Patrolling()
    {
        animatorState.SetEnemyAnimatorState(isWalking: true);
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        distanceBetweenLastPoint = distanceToWalkPoint;
        distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f || distanceToWalkPoint.magnitude == distanceBetweenLastPoint.magnitude) walkPointSet = false;
    }

    private void Teleport()
    {
        bool available = false;
        RaycastHit hit;
        int counter = 1;
        do
        {
            float random = UnityEngine.Random.Range(0, Mathf.PI * 2);
            Vector2 polarCoordinates = new Vector2(random, rangedAttackRange);
            Vector2 cartesian = Helper.ConvertFromPolarToCartesian(polarCoordinates);

            Vector3 rayCastPoint = new Vector3(cartesian.x + player.position.x, transform.position.y+height, cartesian.y + player.position.z);
            Ray ray = new Ray(rayCastPoint, Vector3.down);

            //DebugDraw.DrawVector(rayCastPoint, Vector3.down, 10f, 1f, Color.green, 15f);
            available = Physics.Raycast(ray, out hit, 500f, LayerMask.GetMask("Terrain"));
            counter++;
        } while (counter<10000);

        //hit.point = new Vector3(hit.point.x, hit.point.y + height / 2f, hit.point.z);
        //DebugDraw.DrawVector(hit.point, Vector3.down, 5f, 2f, Color.red, 10f);
        //DebugDraw.DrawVector(hit.point, Vector3.up, 5f, 2f, Color.red, 10f);
        //transform.position = hit.point;
        //transform.LookAt(player);
        TeleportTimer = 0;
        //rangedAttackTimer = 0;
    }
}
