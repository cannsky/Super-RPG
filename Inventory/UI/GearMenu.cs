using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class GearMenu : UIToggle, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        EquipGear(eventData);
    }

    public static void EquipGear(PointerEventData eventData)
    {
        if (SlotUI.draggedItem == null) return;

        Slot slot = SlotUI.draggedItem;
        Type type = slot.item.GetType();

        if (!type.IsSubclassOf(typeof(Equipment))) return;

        EquipmentSlotUI equipmentSlot = InventoryUI.Instance.equipmentSlots.Where(i => i.CheckType(slot.item.GetType())).First();

        equipmentSlot.OnDrop(eventData);
    }
}
