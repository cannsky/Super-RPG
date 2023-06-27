using UnityEngine;
using UnityEngine.AI;

public class PetAI : MonoBehaviour
{
    public Transform player;
    //Nav Mesh agent for managing NPC
    public NavMeshAgent agent;
    //Animator of NPC
    public Animator animator;
    //Layer's to detect the ground and the player
    public LayerMask whatIsGround, whatIsPlayer;
    //Walk point of the NPC
    public Vector3 walkPoint;
    //Walk point range
    public float sightRange, seatRange, walkRange;
    //Waiting Time
    public float waitingTime;
    //Waiting status
    public bool isFollowingPlayer = false;
    //Destination status
    public bool destinationSet;
    public bool playerInSightRange, playerInSeatRange, playerInWalkRange;
    //Last walk point
    Vector3 distanceBetweenLastPoint;
    //Checks the distance between two points
    Vector3 distanceToWalkPoint;
    //Last position of the npc
    Vector3 lastPosition;
    //Last Rotation Y
    float lastRotationY;
    //Vector3
    float rotationAngle;
    
    bool isRotated = false;
    
    public bool isUpdated;

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInSeatRange = Physics.CheckSphere(transform.position, seatRange, whatIsPlayer);
        playerInWalkRange = Physics.CheckSphere(transform.position, walkRange, whatIsPlayer);
        if (playerInSeatRange) Seat();
        else if (playerInSightRange || playerInWalkRange) Stand();
    }

    void Seat()
    {
        isUpdated = false;
        isFollowingPlayer = false;
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Turn Walk", false);
        animator.SetBool("Turn Walk Left", false);
        animator.SetBool("Turn Walk Right", false);
        animator.SetBool("Turn Run", false);
        agent.SetDestination(transform.position);
        animator.SetBool("Seat", true);
    }
    
    private void Stand(){
        if (isFollowingPlayer) FollowPlayer();
        else if (!isUpdated) {
            isUpdated = true;
            animator.SetBool("Seat", false);
            Invoke(nameof(SetFollowingPlayerTrue), 2);
        }
    }

    private void FollowPlayer()
    {
        rotationAngle = lastRotationY - transform.eulerAngles.y;
        if(Mathf.Abs(rotationAngle) > 0.1){
            animator.SetBool("Run", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Turn Walk", true);
            agent.speed = 1;
            rotationAngle = lastRotationY - transform.eulerAngles.y;
            if(!isRotated){
                isRotated = true;
                if(rotationAngle > 0){
                    animator.SetBool("Turn Walk Right", true);
                    animator.SetBool("Turn Walk Left", false);
                }
                else{
                    animator.SetBool("Turn Walk Left", true);
                    animator.SetBool("Turn Walk Right", false);
                }
            }
            
        }
        else if(lastRotationY == 0){
            //Do nothing
        }
        else{
            animator.SetBool("Turn Walk", false);
            animator.SetBool("Turn Walk Right", false);
            animator.SetBool("Turn Walk Left", false);
            animator.SetBool("Turn Run", false);
            animator.SetBool("Walk", true);
            if(playerInWalkRange) {
                Invoke(nameof(SetIsRotatedFalse), 10);
                agent.speed = 2;
                animator.SetBool("Run", false);
            }
            else if(playerInSightRange) {
                agent.speed = 12;
                animator.SetBool("Run", true);
            }
        }
        lastRotationY = transform.eulerAngles.y;
        agent.SetDestination(player.position);
    }
    
    private void SetIsRotatedFalse(){
        isRotated = false;
    }
    
    private void SetFollowingPlayerTrue(){
        isFollowingPlayer = true;
    }
}
