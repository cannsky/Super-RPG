using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ItemDetailsMenu.Instance.isOpened)
            ItemDetailsMenu.Instance.Unload();
        SlotUI slot;
        if (TryGetComponent<SlotUI>(out slot))
            slot.OnPointerClick(eventData);
    }
}