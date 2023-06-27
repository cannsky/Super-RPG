using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class InfiniteScroll : UIAnimator
{
    [SerializeField] float maxY;
    [SerializeField] float minY;

    [NonSerialized] public float destination;
    [NonSerialized] public RectTransform content;
    [NonSerialized] public RectTransform scrollRect;

    Vector3 unitMove;
    List<RectTransform> children;
    Vector3[] childrenLocalPositions;

    float childHeight;
    float spacing;
    int maxIndex;
    float move;
    protected override void Start()
    {
        base.Start();

        scrollRect = GetComponent<RectTransform>();

        content = GetComponentInChildren<VerticalLayoutGroup>().GetComponent<RectTransform>();
        content.localPosition = new Vector3(content.localPosition.x, maxY, content.localPosition.z);

        var temp = content.GetComponent<VerticalLayoutGroup>();
        spacing = temp.spacing;
        //temp.enabled = false;

        UpdateChildren();
        childHeight = (content.rect.height - (children.Count - 1) * spacing) / children.Count;
        maxIndex = children.Count-1;

        childrenLocalPositions = new Vector3[children.Count];

        for (int i = 0; i < children.Count; i++)
            childrenLocalPositions[i] = children[i].localPosition;


        //temp.enabled = true;
    }

    void UpdateChildren() => children = content.GetComponentsInChildren<RectTransform>().Where(c => c != content).ToList();

    void UpdateChildrenPositions()
    {
        for (int i = 0; i < children.Count; i++)
            children[i].localPosition = childrenLocalPositions[i];
    }

    protected override (Action pre, Action animation, Action post,
        float waitBeforePre, float waitBeforeAnimation, float waitBeforePost, float waitAfterPost)
        ConstructActions()
    {
        Action pre = () =>
        {
            //unitMove = new Vector3(0, (destination - content.localPosition.y) / totalFrames, 0);
            unitMove = new Vector3(0, move / totalFrames, 0);
        };

        Action animation = () =>
        {
            content.position += unitMove;
        };

        Action post = null;

        return (pre, animation, post, 0, 0, 0, 0);
    }

    public void SetDestinationAndAnimate(bool up)
    {
        int index = GetVisibleElementIndex();
        index += up ? - 1 : 1;

        if(index<0)
        {
            content.localPosition = new Vector3(0, maxY, 0);
            children[0].SetSiblingIndex(children.Count - 1);
            UpdateChildren();
            UpdateChildrenPositions();
            index = maxIndex - 1;
        }
        else if(index>maxIndex)
        {
            content.localPosition = new Vector3(0, minY, 0);
            children[children.Count - 1].SetSiblingIndex(0);
            UpdateChildren();
            UpdateChildrenPositions();
            index = 1;
        }

        SetDestination(index);
        Animate();
    }
    public override void StopAnimations()
    {
        StopAllCoroutines();
        content.localPosition = new Vector3(content.localPosition.x, destination, content.localPosition.z);
    }
    void SetDestination(int index) 
    {
        //destination = minY + index * childHeight + (index-1)*spacing;
        //move = scrollRect.position.y - children[index].position.y;
        move = childrenLocalPositions[GetVisibleElementIndex()].y - childrenLocalPositions[index].y;

        
        Debug.Log("Move Vector : " + move);
    }
    int GetVisibleElementIndex()
    {
        float dif = content.localPosition.y - minY;
        int index = Mathf.RoundToInt(dif / (childHeight+spacing/2f));
        return index;
    }

    

    //public void OnBeginDrag(PointerEventData eventData) => OnDrag(eventData);
    //public void OnDrag(PointerEventData eventData) => OnValueChanged();
    //public void OnScroll(PointerEventData eventData) => OnValueChanged();

    //private void OnValueChanged()
    //{
    //    CancelInvoke();
    //    Invoke("FixContent", 0.1f);

    //    float deltaTime = Time.time - lastTime;
    //    float way = content.localPosition.y - lastPos.y;

    //    if (Mathf.Abs(way) / deltaTime > maxVelocity)
    //    {
    //        fixVector.y = deltaTime*(way < 0 ? - maxVelocity : maxVelocity);
    //        content.localPosition = lastPos + fixVector;
    //    }

    //    if (content.localPosition.y >= maxY - error)
    //    {
    //        content.localPosition = new Vector3(0, minY, 0);
    //        children[children.Count - 1].SetSiblingIndex(0);
    //        UpdateChildren();
    //        UpdateChildrenPositions();
    //    }
    //    else if (content.localPosition.y <= minY + error)
    //    {
    //        content.localPosition = new Vector3(0, maxY, 0);
    //        children[0].SetSiblingIndex(children.Count - 1);
    //        UpdateChildren();
    //        UpdateChildrenPositions();
    //    }

    //    lastPos = content.localPosition;
    //    lastTime += deltaTime;
    //}
}