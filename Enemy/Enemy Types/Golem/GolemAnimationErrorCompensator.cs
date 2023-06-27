using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimationErrorCompensator : EnemyAnimationErrorCompensator
{
    GolemAI golem;

    protected override void Start()
    {
        base.Start();
        golem = transform.parent.GetComponent<GolemAI>();
    }
    protected override bool GetRootMotionAvailable()
    {
        return base.GetRootMotionAvailable() && golem.enemyState.alreadyAttacked;
    }
}
