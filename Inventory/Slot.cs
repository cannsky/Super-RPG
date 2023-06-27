using System;
using UnityEngine;

public class Slot
{
    public int index;
    public static int lastIndex = 0;
    private Item _item;
    private int _amount;
    public Item item
    {
        get { return _item; }
        set
        {
            if (value == null) amount = 0;
            else _item = value;
        }
    }
    public int amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            currentSlot?.UIController();
            if (value <= 0)
            {
                _item = null;
                Inventory.Instance.HandleLastEmptySlots();
            }
        }
    }
    [NonSerialized]public SlotUI currentSlot;


    static Slot()
    {
        lastIndex = 0;
    }

    public Slot(Item item,int amount,int customIndex=Int32.MaxValue)
    {
        _item = item;
        _amount = item == null ? 0 : !item.stackable ? 1 : amount > 0 ? amount : 1;

        if (customIndex == Int32.MaxValue)
        {
            this.index = lastIndex;
            lastIndex++;
        }
        else
            this.index = customIndex;
    }

    public void DropSlot()
    {
        try
        {
            currentSlot.currentSlot = null;
            currentSlot = null;
        }
        catch (Exception)
        {

        }
    }

    public void AddItem(Equipment equipment)
    {
        if (this.item != null && this.item?.id != equipment.id)
            return;
        
        this.item = equipment;
        if (item is object)
            item.slot = this;
        if (currentSlot)
        {
            InventoryUI.Instance.IncreaseItem();
            currentSlot.UIController();
        }
        this.amount = 1;
    }

    public void AddItem(Item item,int amount)
    {
        if (this.item != null && this.item?.id != item.id)
            return;
        this.item = item;
        if (item is object)
            item.slot = this;
        this.amount = (item?.stackable==true) ? this.amount+amount : 1;

        if (currentSlot)
        {
            InventoryUI.Instance.IncreaseItem();
            currentSlot.UIController();
        }

        Inventory.Instance.HandleLastEmptySlots();
    }

    public Tuple<Item,int> RemoveItem(int amount)
    {
        if (amount <= 0)
            return new Tuple<Item, int>(null, 0);
        if (amount >= this.amount)
            return RemoveItem();

        this.amount -= amount;

        Item theItem = item;

        item = (this.amount == 0) ? null : item;

        currentSlot?.UIController();
        Inventory.Instance.HandleLastEmptySlots();

        return new Tuple<Item, int>(theItem,amount);
    }

    public Tuple<Item,int> RemoveItem()
    {
        int amount = (this.amount > 0) ? this.amount : 0;
        Item theItem = item;

        if (item) 
            item.slot = null;

        item = null;

        if(currentSlot)
        {
            currentSlot.UIController();
            InventoryUI.Instance.DecreaseItem();
        }

        Inventory.Instance.HandleLastEmptySlots();

        return new Tuple<Item, int>(theItem, amount);
    }
}
