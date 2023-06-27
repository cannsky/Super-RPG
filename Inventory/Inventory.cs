using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    #region Singleton
    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            return;
        }
        instance = this;
    }

    #endregion
    //InventoryUI will be updated with OnItemChanged
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    //Space of the inventory
    public int space = 0;
    //Items in the inventory
    public List<Item> items = new List<Item>();
    //Add item to the inventory
    public bool Add (Item item)
    {
        //if item is not a default item
        if (!item.isDefaultItem)
        {
            //If there is enough space
            if(items.Count >= space) return false;
            //Add item to the items list
            items.Add(item);
            //Update UI
            if(onItemChangedCallback != null) onItemChangedCallback.Invoke();
        }
        return true;
    }
    //Remove item
    public void Remove(Item item)
    {
        //Remove item
        items.Remove(item);
        //Update UI
        if(onItemChangedCallback != null) onItemChangedCallback.Invoke();
    }
}
