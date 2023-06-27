using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    /* 
     * Important Notes: Make sure that nav mesh surface which is on a game object is baked and navmesh agent is added to the enemy.
     */

    //Nav Mesh agent for managing enemy
    public NavMeshAgent agent;
    //Player's object
    public Transform player;
    //Animator of enemy
    public Animator animator;
    //Layer's to detect the ground and the player
    public LayerMask whatIsGround, whatIsPlayer;
    //Walk point of the enemy while patrolling
    public Vector3 walkPoint;
    //Walk point set or not
    public bool walkPointSet;
    //Walk point range
    public float walkPointRange;
    //Time between attacks of the enemy
    public float timeBetweenAttacks;
    //Sets if enemy attacked or not
    bool alreadyAttacked;
    //Sight and attack range for the enemy
    public float sightRange, attackRange;
    //Player is in sight and attack range or not
    public bool playerInSightRange, playerInAttackRange;
    //Last walk point
    Vector3 distanceBetweenLastPoint;
    //Checks the distance between two points
    Vector3 distanceToWalkPoint;
    
    public bool waitingStatus;
    
    public int counter = 0;

    void Update()
    {
        if(!waitingStatus) Patrolling();
    }

    void Patrolling()
    {
        animator.SetBool("Walk", true);
        //Looks to the rotation the enemy walks
        transform.Rotate(0, (transform.position.y - walkPoint.y) * Time.deltaTime, 0);
        //If walk point is not set, search new walk point
        if (!walkPointSet) SearchWalkPoint();
        //If there is walk point, move to the walk point
        if (walkPointSet && !waitingStatus) agent.SetDestination(walkPoint);
        if (waitingStatus) agent.SetDestination(transform.position);
        //Set last walk point to the distanceBetweenLastPoint
        distanceBetweenLastPoint = distanceToWalkPoint;
        //Calculate the distance between current and last points
        distanceToWalkPoint = transform.position - walkPoint;
        //If magnitude of distance is lower than 1 a new walk point needed
        if(distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        if(!waitingStatus && distanceToWalkPoint.magnitude == distanceBetweenLastPoint.magnitude) if(counter++ > 10) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        SetWaitingStatus();
        //Create a random z between walk point range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        //Create a random y between walk point range
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        //Create a new walk point
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //Set walk point bool to true
        walkPointSet = true;
    }
    
    public void SetWaitingStatus(){
        waitingStatus = true;
        animator.SetBool("Walk", false);
        Invoke(nameof(DisableWaitingStatus), Random.Range(1, 5));
    }
    
    public void DisableWaitingStatus(){
        counter = 0;
        waitingStatus = false;
    }

}
