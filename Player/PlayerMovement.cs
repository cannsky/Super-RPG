using System;
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //Serialized Fields
    [NonSerialized] public CharacterController controller;
    [SerializeField] Transform mainCamera;
    [SerializeField] float turnSmoothTime = 0.1f;
    [NonSerialized] public bool isRotating;

    public float joggingSpeed = 6f;
    public float runningSpeed = 8f;
    public float runningFastSpeed = 12f;

    //Cached Fields
    PlayerStateController stateController;
    Player player;
    PlayerAttack playerAttack;
    float targetAngle;  
    float angle;  
    float turnSmoothVelocity;
    public static Vector3 direction;  
    Vector3 moveDirection;  
    float horizontal, vertical;
    static PlayerMovement instance;
    public static PlayerMovement Instance { get => instance; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }
    void Start()
    {
        stateController = PlayerStateController.Instance;
        player = Player.Instance;
        playerAttack = PlayerAttack.Instance;
        controller = GetComponent<UnityEngine.CharacterController>();
    }

    void Update()
    {
        if (isRotating) RotateTowardsEnemy();
    }

    public void TryMoving()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
        direction = new Vector3(horizontal, 0, vertical);

        if (direction.magnitude >= 0.1f && !isRotating)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            MovePlayer(stateController.UpdateMovementInfo(true, vertical > 0));
            player.autoChase = false;
            playerAttack.remainingQueueAttacks = 0;
        }
        //else if(
        //    stateController.UpdateMovementInfo(false);
    }

    public void AutoChase(Vector3 targetPosition)
    {
        direction = Helper.FindDirectionVectorWithouty(transform.position, targetPosition);
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        MovePlayer(stateController.UpdateMovementInfo(true, vertical > 0));

        if (Helper.CalculateDistance(transform.position, targetPosition) + Mathf.Epsilon <= player.interactionDistance)
            player.autoChase = false;
    }

    void MovePlayer(float speed)
    {
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        moveDirection = Quaternion.Euler(0f, targetAngle, 0f)*Vector3.forward;
        controller.SimpleMove(moveDirection * speed);
    }

    public void RotateTowardsEnemy()
    {
        if (!player.targetStats)
        {
            isRotating = false;
            return;
        }
        Quaternion originalRotation = transform.rotation;
        transform.LookAt(player.targetStats.transform);
        Quaternion newRotation = transform.rotation;
        transform.rotation = originalRotation;
        if (isRotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 16f * Time.deltaTime);
        }
        if ((transform.rotation.eulerAngles-newRotation.eulerAngles).magnitude<1f)
        {
            isRotating = false;
        }
    }

    public IEnumerator Rotate()
    {
        if (!player.targetStats || isRotating) yield return null;
        else yield return new WaitUntil(() => 
            {
                isRotating = true;
                Quaternion originalRotation = transform.rotation;
                transform.LookAt(player.targetStats.transform);
                Quaternion newRotation = transform.rotation;
                transform.rotation = originalRotation;
                Quaternion.Lerp(transform.rotation, newRotation, 0.8f * Time.deltaTime);
                return (transform.rotation.eulerAngles - newRotation.eulerAngles).magnitude > 5f;
            });
        isRotating = false;
    }
}