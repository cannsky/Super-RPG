using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private GameEvent gameEvent;
    
    public bool CallEvent(GameEvent gameEvent){
        this.gameEvent = gameEvent;
        return gameEvent.GetType().ToString() switch{
            "PlayerEvent" => ((PlayerEvent)gameEvent).HandleEvent(),
            "QuestEvent" => ((QuestEvent)gameEvent).HandleEvent(),
            "WorldEvent" => ((WorldEvent)gameEvent).HandleEvent()
        };
    }
    
}
