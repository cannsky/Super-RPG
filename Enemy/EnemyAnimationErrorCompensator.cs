using UnityEngine;
using System;

public class EnemyAnimationErrorCompensator : AnimationErrorCompensator
{
    protected override void Start()
    {
        base.Start();
        action = () =>
        {
            if(GetRootMotionAvailable())
            {
                anim.applyRootMotion = true;
                transform.parent.position += new Vector3(anim.deltaPosition.x, 0, anim.deltaPosition.z);
            }
            else
            {
                anim.applyRootMotion = false;
            }
        };
    }

    protected virtual bool GetRootMotionAvailable()
    {
        return !anim.GetCurrentAnimatorStateInfo(0).IsTag("No Root Motion");
    }
}