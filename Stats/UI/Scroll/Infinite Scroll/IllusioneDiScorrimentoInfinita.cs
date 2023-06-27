using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class IllusioneDiScorrimentoInfinita : UIAnimator
{
    [SerializeField] List<LanguageSupportedText> actualFreakingTexts;//These won't be visible or interactable
    [SerializeField] TMP_Text textBelow;
    [SerializeField] TMP_Text textAbove;
    [SerializeField] TMP_Text textMiddle;
    DoubleLinkedListNode<LanguageSupportedText> current;
    RectTransform rectAbove;
    RectTransform rectBelow;
    RectTransform rectMiddle;
    Vector3 difAbove;
    Vector3 difBelow;
    Vector3 unitMoveAbove;
    Vector3 unitMoveBelow;
    bool up;
    protected override void Start()
    {
        base.Start();
        actualFreakingTexts.ForEach(afet => afet.color = new Color(1f, 1f, 1f, 0));

        var firstNode = new DoubleLinkedListNode<LanguageSupportedText>();
        var lastNode = firstNode;
        
        for (int i = 0; i < actualFreakingTexts.Count-1; i++)
        {
            lastNode.data = actualFreakingTexts[i];
            lastNode.nextNode = new DoubleLinkedListNode<LanguageSupportedText>();
            lastNode.nextNode.previousNode = lastNode;
            lastNode = lastNode.nextNode;
        }

        lastNode.data = actualFreakingTexts[actualFreakingTexts.Count - 1];
        lastNode.nextNode = firstNode;
        firstNode.previousNode = lastNode;
        current = firstNode;

        textMiddle.text = current.data.text;

        rectBelow = textBelow.GetComponent<RectTransform>();
        rectMiddle = textMiddle.GetComponent<RectTransform>();
        rectAbove = textAbove.GetComponent<RectTransform>();
        difAbove = rectAbove.position - rectMiddle.position;
        difBelow = rectMiddle.position - rectBelow.position;
    }

    protected override (Action pre, Action animation, Action post, float waitBeforePre, float waitBeforeAnimation, float waitBeforePost, float waitAfterPost) ConstructActions()
    {
        Action pre = () =>
        {
            rectMiddle.gameObject.SetActive(false);

            if (up)
            {
                rectAbove.position = rectMiddle.position;
                textAbove.text = textMiddle.text;
                current = current.nextNode;
                textBelow.text = current.data.text;
                
                unitMoveAbove = difAbove/totalFrames;
                unitMoveBelow = difBelow / totalFrames;
            }
            else
            {
                rectBelow.position = rectMiddle.position;
                textBelow.text = textMiddle.text;
                current = current.previousNode;
                textAbove.text = current.data.text;

                unitMoveAbove = -difAbove / totalFrames;
                unitMoveBelow = -difBelow / totalFrames;
            }
        };

        Action animation = () =>
        {
            rectBelow.position += unitMoveBelow;
            rectAbove.position += unitMoveAbove;
        };

        Action post = () =>
        {
            if (up)
                rectBelow.position = rectMiddle.position - difBelow;
            else
                rectAbove.position = rectMiddle.position + difAbove;

            textMiddle.text = current.data.text;
            rectMiddle.gameObject.SetActive(true);
        };

        return (pre, animation, post, 0, 0, 0, 0);
    }

    public void Animate(bool up)
    {
        StopAnimations();
        this.up = up;
        Animate();
    }

    public override void StopAnimations()
    {
        base.StopAnimations();
        ConstructActions().post?.Invoke();
    }
}