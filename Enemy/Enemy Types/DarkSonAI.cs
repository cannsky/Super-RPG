using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSonAI : EnemyAI
{

    private class DarkSonAnimatorState
    {
        public Animator animator;
        public bool isWalking;
        public bool isAttacking1;
        public bool isAttacking2;
        public bool isAttacking3;
        public bool isAttacking4;

        public DarkSonAnimatorState(Animator animator)
        {
            this.animator = animator;
        }

        public bool SetEnemyAnimatorState(bool isWalking = false,
            bool isAttacking1 = false,
            bool isAttacking2 = false,
            bool isAttacking3 = false,
            bool isAttacking4 = false)
        {
            this.isWalking = isWalking;
            this.isAttacking1 = isAttacking1;
            this.isAttacking2 = isAttacking2;
            this.isAttacking3 = isAttacking3;
            this.isAttacking4 = isAttacking4;
            UpdateAnimator();
            return true;
        }

        private void UpdateAnimator()
        {
            animator.SetBool("Walk", isWalking);
            animator.SetBool("Attack1", isAttacking1);
            animator.SetBool("Attack2", isAttacking2);
            animator.SetBool("Attack3", isAttacking3);
            animator.SetBool("Attack4", isAttacking4);
        }
    }

    private DarkSonAnimatorState animatorState;

    protected override void Start()
    {
        base.Start();
        this.animatorState = new DarkSonAnimatorState(animator);
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
                1 => animatorState.SetEnemyAnimatorState(isAttacking2: true),
                2 => animatorState.SetEnemyAnimatorState(isAttacking3: true),
                3 => animatorState.SetEnemyAnimatorState(isAttacking4: true),
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
        if (!enemyState.isRotated && distance < 3f)
        {
            Quaternion newRotation = FindLookPosition();
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 1.2f * Time.deltaTime);
            animatorState.SetEnemyAnimatorState(isWalking: true);
            if ((newRotation.eulerAngles - transform.rotation.eulerAngles).magnitude < 10f)
            {
                enemyState.isRotated = true;
                animatorState.SetEnemyAnimatorState(isWalking: false);
            }
        }
        else
        {
            enemyState.isRotated = false;
            enemyState.isAttacking = true;
            setAnimator(Random.Range(0, 4));
        }

    }

    public override void ResetAnimation()
    {
        animatorState.SetEnemyAnimatorState();
    }


}
