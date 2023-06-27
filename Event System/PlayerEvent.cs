using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event/Player Event")]
public class PlayerEvent : GameEvent
{
    public enum Type {
        CheckLevel,
        CheckQuestGoal,
        CollectItem,
        CraftItem,
        DecreaseHealth,
        DecreaseMana,
        FastTravel,
        GiveExperience,
        GiveGold,
        GiveItem,
        IncreaseHealth,
        IncreaseMana,
        TakeDownEnemy,
        LevelUp,
        LootItem,
        RemoveItem,
        RemoveGold,
        StateUpgrade,
        Trade
    }
    
    public Type type;
    
    void Awake(){
        id = playerEventCount++;
    }
    
    public bool HandleEvent(){
        return this.type switch{
            Type.CheckLevel => CheckLevel(),
            Type.CheckQuestGoal => CheckQuestGoal(),
            Type.CollectItem => CollectItem(),
            Type.CraftItem => CraftItem(),
            Type.DecreaseHealth => DecreaseHealth(),
            Type.DecreaseMana => DecreaseMana(),
            Type.FastTravel => FastTravel(),
            Type.GiveExperience => GiveExperience(),
            Type.GiveGold => GiveGold(),
            Type.GiveItem => GiveItem(),
            Type.IncreaseHealth => IncreaseHealth(),
            Type.IncreaseMana => IncreaseMana(),
            Type.RemoveGold => RemoveGold(),
            Type.RemoveItem => RemoveItem(),
            
        };
    }
    
    public bool CheckLevel(){
        if(!base.ConvertEventVariableToInt()) return false;
        if(base.eventVariable < Player.player.level) return false;
        else return true;
    }
    
    public bool CheckQuestGoal(){
        if(!base.ConvertEventVariableToInt()) return false;
        if(base.eventVariable != Player.player.questGoalCount) return false;
        return true;
    }
    
    public bool CollectItem(){
        //IF ITEM IS QUEST ITEM UPDATE QUEST GOAL!
        string[] eventItemValues = variable.Split(':');
        if(eventItemValues.Length != 2) return false;
        int eventItem, eventItemAmount;
        if(!int.TryParse(eventItemValues[0], out eventItem)) return false;
        if(!int.TryParse(eventItemValues[1], out eventItemAmount)) return false;
        for(int i = 0; i < eventItemAmount; i++) Player.player.inventory.Add(null); //add item with id 1
        return true;
    }
    
    public bool CraftItem(){
        Item eventGameObjectItem = base.eventGameObject.GetComponent<Item>();
        CraftRecipe.RecipeItem[] recipeItems = eventGameObjectItem.craftRecipe.recipeItems;
        if(recipeItems.Length == 0) return false;
        foreach(CraftRecipe.RecipeItem recipeItem in recipeItems){
            int playerAmount = 0;
            foreach(Item item in Player.player.inventory) if(item.id == recipeItem.item.id) playerAmount++;
            if(playerAmount < recipeItem.amount) return false;
        }
        foreach(CraftRecipe.RecipeItem recipeItem in recipeItems){
            eventItem = recipeItem.item.id;
            eventItemAmount = recipeItem.amount;
            if(!this.RemoveItemFromPlayerInventory()) return false;
        }
        eventItem = eventGameObjectItem.id;
        eventItemAmount = eventGameObjectItem.craftRecipe.outputAmount;
        return AddItemToPlayerInventory();
    }
    
    public bool DecreaseHealth(){
        if(!base.ConvertEventVariableToInt()) return false;
        Player.player.health -= base.eventVariable;
        if(Player.player.health <= 0){
            Player.player.health = 0;
            Player.player.isPlayerTakenDown = true;
        }
        return true;
    }
    
    public bool DecreaseMana(){
        if(!base.ConvertEventVariableToInt()) return false;
        if(Player.player.mana - base.eventVariable < 0) return false;
        Player.player.mana -= base.eventVariable;
        return true;
    }
    
    public bool FastTravel(){
        if(Player.player.isPlayerFighting) return false;
        base.eventGameObject.GetComponent<FastTravel>().TravelToFastTravelPoint();
        return true;
    }
    
    public bool GiveGold(){
        if(!base.ConvertEventVariableToInt()) return false;
        Player.player.gold += base.eventVariable;
        return true;
    }
    
    public bool GiveExperience(){
        if(!base.ConvertEventVariableToInt()) return false;
        Player.player.exp += base.eventVariable;
        return true;
    }
    
    public bool GiveItem(){
        //Different than collect item, given by dialogue system
        if(!base.ConvertEventVariableToItem()) return false;
        return AddItemToPlayerInventory();
    }
    
    public bool IncreaseHealth(){
        if(!base.ConvertEventVariableToInt()) return false;
        Player.player.health += base.eventVariable;
        if(Player.player.health > Player.player.healthCapacity) Player.player.health = Player.player.healthCapacity;
        return true;
    }
    
    public bool IncreaseMana(){
        if(!base.ConvertEventVariableToInt()) return false;
        Player.player.mana += base.eventVariable;
        if(Player.player.mana > Player.player.manaCapacity) Player.player.mana = Player.player.manaCapacity;
        return true;
    }
    
    public bool RemoveGold(){
        if(!base.ConvertEventVariableToInt()) return false;
        if(Player.player.gold < base.eventVariable) return false;
        Player.player.gold -= base.eventVariable;
        return true;
    }
    
    public bool RemoveItem(){
        if(!base.ConvertEventVariableToItem()) return false;
        return RemoveItemFromPlayerInventory();
    }
    
    public bool AddItemToPlayerInventory(){
        for(int i = 0; i < eventItemAmount; i++) Player.player.inventory.Add(null); //add item with id 1
        return true;
    }
    
    public bool RemoveItemFromPlayerInventory(){
        int playerAmount = 0;
        foreach(Item item in Player.player.inventory) if(item.id == base.eventItem) playerAmount++;
        if(playerAmount < base.eventItemAmount) return false;
        foreach(Item item in Player.player.inventory) if(item.id == base.eventItem){
            Player.player.inventory.Remove(item);
            if(--playerAmount == 0) break;
        }
        return true;
    }
}
