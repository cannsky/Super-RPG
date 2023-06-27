using System;
using UnityEngine;

public class AnimationErrorCompensator : MonoBehaviour
{
    protected Animator anim;
    protected Action action;
    protected Action idleAction;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            idleAction?.Invoke();
        else
            action?.Invoke();
    }
}
