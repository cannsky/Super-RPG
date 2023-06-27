using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HotBarUIManager : MonoBehaviour
{
    public List<HotBarSlotUI> hotBarSlots;

    private void Start()
    {
        
    }

    public void UseHotBar(int number)
    {
        if (number < 1 || number > 10)
            return;

        hotBarSlots[number-1]?.Use();
    }

    public bool Add(IHotBar hotBar)
    {
        var slot = hotBarSlots.Where(s => s.currentHotBarItem == null).FirstOrDefault();

        if (!slot) return false;
        slot.Load(hotBar);
        return true;
    }

    public void AddToSlot(IHotBar hotBar,int number)
    {
        if (number < 1 || number > 10)
            return;
        hotBarSlots[number-1].Load(hotBar);
    }
}