using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    /*
        Important Notes,
        1) Before using the player movement, make sure that the player has a character controller and there is cinemachine in the unity project.
        2) mainCamera.eulerAngles.y and MoveDirection makes camera to rotate as the player rotates.
    */
    
    //Objects from Unity
    
    public List<Transform> swordEffects = new List<Transform>();
    
    private PlayerAttack playerAttack;
    private PlayerMovement playerMovement;
    
    public int level;
    public int health;
    public int gold = 100;
    public bool debugMode = false;
    public float transformY;
    
    //Player Animator
    public Animator animator;
    //Character Controller
    public CharacterController controller;
    //Main Camera in the scene
    public Transform mainCamera;

    //Character Interaction with NPC

    //Npc game object
    public static GameObject npc;
    //Distance between npc and player
    Vector3 distanceWithNPC;

    //Character Interaction with Objects

    //Triggered game object
    public static GameObject triggeredGameObject;
    //Distance between triggered object and player
    Vector3 distanceWithInteractable;

    //Player Quest
    public PlayerQuest playerQuest;
    //Player Quest Goal
    public static int questGoal;
    
    public List<Quest> sideQuests = new List<Quest>();
    public List<int> sideQuestsGoals = new List<int>();
    
    void Start(){
        if(debugMode) LoadPlayerData();
        this.playerAttack = gameObject.GetComponent<PlayerAttack>();
        this.playerMovement = gameObject.GetComponent<PlayerMovement>();
    }
    
    //Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transformY, transform.position.z);
        //If inventory is opened, player cannot move
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        //Get horizontal and vertical variables
        playerMovement.horizontal = Input.GetAxisRaw("Horizontal");
        playerMovement.vertical = Input.GetAxisRaw("Vertical");

        if(triggeredGameObject != null) {
            //If there is an interactable object near, calculate the distance between object and player
            distanceWithInteractable = transform.position - triggeredGameObject.transform.position;
            //if the distance is higher than 5f, set triggered game object to null
            if (distanceWithInteractable.magnitude >= 5f) triggeredGameObject = null;
            //Interact with an object, pick up the item
            if (Input.GetKey(KeyCode.E))
            {
                triggeredGameObject.GetComponent<ItemPickup>().PickUp();
                triggeredGameObject = null;
            }
        }

        if(npc != null)
        {
            //If there is an npc near, calculate the distance between npc and player
            distanceWithNPC = transform.position - npc.transform.position;
            //if the distance is higher than 5f, set npc game object to null
            if (distanceWithNPC.magnitude >= 5f) npc = null;
            //Open the dialogue 
            if (Input.GetKey(KeyCode.E))
            {
                npc.GetComponent<DialogueTrigger>().TriggerDialogue();
                npc = null;
                SaveSystem.SavePlayer(this);
            }
        }

        if(playerQuest.isActive == true)
        {
            if(true)
            {
                //Debug.Log("walk");
                
            }
        }

        //Update the animator's walk/run animation according to the vertical and horizontal values
        if(!playerAttack.isAttackAnimationPlaying) playerMovement.UpdatePlayerAnimation();

        //Update the direction with horizontal and vertical variables
        playerMovement.direction = new Vector3(playerMovement.horizontal, 0f, playerMovement.vertical);
        
        //If the left mouse button clicked, attack
        if (Input.GetMouseButtonDown(0) && playerAttack.attackQueue.Count < 3 && playerMovement.direction.magnitude == 0) playerAttack.Attack();
        
        //If player is jumping update the velocity
        if (Input.GetButtonDown("Jump")) playerMovement.rolling = true;
        
        if(!playerAttack.isAttackAnimationPlaying && playerAttack.attackQueue.Count != 0 && playerMovement.direction.magnitude == 0) playerAttack.AttackAnimation();

        //If player is moving update the rotation and location of the player
        if (playerMovement.direction.magnitude >= 0.1f && !playerAttack.isAttackAnimationPlaying) playerMovement.MovePlayer();
        
        if(playerMovement.isMoving) playerAttack.attackQueue.Clear();
    }
    
    void LoadPlayerData(){
        PlayerData playerData = LoadSystem.LoadPlayer();
        
        if(playerData == null) return;
        
        this.level = playerData.level;
        this.health = playerData.health;
        
        Vector3 position;
        position.x = playerData.position[0];
        position.y = playerData.position[1];
        position.z = playerData.position[2];
        transform.position = position;
        
        this.playerQuest.isActive = playerData.isQuestActive;
        this.playerQuest.questID = playerData.questID;
        this.playerQuest.questTitle = playerData.questTitle;
        this.playerQuest.questDescription = playerData.questDescription;
        this.playerQuest.experienceReward = playerData.questExperienceReward;
        this.playerQuest.goldReward = playerData.questGoldReward;

        this.ConvertIntegerToQuestGoal(playerData.questGoal);
    }

    void ConvertIntegerToQuestGoal(int goal)
    {
        switch (goal)
        {
            case 1:
                this.playerQuest.goal.goalType = GoalType.Walk;
                break;
            default:
                Debug.Log("Quest Goal is not assigned.");
                break;
        }
    }

    public static int ConvertQuestGoalToInteger(GoalType goal)
    {
        switch (goal)
        {
            case GoalType.Walk:
                return 1;
            default:
                Debug.Log("Quest Goal is not assigned.");
                return 0;
        }
    }
    
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag == "Interactable") triggeredGameObject = collider.gameObject;
        else if (collider.gameObject.tag == "NPC") npc = collider.gameObject;
    }
}
