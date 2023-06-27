using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : MonoBehaviour
{
    
    private class DragonState{
        
        public bool isMoving;
        
        public bool isLocationSet;
        public bool isFlying;
        public bool isTakeOff;
        
        public void SetDragonState(bool isLocationSet = false, bool isFlying = false, bool isTakeOff = false){
            this.isLocationSet = isLocationSet;
            this.isFlying = isFlying;
            this.isTakeOff = isTakeOff;
        }
        
        public void SetDragonStateMoving(){
            this.isMoving = !this.isMoving;
        }
    }
    
    private class DragonAnimatorState{
        
        public bool isFlying;
        public bool isTakeOff;
        public Animator animator;
        public AnimatorStateInfo animatorStateInfo;
        public bool isAnimationChanged;
        public IEnumerator coroutine;
        public float waitTime = 2.5f;
        
        private int randomIndex;
        
        public DragonAnimatorState(Animator animator){
            this.animator = animator;
        }
        
        public void SetDragonAnimatorState(bool isFlying = false, bool isTakeOff = false){
            this.isFlying = isFlying;
            this.isTakeOff = isTakeOff;
            this.UpdateAnimation();
        }
        
        private void UpdateAnimation(){
            if(this.isTakeOff) this.animator.SetBool("runTakeoff", true);
            if(this.isFlying) this.animator.SetBool("flyGlide", true);
        }
        
        public void UpdateRandomFlyAnimation(){
            randomIndex = Random.Range(0, 2);
            if(randomIndex == 0) this.animator.SetBool("flyGlide", true);
            else this.animator.SetBool("flyDeathEnd", true);
        }
        
        public IEnumerator ChangeAnimation(float waitTime){
            this.isAnimationChanged = true;
            yield return new WaitForSeconds(waitTime);
            this.isAnimationChanged = false;
        }
    }
    
    private class DragonPathState{
        
        public List<Transform> dragonLocations;
        public GameObject dragonLocationsParent;
        public Vector3 moveLocation;
        
        private int randomIndex;
        
        public DragonPathState(GameObject dragonLocationsParent){
            this.dragonLocationsParent = dragonLocationsParent;
            this.dragonLocations = new List<Transform>();
            foreach(Transform location in dragonLocationsParent.GetComponentsInChildren<Transform>()) dragonLocations.Add(location);
        }
        
        public void SetDragonPathState(){
            randomIndex = Random.Range(1, 5);
            this.moveLocation = dragonLocations[randomIndex].position;
        }
    }
    
    private class DragonSpeedState{
        
        public float flySpeed = 20f;
        public float rotateSpeed = 5f;
        
        public void SetWorkerSpeedState(float flySpeed = 5f, float rotateSpeed = 5f){
            this.flySpeed = flySpeed;
            this.rotateSpeed = rotateSpeed;
        }
    }
    
    private DragonState dragonState;
    private DragonAnimatorState dragonAnimatorState;
    private DragonPathState dragonPathState;
    private DragonSpeedState dragonSpeedState;
    
    public GameObject dragonPathLocations;
    
    public Animator animator;
    
    public bool moveUpwards;
    
    float timeCounter = 0;
    
    void Start(){
        this.dragonState = new DragonState();
        this.dragonAnimatorState = new DragonAnimatorState(animator);
        this.dragonPathState = new DragonPathState(dragonPathLocations);
        this.dragonSpeedState = new DragonSpeedState();
    }
    
    void Update(){
        if(moveUpwards) MoveUpwards();
        if(!dragonState.isMoving) return;
        if(!dragonState.isLocationSet) SetMoveLocation();
        else if(dragonState.isLocationSet) MoveTowardsLocation();
    }
    
    void OnAnimatorMove(){
        this.dragonAnimatorState.animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if(!this.dragonAnimatorState.animatorStateInfo.IsTag("Fly")){
            this.dragonAnimatorState.animator.ApplyBuiltinRootMotion();
        }
    }
    
    public void SetMoveLocation(){
        this.dragonState.SetDragonState(isLocationSet: true);
        this.dragonPathState.SetDragonPathState();
    }
    
    public void MoveUpwards(){
        moveUpwards = false;
        this.dragonState.SetDragonState(isTakeOff: true);
        this.dragonAnimatorState.SetDragonAnimatorState(isTakeOff: true);
    }
    
    public bool ChangeDragonState(int stateValue){
        return stateValue switch{
            0 => this.UpdateDragonStates(isFlying: true)
        };
    }
    
    public bool UpdateDragonStates(bool isFlying = false, bool isTakeOff = false){
        this.dragonState.SetDragonStateMoving();
        this.dragonState.SetDragonState(isFlying: isFlying, isTakeOff: isTakeOff);
        this.dragonAnimatorState.SetDragonAnimatorState(isFlying: isFlying, isTakeOff: isTakeOff);
        return true;
    }
    
    private void MoveTowardsLocation(){
        float angle = Vector3.Angle(transform.forward, dragonPathState.moveLocation - transform.position);
        Debug.Log(angle);
        transform.position = Vector3.MoveTowards(transform.position, dragonPathState.moveLocation, dragonSpeedState.flySpeed * Time.deltaTime);
        if(!this.dragonAnimatorState.isAnimationChanged) {
            dragonAnimatorState.coroutine = dragonAnimatorState.ChangeAnimation(dragonAnimatorState.waitTime);
            StartCoroutine(dragonAnimatorState.coroutine);
            dragonAnimatorState.UpdateRandomFlyAnimation();
        }
        if(transform.position.x == dragonPathState.moveLocation.x && transform.position.y == dragonPathState.moveLocation.y){
            this.dragonState.SetDragonState(isLocationSet: false);
        }
        LookAtMoveLocation();
    }
    
    private void LookAtMoveLocation(){
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(dragonPathState.moveLocation);
        Quaternion newRotation = transform.rotation;
        transform.rotation = currentRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, dragonSpeedState.rotateSpeed * Time.deltaTime);
    }
}
