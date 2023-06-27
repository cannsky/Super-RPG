using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSwitcher : MonoBehaviour
{
    BarUIManager manager;
    AnimationManager animManager;
    private void Start()
    {
        manager = FindObjectOfType<BarUIManager>();
        animManager = FindObjectOfType<AnimationManager>();
    }

    private void Switch()
    {
        //if (animManager.animator.IsInTransition(0) || (animManager.animator.GetCurrentAnimatorStateInfo(0).length > animManager.animator.GetCurrentAnimatorStateInfo(0).normalizedTime))
        //    return;
        manager.ChangeLayout();
    }
}
