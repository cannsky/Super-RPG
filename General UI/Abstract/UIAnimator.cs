using System.Collections;
using UnityEngine;
using System;
public abstract class UIAnimator : UIToggle
{
    [SerializeField] protected uint totalFrames;
    [SerializeField] protected uint framePerSecond;
    WaitForSeconds beforePre;
    WaitForSeconds beforeAnim;
    WaitForSeconds beforePost;
    WaitForSeconds afterPost;
    Action pre;
    new Action animation;
    Action post;
    protected virtual void Start()
    {
        var actions = ConstructActions();
        pre = actions.pre;
        animation = actions.animation;
        post = actions.post;
        beforePre = new WaitForSeconds(actions.waitBeforePre);
        beforeAnim = new WaitForSeconds(actions.waitBeforeAnimation);
        beforePost = new WaitForSeconds(actions.waitBeforePost);
        afterPost = new WaitForSeconds(actions.waitAfterPost);
    }

    protected abstract 
        (Action pre,Action animation,Action post,
        float waitBeforePre,float waitBeforeAnimation,float waitBeforePost,float waitAfterPost) 
        ConstructActions();
    public void Animate()
    {
        StartCoroutine(AnimateUI(pre,animation,post,beforePre,beforeAnim,beforePost,afterPost));
    }
    private IEnumerator AnimateUI
        (Action pre, Action animation, Action post,
        WaitForSeconds waitBeforePre,WaitForSeconds waitBeforeAnimation,
        WaitForSeconds waitBeforePost,WaitForSeconds waitAfterPost)
    {
        if (framePerSecond == 0 || totalFrames == 0)
            yield return null;
        else
        {
            float time = (float)(totalFrames) / (float)(framePerSecond);
            float deltaTime = 1f / (float)framePerSecond;

            yield return waitBeforePre;

            pre?.Invoke();

            yield return waitBeforeAnimation;

            WaitForSeconds delta = new WaitForSeconds(deltaTime);

            for (int i = 0; i < totalFrames; i++)
            {
                animation?.Invoke();
                yield return delta;
            }

            yield return waitBeforePost;

            post?.Invoke();

            yield return waitAfterPost;
        }
    }

    public virtual void StopAnimations()
    {
        StopAllCoroutines();
    }
}
