using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkeletonAI : EnemyAI
{
    private class SkeletonAnimatorState
    {
        public Animator animator;
        public bool isWalking;
        public bool isAttacking1;

        public SkeletonAnimatorState(Animator animator)
        {
            this.animator = animator;
        }

        public bool SetEnemyAnimatorState(bool isWalking = false,
            bool isAttacking1 = false)
        {
            this.isWalking = isWalking;
            this.isAttacking1 = isAttacking1;
            UpdateAnimator();
            return true;
        }

        private void UpdateAnimator()
        {
            animator.SetBool("Walk", isWalking);
            animator.SetBool("Attack1", isAttacking1);
        }
    }

    private SkeletonAnimatorState animatorState;

    protected override void Start()
    {
        base.Start();
        this.animatorState = new SkeletonAnimatorState(animator);
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
        bool setAnimator(int randomAttackNumber)
        {
            return randomAttackNumber switch
            {
                0 => animatorState.SetEnemyAnimatorState(isAttacking1: true),
                _ => true
            };
        }

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
        if (!enemyState.isRotated)
        {
            Quaternion newRotation = FindLookPosition();
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 1.2f * Time.deltaTime);
            animatorState.SetEnemyAnimatorState(isWalking: true);
            if ((newRotation.eulerAngles - transform.rotation.eulerAngles).magnitude < 5f)
            {
                enemyState.isRotated = true;
                animatorState.SetEnemyAnimatorState(isWalking: false);
            }
        }
        else
        {
            enemyState.isRotated = false;
            enemyState.isAttacking = true;
            setAnimator(Random.Range(0, 1));
        }

    }

    public override void ResetAnimation()
    {
        animatorState.SetEnemyAnimatorState();
    }
}
