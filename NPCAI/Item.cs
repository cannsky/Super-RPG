using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string name;
    public int id;
    public int lowestPrice;
    public int highestPrice;
    
    public CraftRecipe craftRecipe;
    
    void Awake(){
        
    }
}
