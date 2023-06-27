using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public float spawnRange;
        public int size;
    }
    
    [System.Serializable]
    public class PooledObjects{
        public string tag;
        public List<GameObject> objects;
        public PooledObjects(string tag){
            this.tag = tag;
        }
    }
    
    public List<Pool> pools;
    
    public List<PooledObjects> pooledLists = new List<PooledObjects>();
    
    void Start(){
        SpawnEnemy();
    }
    
    public void SpawnEnemy(){
        foreach(Pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();
            pooledLists.Add(new PooledObjects(pool.tag));
            foreach(PooledObjects pooledObjects in pooledLists){
                if(pooledObjects.tag == pool.tag){
                pooledObjects.objects = new List<GameObject>();
                    for(int i = 0; i < pool.size; i++){
                        GameObject obj = Instantiate(pool.prefab, new Vector3(transform.position.x + Random.Range(-pool.spawnRange, pool.spawnRange), transform.position.y, transform.position.z + Random.Range(-pool.spawnRange, pool.spawnRange)), Quaternion.identity);
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);
                        pooledObjects.objects.Add(obj);
                    }
                }
            }
        }
    }
    
    public void SetEnemiesActive(){
        foreach(Pool pool in pools){
            foreach(PooledObjects pooledObjects in pooledLists){
                if(pooledObjects.tag == pool.tag){
                    foreach(GameObject enemyObject in pooledObjects.objects){
                        enemyObject.SetActive(true);
                    }
                }
            }
        }
    }
    
    public void SetEnemiesDeactive(){
        foreach(PooledObjects pooledObjects in pooledLists){
            foreach(GameObject enemyObject in pooledObjects.objects){
                enemyObject.SetActive(false);
            }
        }
    }
}
