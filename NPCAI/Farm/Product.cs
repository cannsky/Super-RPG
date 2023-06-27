using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public enum Type{
        Vegetable,
        Fruit,
        Mashroom,
        Flower,
        PotionMaterial
    };
    public Type productType;
    public bool isNpcWorking;
    
    private Object[] items;
    
    public Item CreateProduct(){
        items = Resources.LoadAll("Items/" + productType.ToString());
        int randomNumber = Random.Range(0, items.Length - 1);
        return (Item)(items[randomNumber]);
    }
}
