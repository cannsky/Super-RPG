using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSailorAI : MonoBehaviour
{
    
    public GameObject sailorPath;
    
    private SailorState sailorState;
    private SailorSpeedState sailorSpeedState;
    private SailorPathState sailorPathState;
    
    private class SailorState{
        
        public bool isLocationSet;
        
        public void SetSailorState(bool isLocationSet = false){
            this.isLocationSet = isLocationSet;
        }
        
    }
    
    private class SailorSpeedState{
        
        public float walkSpeed = 25f;
        public float rotateSpeed = 5f;
        
        public void SetSailorSpeedState(float walkSpeed = 5f, float rotateSpeed = 5f){
            this.walkSpeed = walkSpeed;
            this.rotateSpeed = rotateSpeed;
        }
        
    }
    
    private class SailorPathState{
        
        public Vector3 moveLocation;
        public GameObject sailorPath;
        public List <GameObject> sailorPaths;
        
        private int index;
        private int count;
        
        public SailorPathState(GameObject sailorPath){
            this.sailorPath = sailorPath;
            this.sailorPaths = new List<GameObject>();
            foreach(Transform path in sailorPath.GetComponentsInChildren<Transform>()) sailorPaths.Add(path.gameObject);
            this.count = sailorPaths.Count;
        }
        
        public void UpdateSailorPathState(){
            if(index == count) index = 0;
            this.moveLocation = sailorPaths[index++].transform.position;
        }
    }
    
    void Awake(){
        this.sailorState = new SailorState();
        this.sailorSpeedState = new SailorSpeedState();
        this.sailorPathState = new SailorPathState(this.sailorPath);
    }
    
    void Update(){
        if(!sailorState.isLocationSet) SetMoveLocation();
        else MoveTowardsLocation();
    }
    
    private void SetMoveLocation(){
        sailorState.SetSailorState(isLocationSet: true);
        sailorPathState.UpdateSailorPathState();
    }
    
    private void MoveTowardsLocation(){
        transform.position = Vector3.MoveTowards(transform.position, sailorPathState.moveLocation, sailorSpeedState.walkSpeed * Time.deltaTime);
        LookAtMoveLocation();
        if(transform.position.x == sailorPathState.moveLocation.x && transform.position.y == sailorPathState.moveLocation.y) sailorState.isLocationSet = false;
    }
    
    private void LookAtMoveLocation(){
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(sailorPathState.moveLocation);
        Quaternion newRotation = transform.rotation;
        transform.rotation = currentRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, sailorSpeedState.rotateSpeed * Time.deltaTime);
    }
}
