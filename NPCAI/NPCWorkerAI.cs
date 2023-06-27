using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWorkerAI : MonoBehaviour
{
    public enum Type { Farmer, Miner, Hunter };
    public Type workerType;
    
    private WorkerState workerState;
    private WorkerSpeedState workerSpeedState;
    private WorkerPathState workerPathState;
    private WorkerProductionState workerProductionState;
    private WorkerAnimatorState workerAnimatorState;
    
    private class WorkerState{
        
        public bool isLocationSet;
        public bool isReadyForWork;
        public bool isWorking;
        public bool isSellingGoods;
        public bool isCollectingResources;
        public bool isTalking;
        
        public void SetWorkerState(bool isLocationSet = false, bool isReadyForWork = false, bool isWorking = false, bool isSellingGoods = false, bool isCollectingResources = false, bool isTalking = false){
            this.isLocationSet = isLocationSet;
            this.isReadyForWork = isReadyForWork;
            this.isWorking = isWorking;
            this.isSellingGoods = isSellingGoods;
            this.isCollectingResources = isCollectingResources;
            this.isTalking = isTalking;
        }
    }
    
    private class WorkerSpeedState{
        
        public float walkSpeed = 25f;
        public float rotateSpeed = 5f;
        
        public void SetWorkerSpeedState(float walkSpeed = 5f, float rotateSpeed = 5f){
            this.walkSpeed = walkSpeed;
            this.rotateSpeed = rotateSpeed;
        }
        
    }
    
    private class WorkerPathState{
        
        public Vector3 moveLocation;
        public GameObject workerPath;
        public List <GameObject> workerPaths;
        
        public bool isWorkingPathReached;
        public bool isVillagePathReached;
        
        private int index;
        private int count;
        
        public void SetWorkerPathState(GameObject workerPath){
            this.workerPath = workerPath;
            this.workerPaths = new List<GameObject>();
            foreach(Transform path in workerPath.GetComponentsInChildren<Transform>()) workerPaths.Add(path.gameObject);
            this.count = workerPaths.Count;
        }
        
        public void SetWorkerPathState(bool isWorkingPathReached = false, bool isVillagePathReached = false){
            this.isWorkingPathReached = isWorkingPathReached;
            this.isVillagePathReached = isVillagePathReached;
        }
        
        public void UpdateWorkerPathState(){
            if(index == count) index = 0;
            if(workerPaths[index].tag == "WorkingLocation") this.isWorkingPathReached = true;
            if(workerPaths[index].tag == "VillageLocation") this.isVillagePathReached = true;
            this.moveLocation = workerPaths[index++].transform.position;
        }
    }
    
    private class WorkerProductionState{
        
        public GameObject workingGameObject;
        
        public Merchant merchant;
        
        public Product workerProduct;
        
        public List<Item> workerInventory;
        
        public WorkerProductionState(){
            this.workerInventory = new List<Item>();
            this.merchant = GameObject.Find("Merchant").GetComponent<Merchant>();
        }
        
        public void SetWorkerProductionState(GameObject workingGameObject){
            this.workingGameObject = workingGameObject;
        }
        
        public void SetWorkerProductionState(Product workerProduct){
            this.workerProduct = workerProduct;
        }
        
        public void UpdateWorkerInventory(){
            this.workerInventory.Add(workerProduct.CreateProduct());
        }
        
        public void ClearWorkerInventory(){
            this.workerInventory.Clear();
        }
    }
    
    private class WorkerAnimatorState{
        
        public Animator animator;
        
        public bool isWalking;
        public bool isWorking;
        public bool isTalking;
        
        public NPCWorkerAI worker;
        
        public WorkerAnimatorState(NPCWorkerAI worker, Animator animator){
            this.worker = worker;
            this.animator = animator;
        }
        
        public void SetWorkerAnimatorState(bool isWalking = false, bool isWorking = false, bool isTalking = false){
            this.isWalking = isWalking;
            this.isWorking = isWorking;
            this.isTalking = isTalking;
            this.UpdateAnimator();
        }
        
        private void UpdateAnimator(){
            if(this.isWalking) this.animator.SetBool("Walk", true);
            else this.animator.SetBool("Walk", false);
            if(this.isWorking) this.WorkAnimation();
            else this.animator.SetBool("Working", false);
            if(this.isTalking) this.animator.SetBool("Talk", true);
            else this.animator.SetBool("Talk", false);
        }
        
        private void WorkAnimation(){
            this.animator.SetBool("Working", true);
            if(worker.workerType == Type.Farmer) this.animator.SetBool("Farmer", true);
            else this.animator.SetBool("Farmer", false);
            if(worker.workerType == Type.Miner) this.animator.SetBool("Miner", true);
            else this.animator.SetBool("Miner", false);
            if(worker.workerType == Type.Hunter) this.animator.SetBool("Hunter", true);
            else this.animator.SetBool("Hunter", false);
        }
    }
    
    void Awake(){
        workerState = new WorkerState();
        workerSpeedState = new WorkerSpeedState();
        workerPathState = new WorkerPathState();
        workerProductionState = new WorkerProductionState();
        workerAnimatorState = new WorkerAnimatorState(this, gameObject.GetComponent<Animator>());
    }
    
    // Update is called once per frame
    void Update()
    {
        if(workerState.isLocationSet) MoveTowardsLocation();
        else if(workerState.isReadyForWork) GetProductLocation();
        else if(!workerState.isWorking && !workerState.isSellingGoods) SetMoveLocation();
    }
    
    public void UpdateNPCWorkerAI(NPCWorkerAI.Type workerType, GameObject workerPath, GameObject workingGameObject){
        this.workerType = workerType;
        workerPathState.SetWorkerPathState(workerPath);
        workerProductionState.SetWorkerProductionState(workingGameObject);
    }
    
    private void SetMoveLocation(){
        workerState.SetWorkerState(isLocationSet: true);
        workerPathState.UpdateWorkerPathState();
    }
    
    private void GatherResources(){
        IEnumerator coroutine = CollectResources(10f);
        workerAnimatorState.SetWorkerAnimatorState(isWorking: true);
        workerState.SetWorkerState(isWorking: true, isCollectingResources: true);
        StartCoroutine(coroutine);
    }
    
    private IEnumerator CollectResources(float waitTime){
        yield return new WaitForSeconds(waitTime);
        workerProductionState.UpdateWorkerInventory();
        workerProductionState.workerProduct.isNpcWorking = false;
        workerState.SetWorkerState(isLocationSet: true);
        workerAnimatorState.SetWorkerAnimatorState(isWalking: true);
    }
    
    private void GetProductLocation(){
        workerState.SetWorkerState(isWorking: true, isLocationSet: true);
        workerProductionState.workerProduct = workerProductionState.workingGameObject.GetComponent<WorkingGameObject>().AttachNPCToGameObject();
        workerPathState.moveLocation = workerProductionState.workerProduct.transform.position;
    }
    
    private void GetMerchantLocation(){
        workerState.SetWorkerState(isLocationSet: true, isSellingGoods: true);
        workerPathState.SetWorkerPathState(isVillagePathReached: false);
        workerPathState.moveLocation = workerProductionState.merchant.npcTalkLocation.position;
    }
    
    private void GotoMerchant(){
        workerState.SetWorkerState(isSellingGoods: true, isTalking: true);
        IEnumerator coroutine = SellGoodsToMerchant(5f);
        workerAnimatorState.SetWorkerAnimatorState(isTalking: true);
        workerProductionState.merchant.GetMerchantAnimatorState().SetMerchantAnimatorState(isTalking: true);
        workerProductionState.merchant.SellItems(workerProductionState.workerInventory);
        workerProductionState.ClearWorkerInventory();
        StartCoroutine(coroutine);
    }
    
    private IEnumerator SellGoodsToMerchant(float waitTime){
        yield return new WaitForSeconds(waitTime);
        workerProductionState.workerProduct.isNpcWorking = false;
        workerState.SetWorkerState(isLocationSet: true);
        workerAnimatorState.SetWorkerAnimatorState(isWalking: true);
    }
    
    private void MoveTowardsLocation(){
        workerAnimatorState.SetWorkerAnimatorState(isWalking: true);
        transform.position = Vector3.MoveTowards(transform.position, workerPathState.moveLocation, workerSpeedState.walkSpeed * Time.deltaTime);
        LookAtMoveLocation();
        if(transform.position.x == workerPathState.moveLocation.x && transform.position.y == workerPathState.moveLocation.y){
            if(workerPathState.isVillagePathReached) GetMerchantLocation();
            else if(workerState.isSellingGoods && !workerState.isTalking) GotoMerchant(); 
            else if(workerState.isWorking && !workerState.isCollectingResources) GatherResources();
            else if(!workerPathState.isWorkingPathReached) SetMoveLocation();
            else if(!workerState.isCollectingResources && !workerState.isTalking){
                workerState.SetWorkerState(isReadyForWork: true);
                workerPathState.SetWorkerPathState(isWorkingPathReached: false);
            }
        }
    }
    
    private void LookAtMoveLocation(){
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(workerPathState.moveLocation);
        Quaternion newRotation = transform.rotation;
        transform.rotation = currentRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, workerSpeedState.rotateSpeed * Time.deltaTime);
    }
}
