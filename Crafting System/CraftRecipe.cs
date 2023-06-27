using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftRecipe
{
    [System.Serializable]
    public class RecipeItem {
        public Item item;
        public int amount;
    }
    
    public int outputAmount;
    public RecipeItem[] recipeItems;
}
