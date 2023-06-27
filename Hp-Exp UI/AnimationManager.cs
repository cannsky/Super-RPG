using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class AnimationManager : MonoBehaviour
{
    //Config Params
    [SerializeField] string levelUpString = "LevelUp";
    [SerializeField] string oneThirdCompleteString = "OneThirdComplete";
    [SerializeField] string oneThirdAnimationCountString = "OneThirdAnimationCount";

    //Cached Fields
    [System.NonSerialized] public Animator animator;
    private int count = 0;
    public static bool isStarted = false;

    public void Start()
    {
        if (isStarted)
            return;
        animator = GetComponent<Animator>();
        isStarted = true;
    }

    private void GetCurrentExpValues()
    {
        BarUIManager.Instance.Load();
    }  

    public void PlayOneThirdComplete(int times)
    {
        if (times == 0)
            return;

        count = times+1;
        DecreaseCount();
    }

    public void PlayLevelUp()
    {
        animator.SetTrigger(levelUpString);
    }

    private void DecreaseCount()
    {
        count--;
        animator.SetInteger(oneThirdAnimationCountString, count);
        if (count > 0)
            animator.SetTrigger(oneThirdCompleteString);
    }
}
