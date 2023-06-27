using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : SlotUI
{
    enum EquipmentTypes
    {
        Boots=0,
        ChestPlate,
        Gauntlets,
        Helmet,
        Leggings,
        Shoulders,
        Mount,
        Saddle,
        Pet,
        Amulet,
        Bracelet,
        Necklace,
        Ring,
        Mainhand,
        Offhand
    }

    [SerializeField] EquipmentTypes type;
    [NonSerialized] bool isStarted = false;
    [SerializeField] public Image defaultImage;

    internal override void Start()
    {
        if (isStarted) return;
        base.Start();
        isStarted = true;
    }

    public override void UIController()
    {
        if (!currentSlot?.item)
        {
            defaultImage.enabled = true;
            itemImage.enabled = false;
        }
        else
        {
            defaultImage.enabled = false;
            itemImage.enabled = true;
            itemImage.sprite = currentSlot.item.icon;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        draggedItemOriginalPage = 0;
        base.OnBeginDrag(eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (draggedItem == null)
            return;

        if (!draggedItem.item.Equals(currentSlot?.item))
        {
            if(draggedItemOriginalPage==0)
                Inventory.Instance.SwapItemsInEquipment(draggedItem, currentSlot);
            else
            {
                if(currentSlot.item!=null)
                {
                    Equipment oldItem = (Equipment)currentSlot.item;
                    oldItem.OnUnequip();
                }

                Equipment newItem = (Equipment)draggedItem.item;
                newItem.OnEquip();
                Inventory.Instance.SwapItemsToEquipment(draggedItem, currentSlot);
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem == null)
            return;
        GetOutOfDragForm();
        EndDragging();
    }


    new public Type GetType()
    {
        Type type = null;
        switch (this.type)
        {
            case EquipmentTypes.Boots:
                type = typeof(Boots);
                break;
            case EquipmentTypes.ChestPlate:
                type = typeof(ChestPlate);
                break;
            case EquipmentTypes.Gauntlets:
                type = typeof(Gauntlets);
                break;
            case EquipmentTypes.Helmet:
                type = typeof(Helmet);
                break;
            case EquipmentTypes.Leggings:
                type = typeof(Leggings);
                break;
            case EquipmentTypes.Shoulders:
                type = typeof(Shoulders);
                break;
            case EquipmentTypes.Mount:
                type = typeof(Mount);
                break;
            case EquipmentTypes.Pet:
                type = typeof(Pet);
                break;
            case EquipmentTypes.Amulet:
                type = typeof(Amulet);
                break;
            case EquipmentTypes.Bracelet:
                type = typeof(Bracelet);
                break;
            case EquipmentTypes.Necklace:
                type = typeof(Necklace);
                break;
            case EquipmentTypes.Ring:
                type = typeof(Ring);
                break;
            case EquipmentTypes.Mainhand:
                type = typeof(MainHand);
                break;
            case EquipmentTypes.Offhand:
                type = typeof(OffHand);
                break;
        }

        return type;
    }

    public bool CheckType(Type type)
    {
        if (type==null)
            return true;

        return type == GetType() || type.IsSubclassOf(GetType());
    }
}
