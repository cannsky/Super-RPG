using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : ScriptableObject
{
    internal static int playerEventCount;
    internal static int worldEventCount;
    internal static int questEventCount;
    
    public int id;
    
    public GameObject eventGameObject;
    public GameObject targetGameObject;
    
    public string variable;
    public int eventVariable;
    
    public int eventItem;
    public int eventItemAmount;
    
    public bool ConvertEventVariableToInt(){
        if(!int.TryParse(variable, out eventVariable)) return false;
        return true;
    }
    
    public bool ConvertEventVariableToItem(){
        string[] eventItemValues = variable.Split(':');
        if(eventItemValues.Length != 2) return false;
        if(!int.TryParse(eventItemValues[0], out eventItem)) return false;
        if(!int.TryParse(eventItemValues[1], out eventItemAmount)) return false;
        return true;
    }
}
