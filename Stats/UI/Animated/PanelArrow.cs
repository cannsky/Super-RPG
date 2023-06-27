using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[Obsolete(message:"This feature is no longer used",error:true)]
class PanelArrow : UIAnimator
{
    [SerializeField] FlagButtonSensor panelSensor;
    [SerializeField] PanelStick panelStick;
    [SerializeField] float movePointX;
    [SerializeField] float finalScale = 1.5f;
    [NonSerialized] public RectTransform rect;
    [NonSerialized] public bool activatable=true;
    bool first = true;

    float originalXPos;
    float originalScale;

    protected override void Start()
    {
        base.Start();
        rect = GetComponent<RectTransform>();
        originalXPos = rect.localPosition.x;
        originalScale = rect.localScale.x;
    }

    private void OnEnable()
    {
        Animate();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        rect.localPosition = new Vector3(originalXPos, rect.localPosition.y, rect.localPosition.z);
        first = true;
    }
    public void OnButtonClick()
    {
        panelStick.Animate();
        Close();
    }

    protected override
        (Action pre, Action animation, Action post,
        float waitBeforePre, float waitBeforeAnimation, float waitBeforePost, float waitAfterPost)
        ConstructActions()
    {
        Action pre;
        Action animation;
        Action post;
        Vector3 deltaMoveVector = Vector3.zero;
        Vector3 deltaScaleVector = Vector3.zero;

        if (first)
        {
            pre = () =>
            {
                float deltaMove = (movePointX - rect.localPosition.x) / totalFrames;
                float deltaScale = (finalScale - transform.localScale.x) / totalFrames;

                deltaMoveVector.x = deltaMove;
                deltaScaleVector = new Vector3(deltaScale, deltaScale, deltaScale);
            };

            animation = () =>
            {
                transform.position += deltaMoveVector;
                transform.localScale += deltaScaleVector;
            };

            post = () =>
            {
                first = false;
                Animate();
            };

            return (pre, animation, post, 0, 0, 0.1f, 0);
        }
        else
        {
            pre = () =>
            {
                float deltaMove = (originalXPos - rect.localPosition.x) / totalFrames;
                float deltaScale = (originalScale - transform.localScale.x) / totalFrames;

                deltaMoveVector.x = deltaMove;
                deltaScaleVector = new Vector3(deltaScale, deltaScale, deltaScale);
            };

            animation = () =>
            {
                transform.position += deltaMoveVector;
                transform.localScale += deltaScaleVector;
            };

            post = null;

            return (pre, animation, post, 0, 0, 0, 0);
        }
    }

    public override void StopAnimations()
    {
        StopAllCoroutines();
        transform.localPosition = new Vector3(originalXPos, transform.localPosition.y, transform.localPosition.z);
        transform.localScale = new Vector3(originalScale, originalScale, originalScale);
    }
}
