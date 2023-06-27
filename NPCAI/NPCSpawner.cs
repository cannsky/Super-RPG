using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    
    [System.Serializable]
    public class Pool{
        public NPCWorkerAI.Type tag;
        public GameObject prefab;
        public int size;
        public int count;
    }
    
    public GameObject farmPath, minePath, huntingPath;
    public GameObject farm, mine, hunting;
    
    public List<Pool> pools;
    public Dictionary<NPCWorkerAI.Type, Queue<GameObject>> poolDictionary;
    
    public class SpawnState{
        public bool isNpcSpawning = false;
        
        public void SetSpawnState(bool isNpcSpawning = false){
            this.isNpcSpawning = isNpcSpawning;
        }
    }
    
    private IEnumerator coroutine;
    
    private SpawnState spawnState;
    
    void Start(){
        spawnState = new SpawnState();
        poolDictionary = new Dictionary<NPCWorkerAI.Type, Queue<GameObject>>();
        SetPoolDictionary();
    }
    
    void Update(){
        if(!spawnState.isNpcSpawning) foreach(Pool pool in pools) if (pool.count < pool.size) Spawn(pool);
    }
    
    private void SetPoolDictionary(){
        foreach(Pool pool in pools){
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++) objectPool.Enqueue(CreateGameObject(pool));
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    private GameObject CreateGameObject(Pool pool){
        GameObject npcGameObject = Instantiate(pool.prefab);
        npcGameObject.GetComponent<NPCWorkerAI>().UpdateNPCWorkerAI(pool.tag, GetPath(pool.tag), GetWorkingGameObject(pool.tag));
        npcGameObject.SetActive(false);
        return npcGameObject;
    }
    
    public GameObject GetPath(NPCWorkerAI.Type workerType){
        return workerType switch{
            NPCWorkerAI.Type.Farmer => farmPath,
            NPCWorkerAI.Type.Miner => minePath,
            NPCWorkerAI.Type.Hunter => huntingPath
        };
    }
    
    public GameObject GetWorkingGameObject(NPCWorkerAI.Type workerType){
        return workerType switch{
            NPCWorkerAI.Type.Farmer => farm,
            NPCWorkerAI.Type.Miner => mine,
            NPCWorkerAI.Type.Hunter => hunting
        };
    }
    
    private void Spawn(Pool pool){
        pool.count++;
        coroutine = SpawnNPC(pool.tag, 2f);
        StartCoroutine(coroutine);
    }
    
    private IEnumerator SpawnNPC(NPCWorkerAI.Type tag, float waitTime){
        spawnState.SetSpawnState(isNpcSpawning: true);
        yield return new WaitForSeconds(waitTime);
        SpawnFromPool(tag, this.transform.position, this.transform.rotation);
        spawnState.SetSpawnState(isNpcSpawning: false);
    }
    
    private GameObject SpawnFromPool(NPCWorkerAI.Type tag, Vector3 position, Quaternion rotation){
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
