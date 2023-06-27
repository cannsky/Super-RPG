using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OthersButton : UIAnimator
{
    [SerializeField] float rotationAngle = 45f;
    [SerializeField] UIToggle othersPanel;
    Button button;

    protected override void Start()
    {
        base.Start();
        button = GetComponent<Button>();
        othersPanel.Toggle();
    }

    protected override 
        (Action pre, Action animation, Action post, 
        float waitBeforePre, float waitBeforeAnimation, float waitBeforePost, float waitAfterPost) 
        ConstructActions()
    {
        Action pre = () => { button.interactable = false; };
        Action animation = () => { gameObject.transform.Rotate(0, 0, (rotationAngle / totalFrames)); };
        Action post = () => {
            othersPanel.Toggle();
            button.interactable = true;
        };

        return (pre, animation, post,0,0,0,0);
    }

    public override void StopAnimations()
    {
        //Nothing
    }
}
