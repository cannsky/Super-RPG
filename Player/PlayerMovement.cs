using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    
    //Horizontal and vertical values
    public float horizontal, vertical;
    //Speed of the Character Movements
    public float speed = 12f;
    //Rolling value
    public bool rolling = false;
    //Rolling start point
    public Vector3 rollingStartPoint;
    //Is player moving
    public bool isMoving = false;
    
    //Angle for updating the character to the direction the player moves
    float targetAngle;
    //New angle after updating targetAngle
    public float angle;
    //Used for transforming the character in the way the player moves
    public float turnSmoothTime = 0.1f;
    //Used with turnSmoothTime
    float turnSmoothVelocity;
    //Direction of the player
    public Vector3 direction;
    //New direction after updating the direction
    Vector3 moveDirection;
    
    void Start(){
        this.player = gameObject.GetComponent<Player>();
    }
    
    public void UpdatePlayerAnimation()
    {
        if (rolling)
        {
            isMoving = true;
            horizontal += (horizontal != 0) ? 5 * horizontal : 0;
            vertical += (vertical != 0) ? 5 * vertical : 0;
            player.animator.SetBool("Roll Forward", true);
            player.animator.SetBool("Run", false);
        }
        else if ((vertical != 0 || horizontal != 0))
        {
            isMoving = true;
            player.animator.SetBool("Roll Forward", false);
            player.animator.SetBool("Run", true);
        }
        else
        {
            isMoving = false;
            player.animator.SetBool("Roll Forward", false);
            player.animator.SetBool("Run", false);
        }
    }
    
    public void MovePlayer(){
        //Target angle finds the rotation of the player after moving
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + player.mainCamera.eulerAngles.y;
        
        //Angle is the updated target angle that is changed by the time which makes the rotation smoother
        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        
        //Rotate the player with the angle
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        //Rotates the camera as the player rotates
        moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        
        //Move the character
        player.controller.Move(moveDirection * speed * Time.deltaTime);
    }
}
