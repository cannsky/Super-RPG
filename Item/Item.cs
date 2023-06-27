using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Invetory/Item")]

public class Item : ScriptableObject
{
    //Item's name
    new public string name = "New Item";
    //Item's icon
    public Sprite icon = null;
    //Is item default item or not
    public bool isDefaultItem = false;

    public virtual void Use()
    {
        //Will be added.
        Debug.Log("item used");
    }
}
