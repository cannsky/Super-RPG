using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public static Player player;
    public int gold;
    public int level;
    public int exp;
    public int health;
    public int mana;
    public int questGoalCount;
    public int questGoalId;
    public bool isPlayerTakenDown;
    public bool isPlayerFighting;
    public int healthCapacity;
    public int manaCapacity;
    
    void Awake(){
        player = this;
    }
}
