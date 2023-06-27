using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isDay = true;
    
    public int updateCounter = 0;
    
    public int updatePoint = 1000;
    
    public Transform spawners;
    
    public List<GameObject> enemySpawners = new List<GameObject>();
    
    void Start(){
        foreach(Transform spawner in spawners){
            if(spawner.name == "Enemy Spawners"){
                foreach(Transform enemySpawner in spawner){
                    enemySpawners.Add(enemySpawner.gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(updateCounter++ == updatePoint) UpdateGame();
    }
    
    void UpdateGame(){
        updateCounter = 0;
        UpdateSpawners();
    }
    
    void UpdateSpawners(){
        if(isDay) return;
        foreach(GameObject enemySpawner in enemySpawners) enemySpawner.GetComponent<EnemySpawner>().SetEnemiesActive();
    }
}
