using System;
using System.Linq;
using UnityEngine;

public class ItemDetailsControls : MonoBehaviour
{
    SlotUI currentItem;
    static ItemDetailsMenu menu;
    static Popup popup;
    static Inventory inventory;
    static InventoryUI inventoryUI;

    private static ItemDetailsControls instance;
    public static ItemDetailsControls Instance { get => instance; }

    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        menu = ItemDetailsMenu.Instance;
        inventory = Inventory.Instance;
        inventoryUI = InventoryUI.Instance;
        popup = Resources.FindObjectsOfTypeAll<Popup>()[0];
    }

    public void Load(SlotUI slot)
    {
        currentItem = slot;
    }

    public void LoadDropPopup()
    {
        if (currentItem.currentSlot.amount > 1)
            popup.LoadStackablePopup("How many do you want to drop ?", DropAmount, currentItem, menu.transform.position);
        else
            popup.LoadYesNoPopup("Do you want to drop this item ?", Drop, currentItem, menu.transform.position);
    }

    public static void LoadDropPopup(SlotUI slot,Vector2 position)
    {
        if(slot.currentSlot.amount>1)
            popup.LoadStackablePopup("How many do you want to drop ?",DropAmount,slot,position);
        else
            popup.LoadYesNoPopup("Do you want to drop this item ?", Drop, slot,position);
    }

    private static void Drop(SlotUI slotUI)
    {
        if(slotUI.GetType() ==typeof(InventorySlotUI))
        {
            inventory.DropItem(slotUI.currentSlot.index);
            menu.Unload();
        }
        else
        {
            inventory.DropEquipmentItem(slotUI.currentSlot.index);
            menu.Unload();
        }
    }

    private static void DropAmount(SlotUI slotUI, int amount)
    {
        inventory.DropItem(slotUI.currentSlot.index, amount);
        if (slotUI.currentSlot?.item == null)
        {
            menu.Unload();
        }
    }

    public void LoadSplitPopup()
    {
        popup.LoadStackablePopup("How many do you want to split ?", Split, currentItem, menu.transform.position);
    }

    public void Split(SlotUI slotUI, int amount)
    {
        Slot slot = null;
        int slotCount = inventoryUI.loadedInventorySlots.Count;
        int index = inventoryUI.loadedInventorySlots.IndexOf(slotUI.currentSlot);

        var emptySlot1 = inventoryUI.loadedInventorySlots.GetRange(index, slotCount - index).Where(s => s.item == null).FirstOrDefault();
        var emptySlot2 = inventoryUI.loadedInventorySlots.GetRange(0, index).Where(s => s.item == null).LastOrDefault();

        int temp1 = inventoryUI.loadedInventorySlots.IndexOf(emptySlot1);
        int temp2 = inventoryUI.loadedInventorySlots.IndexOf(emptySlot2);

        temp1 = temp1 == -1 ? Int32.MaxValue : temp1 - index;
        temp2 = temp2 == -1 ? Int32.MaxValue : index - temp2;

        if (temp1 <= temp2) slot = emptySlot1;
        else slot = emptySlot2;


        var split = slotUI.currentSlot.RemoveItem(amount);
        slot.AddItem(split.Item1, split.Item2);

        if (slotUI.currentSlot?.item == null)
        {
            menu.Unload();
        }
        else if (split.Item2 > 0)
        {
            inventoryUI.IncreaseItem();
        }
    }

    public void LoadStackAllPopup()
    {
        popup.LoadYesNoPopup("Are you sure you want to stack all items of this kind on this slot ?", StackAll, currentItem, menu.transform.position);
    }

    public static void StackAll(SlotUI slotUI)
    {
        var items = inventoryUI.loadedInventorySlots.Where(s =>s.item!=null).Where(s=>s.item.Equals(slotUI.currentSlot.item)).ToList();

        foreach (var item in items)
        {
            var removed = item.RemoveItem();
            slotUI.currentSlot.AddItem(removed.Item1, removed.Item2);
            inventoryUI.DecreaseItem();
        }
        inventoryUI.IncreaseItem();
    }

    //After consumables and effects are implemented...
    public void Use()
    {
        Consumable consumable = (Consumable)currentItem.currentSlot.item;
        consumable.Use();
    }

    public void Equip()
    {
        SlotUI.draggedItem = currentItem.currentSlot;
        SlotUI.draggedItemOriginalPage = inventoryUI.currentPage;
        GearMenu.EquipGear(null);
        SlotUI.draggedItem = null;
        menu.Unload();
    }

    public void Unequip()
    {
        SlotUI.draggedItem = currentItem.currentSlot;
        SlotUI.draggedItemOriginalPage = 0;
        InventoryPanel.Unequip(null);
        SlotUI.draggedItem = null;
        menu.Unload();
    }
}