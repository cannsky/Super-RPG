using System;
using UnityEngine.UI;
using UnityEngine;

public class PanelStick : UIAnimator
{
    [Range(0, 1)] [SerializeField] float startFill;
    [Range(0, 1)] [SerializeField] float endFill;
    [SerializeField] UIToggle panel;
    [SerializeField] FlagButton flagButton;
    Image image;
    [NonSerialized] public bool on = false;
    protected override void Start()
    {
        base.Start();
        image = GetComponent<Image>();
    }
    
    protected override 
        (Action pre, Action animation, Action post,
        float waitBeforePre, float waitBeforeAnimation, float waitBeforePost, float waitAfterPost) 
        ConstructActions()
    {
        Action pre = () =>
        {
            if (on) panel.Close();
            flagButton.StopAnimations();
            flagButton.SetUninteractable();
        };
        Action animation = () =>
        {
            if (!on) image.fillAmount += (Mathf.Abs(endFill - startFill)) / totalFrames;
            else image.fillAmount -= (Mathf.Abs(endFill - startFill) / totalFrames);
        };
        Action post = () =>
        {
            if (!on) panel.Open();
            flagButton.SetInteractable();
            on = !on;
        };

        return (pre, animation, post,0,0,0,0);
    }

    public override void StopAnimations()
    {
        //Nothing
    }
}
