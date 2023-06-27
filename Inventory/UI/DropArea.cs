using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DropArea : MonoBehaviour,IDropHandler
{
    [SerializeField] TMP_Text itemText;
    Image self;
    TMP_Text text;

    private void Start()
    {
        self = GetComponent<Image>();
        text = self.GetComponentInChildren<TMP_Text>();
        if(!(gameObject.tag=="PreviousPage" || gameObject.tag=="NextPage"))
            Toggle();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (SlotUI.draggedItem == null) return;

        Slot slot = null;

        if(gameObject.tag=="PreviousPage")
        {
            if (InventoryUI.Instance.currentPage == 1) return;
            int page = InventoryUI.Instance.currentPage-1;

            var slots = InventoryUI.Instance.loadedInventorySlots.GetRange((page - 1) * 40, 40);
            foreach (var item in slots)
            {
                if(item.item==null)
                {
                    slot = item;
                    break;
                }
            }
        }
        else if(gameObject.tag=="NextPage")
        {
            if (InventoryUI.Instance.currentPage == InventoryUI.Instance.currentTotalPageCount) return;
            int page = InventoryUI.Instance.currentPage + 1;

            var slots = InventoryUI.Instance.loadedInventorySlots.GetRange((page - 1) * 40, 40);

            foreach (var item in slots)
            {
                if(item.item==null)
                {
                    slot = item;
                    break;
                }
            }
        }
        else
        {
            foreach (var item in InventoryUI.Instance.loadedInventorySlots)
            {
                if (item.item == null)
                {
                    slot = item;
                    break;
                }
            }
        }

        if (slot == null) return;

        if (SlotUI.draggedItem.currentSlot.GetType() == typeof(EquipmentSlotUI))
            Inventory.Instance.SwapItemsToEquipment(slot, SlotUI.draggedItem);
        else
            Inventory.Instance.SwapItems(SlotUI.draggedItem, slot);
    }

    public void Toggle()
    {
        if(self.enabled)
        {
            self.enabled = false;
            text.enabled = false;
            itemText.enabled = true;
        }
        else
        {
            itemText.enabled = false;
            self.enabled = true;
            text.enabled = true;
        }
    }
}
