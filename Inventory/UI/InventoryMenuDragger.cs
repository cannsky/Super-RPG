using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMenuDragger : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
{
    Vector3 currentScale;
    [SerializeField] Canvas menu;
    [SerializeField] float scaleFactor = 0.9f;
    bool scaleChanged=false;
    Vector2 delta = new Vector2(5f, 5f);
    [SerializeField] InventoryUIManager manager;
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
        menu.transform.position -= new Vector3(revert.x, revert.y, 0);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!manager.CanMove()) return;
        menu.transform.SetSiblingIndex(2);
        currentScale =  menu.transform.localScale;
        menu.transform.localScale = new Vector3(currentScale.x * scaleFactor, currentScale.y * scaleFactor, currentScale.z * scaleFactor);
        scaleChanged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!manager.CanMove()) return;
        menu.transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!manager.CanMove()) return;
        Vector3 currentScale = menu.transform.localScale;
        menu.transform.localScale = new Vector3(currentScale.x / scaleFactor, currentScale.y / scaleFactor, currentScale.z / scaleFactor);
        scaleChanged = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        menu.transform.SetSiblingIndex(2);
    }

    private void OnApplicationQuit()
    {
        if(scaleChanged)
            menu.transform.localScale = new Vector3(currentScale.x / scaleFactor, currentScale.y / scaleFactor, currentScale.z / scaleFactor);
    }
}