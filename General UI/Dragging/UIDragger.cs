using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    //Serialized Fields
    public UIManager manager;
    public DragTarget target;
    [SerializeField] float scaleFactor = 0.9f;
    [SerializeField] int draggingSiblingIndex;
    [SerializeField] Vector2 delta = new Vector2(5f, 5f);

    //Caches
    Vector3 currentScale;
    bool scaleChanged = false;
   
    public void RevertDrag(Blocker.BlockerType direction)
    {
        Vector2 revert = new Vector2();

        switch (direction)
        {
            case Blocker.BlockerType.Top:
                revert = new Vector2(0, delta.y);
                break;
            case Blocker.BlockerType.Bottom:
                revert = new Vector2(0, -delta.y);
                break;
            case Blocker.BlockerType.Right:
                revert = new Vector2(delta.x, 0);
                break;
            case Blocker.BlockerType.Left:
                revert = new Vector2(-delta.x, 0);
                break;
        }
        target.Drag(new Vector3(-revert.x, -revert.y, 0));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CanMove())
        {
            target.SetSiblingIndex(draggingSiblingIndex);
            currentScale = target.GetLocalScale();
            target.SetLocalScale(new Vector3(currentScale.x * scaleFactor, currentScale.y * scaleFactor, currentScale.z * scaleFactor));
            scaleChanged = true;
        }
        else Debug.Log("wtf man");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(CanMove())
            target.Drag(new Vector3(eventData.delta.x, eventData.delta.y, 0));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CanMove())
        {
            currentScale = target.GetLocalScale();
            target.SetLocalScale(new Vector3(currentScale.x / scaleFactor, currentScale.y / scaleFactor, currentScale.z / scaleFactor));
            scaleChanged = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        target.SetSiblingIndex(draggingSiblingIndex);
    }

    bool CanMove() => !manager || (manager && manager.CanMove());

    private void OnApplicationQuit()
    {
        if (scaleChanged)
            target.SetLocalScale(new Vector3(currentScale.x / scaleFactor, currentScale.y / scaleFactor, currentScale.z / scaleFactor));
    }
}
