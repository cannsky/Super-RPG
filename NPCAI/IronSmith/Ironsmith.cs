using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ironsmith : MonoBehaviour
{
    [System.Serializable]
    public class IronsmithItem {
        
        public Item item;
        public int price;
        public int amount;
        
        public IronsmithItem(Item item, int price){
            this.item = item;
            this.price = price;
        }
        
    }
    
    [System.Serializable]
    public class IronsmithResourceItem{
        
        public Item item;
        public int amount;
        
    }
    
    private class IronsmithState{
        
        public bool isForging;
        public bool isBuyingItems;
        public bool isReturning;
        public bool isWaiting;
        
        public void SetIronsmithState(bool isForging = false, bool isBuyingItems = false, bool isReturning = false, bool isWaiting = false){
            this.isForging = isForging;
            this.isBuyingItems = isBuyingItems;
            this.isReturning = isReturning;
        }
    }
    
    private class IronsmithAnimatorState{
        
        public Animator animator;
        
        public bool isWalking;
        public bool isForging;
        public bool isTalking;
        public bool isWaiting;
        
        public void SetIronsmithAnimatorState(bool isWalking = false, bool isForging = false, bool isTalking = false, bool isWaiting = false){
            this.isWalking = isWalking;
            this.isForging = isForging;
            this.isTalking = isTalking;
            this.isWaiting = isWaiting;
        }
    }
    
    private class IronsmithTimeState{
        
        public float walkingTime = 10f;
        public float forgingTime = 10f;
        public float talkingTime = 10f;
        public float waitingTime = 10f;
        
    }
    
    private IronsmithState ironsmithState;
    private IronsmithAnimatorState ironsmithAnimatorState;
    private IronsmithTimeState ironsmithTimeState;
    
    private Object[] items;
    public List<IronsmithItem> ironsmithInventory;
    public List<IronsmithResourceItem> ironsmithResourceInventory;
    
    void Start(){
        ironsmithInventory = new List<IronsmithItem>();
        ironsmithResourceInventory = new List<IronsmithResourceItem>();
        this.ironsmithState = new IronsmithState();
        this.ironsmithAnimatorState = new IronsmithAnimatorState();
        this.ironsmithTimeState = new IronsmithTimeState();
        this.ironsmithState.SetIronsmithState(isWaiting: true);
    }
    
    void Update(){
        
    }
    
}
