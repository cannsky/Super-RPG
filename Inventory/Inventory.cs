using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<Item> allItems;
    [NonSerialized]public List<Slot> inventorySlots;
    [NonSerialized] public List<Slot> equipmentSlots;
    [NonSerialized] public bool isStarted = false;

    private static Inventory instance;
    public static Inventory Instance { get=> instance; }
    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        Resources.LoadAll(string.Empty);
        allItems = Resources.FindObjectsOfTypeAll<Item>().OrderBy(i => i.id).ToList();
    }
    internal void Start()
    {
        if (isStarted)
            return;
        inventorySlots = new List<Slot>();
        equipmentSlots = new List<Slot>();
        for (int i = 1; i < 19; i++)
        {
            Slot empty = new Slot(null, 0, -i);
            equipmentSlots.Add(empty);
        }

        System.Random rnd = new System.Random();
        HandleLastEmptySlots();

        foreach (var item in allItems)
        {
            Item instance = ScriptableObject.Instantiate<Item>(item);
            AddItem(instance, rnd.Next(1, 100), false);
        }
        //Load stuff
        HandleLastEmptySlots();

        isStarted = true;
    }

    public void SwapItems(Slot slot1,Slot slot2)
    {
        Tuple<Item, int> temp1 = slot1.RemoveItem();
        Tuple<Item, int> temp2 = slot2.RemoveItem();
        slot2.AddItem(temp1.Item1, temp1.Item2);
        slot1.AddItem(temp2.Item1, temp2.Item2);
    }


    public void SwapItemsToEquipment(Slot inventorySlot, Slot equipmentSlot)
    {
        EquipmentSlotUI equipment = ((EquipmentSlotUI)equipmentSlot.currentSlot);

        Type type = inventorySlot.item?.GetType();

        if (!equipment.CheckType(type)) return;

        Tuple<Item, int> temp1 = inventorySlot.RemoveItem();
        Tuple<Item, int> temp2 = equipmentSlot.RemoveItem();

        equipmentSlot.AddItem(temp1.Item1, temp1.Item2);
        inventorySlot.AddItem(temp2.Item1, temp2.Item2);
    }

    public void SwapItemsInEquipment(Slot slot1, Slot slot2)
    {
        EquipmentSlotUI sl1 = ((EquipmentSlotUI)slot1.currentSlot);
        EquipmentSlotUI sl2 = ((EquipmentSlotUI)slot2.currentSlot);

        bool con1 = sl1.CheckType(sl2.GetType());
        bool con2 = sl2.CheckType(sl1.GetType());

        if (!(con1 && con2)) return;

        Tuple<Item, int> temp1 = slot1.RemoveItem();
        Tuple<Item, int> temp2 = slot2.RemoveItem();

        slot2.AddItem(temp1.Item1, temp1.Item2);
        slot1.AddItem(temp2.Item1, temp2.Item2);
    }

    public Tuple<Item,int> DropItem(int index)
    {
        var returnValue = inventorySlots[index].RemoveItem();
        HandleLastEmptySlots();

        return returnValue;
    }

    public Tuple<Item,int> DropEquipmentItem(int index)
    {
        var returnValue = equipmentSlots[Mathf.Abs(index)-1].RemoveItem();

        return returnValue;
    }

    public Tuple<Item,int> DropItem(int index,int amount)
    {
        var returnValue = inventorySlots[index].RemoveItem(amount);
        HandleLastEmptySlots();

        return returnValue;
    }


    public void AddItem(Item item,int amount,bool stacking)
    {
        if (!item)
            return;

        if(item.stackable && stacking)
        {
            Slot slot = inventorySlots.Where(i => i.item != null).ToList().Find(i => i.item.Equals(item));
            
            if(slot!=null)
                slot.amount += amount;
            else
                inventorySlots[inventorySlots.Count - 40].AddItem(item, amount);
        }
        else
            inventorySlots[inventorySlots.Count - 40].AddItem(item, amount);
    }

    public void ClearEmptySlots()
    {
        List<Slot> tempList = new List<Slot>();
        int counter = 0;
        foreach (var slot in inventorySlots)
        {
            if (slot.item != null)
            {
                slot.index = counter;
                tempList.Add(slot);
                counter++;
            }
        }
        inventorySlots.Clear();
        Slot.lastIndex = counter;
        foreach (var slot in tempList)
        {
            inventorySlots.Add(slot);
        }
        HandleLastEmptySlots();
    }

    public void HandleLastEmptySlots()
    {
        inventorySlots.Reverse();
        int counter = 0;
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == null)
                counter++;
            else break;
        }

        int addCount = 40 - counter;

        if(addCount<0)
        {
            addCount = Mathf.Abs(addCount);

            for (int i = 0; i < addCount; i++)
            {
                inventorySlots.RemoveAt(0);
                Slot.lastIndex--;
            }
            inventorySlots.Reverse();
        }
        else
        {
            inventorySlots.Reverse();
            for (int i = 0; i < addCount; i++)
            {
                Slot empty = new Slot(null, 0);
                inventorySlots.Add(empty);
            }
        }
    }

}