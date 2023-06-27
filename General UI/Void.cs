using UnityEngine;
using UnityEngine.EventSystems;

public class Void : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (SlotUI.draggedItem == null) return;

        ItemDetailsControls.LoadDropPopup(SlotUI.draggedItem.currentSlot,eventData.position);
    }
}