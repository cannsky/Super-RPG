using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event/World Event")]
public class WorldEvent : GameEvent
{
    public enum Type {
        ChangeScene,
        DisablePrefab,
        EnablePrefab,
        PlayMusic,
        ShowCinematic,
        ShowGUIText
    }
    
    public Type type;
    
    void Awake(){
        id = worldEventCount++;
    }
    
    public bool HandleEvent(){
        return true;
    }
    
    public bool PlayMusic(){
        if(base.ConvertEventVariableToInt()) return false;
        if(!MusicManager.musicManager.PlayEventMusic(base.eventVariable)) return false;
        return true;
    }
}
