using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //Item of the game object
    public Item item;
    //Adds item to the inventory
    public void PickUp()
    {
        if(Inventory.instance.Add(item)) Destroy(gameObject);
    }

}
