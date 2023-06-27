using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvent : GameEvent
{
    
    public enum Type{
        CollectItem,
        TriggerItem,
        UseItem,
    }
    
    public Type type;
    
    void Awake(){
        id = playerEventCount++;
    }
    
    public bool HandleEvent(){
        return true;
    }
}
