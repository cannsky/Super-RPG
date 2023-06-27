using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public delegate void ItemHandler();
public abstract class Item : ScriptableObject
{
    public int id = 0;
    //Item's name
    new public string name = "New Item";

    public string description = "Description";
    //Item's icon
    public Sprite icon = null;
    //Is item default item or not
    public bool isDefaultItem = false;

    public bool stackable = false;

    public Slot slot;

    public CraftRecipe craftRecipe;
    public override bool Equals(object other)
    {
        if (other == null)
            return false;
        if (!(other.GetType().IsSubclassOf(typeof(Item)) || other.GetType()==typeof(Item)))
            return false;
        Item otherItem = (Item)(other);

        if (id == otherItem.id)
        {
            //These if else are temporary for the time items are created to prevent duplicate ids
            if (name.Equals(otherItem.name))
                return true;
            else
            {
                Debug.LogWarning("Duplicate ID!!!\n" + "ID : " + id + " ;\nItem 1 : " + name + "\nItem 2 : " + otherItem.name);
                return false;
            }
        }

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public void Pickup()
    {
        //very temporary
        Inventory.Instance.AddItem(this, 1, stackable);
    }
}