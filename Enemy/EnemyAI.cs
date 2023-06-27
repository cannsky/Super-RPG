using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
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
    public LayerMask whatIsPlayer;
    //Walk point of the enemy while patrolling
    private Vector3 walkPoint;
    //Walk point set or not
    private bool walkPointSet;
    //Walk point range
    public float walkPointRange;
    //Time between attacks of the enemy
    public float timeBetweenAttacks;
    //Sets if enemy attacked or not
    bool alreadyAttacked;
    //Sight and attack range for the enemy
    public float sightRange, attackRange;
    //Player is in sight and attack range or not
    private bool playerInSightRange, playerInAttackRange;
    //Last walk point
    private Vector3 distanceBetweenLastPoint;
    //Checks the distance between two points
    private Vector3 distanceToWalkPoint;
    //Enemy's waiting status in a place while patrolling
    private bool waitingStatus;
    //Enemy's tolerance when path is blocked
    private int counter;
    //Random Number
    private int randomNumber, lastRandomNumber;
    
    void Start(){
        player = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        //Checks if the player is in sight, attack range or not
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //If player is not in sight or attack range patrol
        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        //If player is in sight range chase player
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        //If player is in attack range attack to player
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    void Patrolling()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        transform.Rotate(0, (transform.position.y - walkPoint.y) * Time.deltaTime, 0);
        //If patrolling, the enemy will walk
        animator.SetBool("Run", false);
        animator.SetBool("Walk", true);
        //If walk point is not set, search new walk point
        if(!walkPointSet) SearchWalkPoint();
        //If there is walk point, move to the walk point
        if(walkPointSet && !waitingStatus) agent.SetDestination(walkPoint);
        //Set walk point if enemy is not waiting
        if(waitingStatus) agent.SetDestination(transform.position);
        //Set last walk point to the distanceBetweenLastPoint
        distanceBetweenLastPoint = distanceToWalkPoint;
        //Calculate the distance between current and last points
        distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        if(!waitingStatus && distanceToWalkPoint.magnitude == distanceBetweenLastPoint.magnitude) if(counter++ > 10) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Create a random z between walk point range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        //Create a random y between walk point range
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        //Create a new walk point
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //Set walk point bool to true
        walkPointSet = true;
    }

    void ChasePlayer()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        //Set run to true when chasing the player
        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        //Set destination to the player's poisiton
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        //Set run to false
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        //Set destination to the current location
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //Set rotation to the player
        transform.Rotate(0, (transform.position.y - player.position.y) * Time.deltaTime, 0);
        //If not attacked
        if (!alreadyAttacked)
        {
            //Create a random number between 1 and 4: 1, 2, 3.
            randomNumber = Random.Range(1, 10);
            randomNumber = (randomNumber < 5) ? 5 : Random.Range(1, 5);
            //If the last random number and the generated random number are same, change the random number
            randomNumber = (lastRandomNumber != randomNumber) ? randomNumber : Random.Range(1, 5);
            //Set already attacked to true
            alreadyAttacked = true;
            //Animation of the enemy to attack
            animator.SetBool("Attack " + randomNumber, true);
            //Reset attack after given time
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        //Set already attacked to false
        alreadyAttacked = false;
    }

}
