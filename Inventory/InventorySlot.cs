using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //Icon of the item
    public Image icon;
    //Remove button
    public Button removeButton;
    //Item
    Item item;
    //Adds item to the slot
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }
    //Removes item from the slot
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }
    //When remove button clicked
    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }
    //Use item
    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }
    }
}
