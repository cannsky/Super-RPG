using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Consumable", menuName = "Inventory/Consumable/Consumable")]
public class Consumable : Item,IHotBar
{
    [Header(header:"Please care to use only one of these effects unless you have better plans")]
    public InstantDamageEffect[] instantDamageEffects;
    public DamageOverTimeEffect[] damageOverTimeEffects;
    public InstantRegenerationEffect[] instantRegenerationEffects;
    public RegenerationOverTimeEffect[] regenerationOverTimeEffects;
    public BuffEffect[] buffEffects;
    public WeakeningEffect[] weakeningEffects;
    public MovementBuffEffect[] movementBuffEffects;
    public MovementWeakeningEffect[] movementWeakeningEffects;

    public int GetAmount() => slot is object? slot.amount : 0;

    public Sprite GetImage() => icon;

    public void Use()
    {
        foreach (var item in instantDamageEffects)
        {
            item.Execute(name);
        }
        foreach (var item in damageOverTimeEffects)
        {
            item.Execute(name);
        }
        foreach (var item in instantRegenerationEffects)
        {
            item.Execute(name);
        }
        foreach (var item in regenerationOverTimeEffects)
        {
            item.Execute(name);
        }
        foreach (var item in buffEffects)
        {
            item.Execute(name);
        }
        foreach (var item in weakeningEffects)
        {
            item.Execute(name);
        }
        foreach (var item in movementBuffEffects)
        {
            item.Execute(name);
        }
        foreach (var item in movementWeakeningEffects)
        {
            item.Execute(name);
        }
        
        slot.amount--;
        if (slot.amount < 1)
            slot.RemoveItem();
    }
}