using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GolemAI : EnemyAI
{
    private class GolemAnimatorState
    {
        public Animator animator;
        public bool isWalking;
        public bool isWalkingBackwards;
        public bool isAttacking;
        public bool isRightFootAttack;
        public bool isLeftFootAttack;

        public GolemAnimatorState(Animator animator)
        {
            this.animator = animator;
        }

        public void SetEnemyAnimatorState(bool isWalking = false,
            bool isWalkingBackwards = false,
            bool isAttacking = false,
            bool isAttackingCloseDistance1 = false,
            bool isAttackingCloseDistance2 = false)
        {
            this.isWalking = isWalking;
            this.isWalkingBackwards = isWalkingBackwards;
            this.isAttacking = isAttacking;
            this.isRightFootAttack = isAttackingCloseDistance1;
            this.isLeftFootAttack = isAttackingCloseDistance2;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetBool("Walk", isWalking);
            animator.SetBool("Walk Backwards", isWalkingBackwards);
            animator.SetBool("Attack", isAttacking);
            animator.SetBool("RightFootAttack", isRightFootAttack);
            animator.SetBool("LeftFootAttack", isLeftFootAttack);
        }
    }

    private GolemAnimatorState animatorState;
    [SerializeField] float feetAttackRange = 3f;
    [SerializeField] float hammerAttackRange = 10f;
    [SerializeField] float maxDistance;

    protected override void Start()
    {
        base.Start();
        this.animatorState = new GolemAnimatorState(animator);
    }
    protected override void Patrolling()
    {
        agent.isStopped = false;
        animatorState.SetEnemyAnimatorState(isWalking: true);
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        distanceBetweenLastPoint = distanceToWalkPoint;
        distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f || distanceToWalkPoint.magnitude == distanceBetweenLastPoint.magnitude) walkPointSet = false;
    }

    protected override void ChasePlayer()
    {
        agent.isStopped = false;
        animatorState.SetEnemyAnimatorState(isWalking: true);
        agent.SetDestination(player.position);
    }

    protected override void AttackPlayer()
    {
        #region Set Variables
        void SetVariables()
        {
            enemyState.alreadyAttacked = true;
            enemyState.isAttacking = true;
            enemyState.isRotated = false;
            enemyState.isRotating = true;
        }
        #endregion
        #region Find Look Position
        Quaternion FindLookPosition()
        {
            Quaternion originalrotation = transform.rotation;
            transform.LookAt(player);
            Quaternion newrotation = transform.rotation;
            transform.rotation = originalrotation;
            return newrotation;
        }
        #endregion

        agent.isStopped = true;
        float distance = Helper.CalculateDistance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.position.x, player.position.z));
        
        if (!enemyState.alreadyAttacked && distance < 3f && enemyState.isRotated)
        {
            if (Random.Range(0, 2) == 1)
                animatorState.SetEnemyAnimatorState(isAttackingCloseDistance1: true);
            else
                animatorState.SetEnemyAnimatorState(isAttackingCloseDistance2: true);
            SetVariables();
        }
        else if (!enemyState.alreadyAttacked && enemyState.isRotated)
        {
            animatorState.SetEnemyAnimatorState(isAttacking: true);
            SetVariables();
        }
        else if(!enemyState.isRotated)
        {
            Quaternion newRotation = FindLookPosition();
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 0.8f * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime);
            animatorState.SetEnemyAnimatorState(isWalkingBackwards: true);
            if ((newRotation.eulerAngles - transform.rotation.eulerAngles).magnitude < 10f)
            {
                enemyState.isRotated = true;
                animatorState.SetEnemyAnimatorState(isWalkingBackwards: false);
            }
        }
    }
    public override void ResetAnimation()
    {
        animatorState.SetEnemyAnimatorState();
    }
}