using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup),typeof(ClickHandler))]
public abstract class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField] protected Image draggingImage;
    [SerializeField] DropArea dropArea;
    [SerializeField] TMP_Text itemText;
    [NonSerialized] public static Slot draggedItem;
    [NonSerialized] internal Slot currentSlot;

    protected Image itemImage;
    protected CanvasGroup group;
    protected Canvas canvas;
    protected InventoryUI inventory;
    public static int draggedItemOriginalPage = 0;
    bool dragging = false;
    ItemDetailsMenu menu;

    internal virtual void Start()
    {
        itemImage = GetComponentsInChildren<Image>(true).Where(i => i.name == "Item").FirstOrDefault();
        itemImage.color = new Color(itemImage.color.r, itemImage.color.g, itemImage.color.b, 1f);
        group = GetComponent<CanvasGroup>();
        canvas = null;
        inventory = InventoryUI.Instance;
        menu = ItemDetailsMenu.Instance;
        Transform _transform = transform;
        while (canvas == null)
        {
            _transform = _transform.parent;
            canvas = _transform.GetComponent<Canvas>();
        }
    }

    internal void LoadSlot(Slot slot)
    {
        if (currentSlot != null)
            currentSlot.currentSlot = null;
        slot.currentSlot = this;
        currentSlot = slot;
        UIController();
    }

    public abstract void UIController();

    protected virtual void UpdateDragSourceSlot(int page)
    {
        if (draggedItem == null) return;
        if (page <= 0) return;
        if (currentSlot.index % 40 != draggedItem.index % 40)
            return;

        if (draggedItemOriginalPage == page)
        {
            GetIntoDragForm();
        }
        else
        {
            GetOutOfDragForm();
        }
    }

    protected void GetIntoDragForm()
    {
        itemImage.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        group.alpha = .5f;
        group.blocksRaycasts = false;
    }

    protected void GetOutOfDragForm()
    {
        itemImage.transform.localScale = new Vector3(1f, 1f, 1f);
        group.alpha = 1f;
        group.blocksRaycasts = true;
    }


    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
            GetComponent<ClickHandler>().OnPointerClick(eventData);
        if (currentSlot.item != null)
        {
            dropArea.Toggle();
            dragging = true;
            draggedItem = currentSlot;
            draggingImage.enabled = true;
            draggingImage.sprite = draggedItem.item.icon;
            draggingImage.transform.position = new Vector2(eventData.position.x, eventData.position.y);
            GetIntoDragForm();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragging)
        {
            draggingImage.transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }
    }

    
    public virtual void OnDrop(PointerEventData eventData)
    {
        if (draggedItem == null)
            return;

        if (!draggedItem.item.Equals(currentSlot?.item))
        {
            if (draggedItemOriginalPage == 0)
               Inventory.Instance.SwapItemsToEquipment(currentSlot, draggedItem);
            else
                Inventory.Instance.SwapItems(draggedItem, currentSlot);
        }
        else if (currentSlot.item?.stackable == true)
        {
            var draggedSlot = Inventory.Instance.inventorySlots[draggedItem.index].RemoveItem();
            currentSlot.AddItem(draggedSlot.Item1, draggedSlot.Item2);
            inventory.DecreaseItem();
        }

        inventory.CheckPageFormation();
    }

    public abstract void OnEndDrag(PointerEventData eventData);

    protected void EndDragging()
    {
        dragging = false;
        draggedItem = null;
        draggingImage.enabled = false;
        dropArea.Toggle();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData == null) return;

        if(eventData.button==PointerEventData.InputButton.Left)
        {
            inventory.UpdateItemText(currentSlot.item?.name);
        }
        else if(eventData.button==PointerEventData.InputButton.Right)
        {
            menu.Load(this);
        }
    }
}