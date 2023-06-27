using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    
    private MerchantAnimatorState merchantAnimatorState;
    
    [System.Serializable]
    public class MerchantItem {
        
        public Item item;
        public int price;
        public int amount;
        
        public MerchantItem(Item item, int price){
            this.item = item;
            this.price = price;
        }
        
    }
    
    public class MerchantAnimatorState {
        
        public Animator animator;
        
        public bool isTalking;
        
        public MerchantAnimatorState(Animator animator){
            this.animator = animator;
        }
        
        public void SetMerchantAnimatorState(bool isTalking = false){
            this.isTalking = isTalking;
            this.UpdateAnimator();
        }
        
        private void UpdateAnimator(){
            if(this.isTalking) this.animator.SetBool("Talking", true);
            else this.animator.SetBool("Talking", false);
        }
    }
    
    private Object[] items;
    public List<MerchantItem> merchantInventory;
    public Transform npcTalkLocation;
    
    public MerchantAnimatorState GetMerchantAnimatorState(){
        return this.merchantAnimatorState;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        merchantAnimatorState = new MerchantAnimatorState(gameObject.GetComponent<Animator>());
        merchantInventory = new List<MerchantItem>();
        items = Resources.LoadAll("Items");
        foreach(Object item in items)
            merchantInventory.Add(new MerchantItem((Item)item, (((Item)item).lowestPrice + ((Item)item).highestPrice) / 2));
    }
    
    public void SellItems(List<Item> workerInventory){
        foreach(Item workerItem in workerInventory)
            foreach(MerchantItem merchantItem in merchantInventory)
                if(merchantItem.item == workerItem) {
                    merchantItem.price = (merchantItem.price > merchantItem.item.lowestPrice) ? merchantItem.price - (merchantItem.amount++) : merchantItem.price;
                    break;
                }
    }
}
