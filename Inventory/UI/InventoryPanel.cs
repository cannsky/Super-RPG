using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPanel : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Unequip(eventData);
    }

    public static void Unequip(PointerEventData eventData)
    {
        if (SlotUI.draggedItem == null) return;

        if (SlotUI.draggedItem.currentSlot.GetType() != typeof(EquipmentSlotUI)) return;


        foreach (var item in InventoryUI.Instance.inventorySlots)
        {
            if (item.currentSlot.item == null)
            {
                item.OnDrop(eventData);
                return;
            }
        }

        foreach (var item in InventoryUI.Instance.loadedInventorySlots)
        {
            if (item.item == null)
            {
                Inventory.Instance.SwapItemsToEquipment(item, SlotUI.draggedItem);
                return;
            }
        }

        Equipment equipment = (Equipment)SlotUI.draggedItem.item;
        equipment.OnUnequip();
    }
}
