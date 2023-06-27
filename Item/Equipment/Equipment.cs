using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Equipment : Item,IHotBar
{
    public StatFields equipmentStats;
    public bool isEquipped;

    //id ver,childrenlarda dolas id ye gore setactive yap job done

    public void OnEquip()
    {
        if (!isEquipped)
        {
            isEquipped = true;
            Player.Instance.EquipGear(this);
        }

        //model update
    }
    public void OnUnequip()
    {
        if (isEquipped)
        {
            isEquipped = false;
            Player.Instance.UnequipGear(this);
        }

        //model update
        Debug.Log("Unequipped");
    }

    public void Use()
    {
        //fill here
    }

    public int GetAmount() => slot is object ? 1 : 0;

    public Sprite GetImage() => icon;
}
