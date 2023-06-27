using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlagButton : UIAnimator,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] PanelStick stick;
    [NonSerialized] public RectTransform rect;
    [Range(0, 360)] [SerializeField] float rotation;
    [SerializeField] float finalScale;
    Button button;
    Vector3 originalScale;
    Quaternion originalRotation;
    protected override void Start()
    {
        base.Start();
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(stick.Animate);
        originalRotation = rect.localRotation;
        originalScale = rect.localScale;
    }

    protected override
        (Action pre, Action animation, Action post,
        float waitBeforePre, float waitBeforeAnimation,
        float waitBeforePost, float waitAfterPost)
        ConstructActions()
    {
        Vector3 deltaRotationVector = Vector3.zero;
        Vector3 deltaScaleVector=Vector3.zero;
        int frameCount=0;
        bool first = true;
        Action pre = () =>
        {
            float deltaRotation = rotation / totalFrames;
            float deltaScale = 2*(finalScale - transform.localScale.x) / totalFrames;

            deltaRotationVector = new Vector3(0, 0, deltaRotation);
            deltaScaleVector = new Vector3(deltaScale, deltaScale, deltaScale);
            first = true;
            frameCount = 0;
        };

        Action animation = () =>
        {
            Vector3 newRotation = rect.localRotation.eulerAngles + deltaRotationVector;
            rect.localRotation = Quaternion.Euler(newRotation);

            if (first)
                rect.localScale += deltaScaleVector;
            else
                rect.localScale -= deltaScaleVector;

            frameCount++;
            if (frameCount == totalFrames / 2)
                first = false;
        };

        Action post = null;

        return (pre, animation, post, 0.05f, 0, 0, 0);
    }

    public override void StopAnimations()
    {
        StopAllCoroutines();
        rect.localScale = originalScale;
        rect.localRotation = originalRotation;
    }

    public void ButtonClick()
    {
        stick.Animate();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Animate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAnimations();
    }

    public void SetInteractable()
    {
        button.interactable = true;
    }

    public void SetUninteractable()
    {
        button.interactable = false;
    }
}
