using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class InventorySlotUI : SlotUI
{

    [NonSerialized] public bool isStarted = false;
    TMP_Text amountText;


    internal override void Start()
    {
        if (isStarted)
            return;
        base.Start();
        amountText = GetComponentInChildren<TMP_Text>();
        inventory.PageChange += UpdateDragSourceSlot;
        
        isStarted = true;
    }


    public override void UIController()
    {
        if (!currentSlot?.item)
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
            amountText.text = "";
        }
        else
        {
            itemImage.enabled = true;
            itemImage.sprite = currentSlot.item.icon;
            amountText.text = currentSlot.amount <= 1 ? "" : currentSlot.amount.ToString();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        draggedItemOriginalPage = inventory.currentPage;
        base.OnBeginDrag(eventData);
    }


    public override void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem == null)
            return;

        UpdateDragSourceSlot(draggedItemOriginalPage + 1);
        EndDragging();
    }
}
