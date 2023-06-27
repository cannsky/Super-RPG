using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(EnemyStats), typeof(ExperienceGiver))]
public class EnemyAI : MonoBehaviour
{
    public class EnemyState
    {
        public bool isRotating;
        public bool isAttacking;
        public bool isRotated;
        public bool alreadyAttacked;
    }

    public EnemyState enemyState;
 
    #region Fields
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] public Animator animator;
    [SerializeField] protected float walkPointRange;
    [SerializeField] protected float timeBetweenAttacks;
    [SerializeField] protected float timeBetweenFastAttacks;
    [SerializeField] protected float sightRange, attackRange;
    [SerializeField] protected float speedMultiplier = 1f;
    [Range(0,1)][SerializeField] protected float rotateSpeedMultipler;
    [SerializeField] protected LayerMask playerLayer;

    public float attackResetTime = 5f;

    protected Vector3 walkPoint;
    protected bool walkPointSet;
    protected bool playerInSightRange, playerInAttackRange;
    protected float playerDistance;
    protected Vector3 distanceBetweenLastPoint;
    protected Vector3 distanceToWalkPoint;
    protected Transform player;
    protected EnemyStats stats;
    protected Player playerStats;
    protected PlayerStateController stateController;
    protected IEnumerator attack;

    public virtual float SpeedMultiplier
    {
        get => speedMultiplier;
        set
        {
            speedMultiplier = value;
            agent.speed *= value;
        }
    }
    
    #endregion

    protected virtual void Start()
    {
        enemyState = new EnemyState();
        playerStats = Player.Instance;
        player = playerStats.transform;
        stats = GetComponent<EnemyStats>();
        stateController = PlayerStateController.Instance;
    }

    protected virtual void Update()
    {
        CheckRanges();
        UpdateChecks();
    }

    protected virtual void UpdateChecks()
    {
        if (enemyState.isAttacking) return;
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
        else if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        else Patrolling();
    }
    protected virtual void CheckRanges()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        playerDistance = Helper.CalculateDistance(transform.position, player.position);
    }
    protected virtual void Patrolling()
    {
        throw new NotImplementedException();
    }

    protected virtual void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPointSet = true;
    }

    protected virtual void ChasePlayer()
    {
        animator.SetBool("Run", true);
        agent.SetDestination(player.position);
        transform.rotation = Quaternion.LookRotation(transform.position - player.position);
    }

    protected virtual void AttackPlayer()
    {
        throw new NotImplementedException();
    }

    protected virtual IEnumerator RotateTowardsPlayer(Action action)
    {
        if(!playerInAttackRange)
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

    protected float CalculateFlightDistanceToPlayer()
    {
        return Helper.CalculateDistance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));
    }

    protected float CalculateDistanceToPlayer()
    {
        return Helper.CalculateDistance(transform.position, player.position);
    }

    public virtual void ResetAnimation()
    {
        //nothing here!
    }
}
