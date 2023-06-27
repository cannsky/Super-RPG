using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingGameObject : MonoBehaviour
{
    public List<Product> farmProducts;
    // Start is called before the first frame update
    void Start()
    {
        this.farmProducts = new List<Product>();
        Transform[] products = this.GetComponentsInChildren<Transform>();
        for(int i = 1; i < products.Length; i++) farmProducts.Add(products[i].gameObject.GetComponent<Product>());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Product AttachNPCToGameObject(){
        int randomNumber = 0;
        for(int i = 0; i < farmProducts.Count && farmProducts[randomNumber].isNpcWorking; i++){
            randomNumber = Random.Range(0, farmProducts.Count - 1);
            if(!farmProducts[randomNumber].isNpcWorking) break;
            if(!farmProducts[i].isNpcWorking) randomNumber = i;
        }
        Product currentFarmProduct = farmProducts[randomNumber];
        currentFarmProduct.isNpcWorking = true;
        return currentFarmProduct;
    }
}
