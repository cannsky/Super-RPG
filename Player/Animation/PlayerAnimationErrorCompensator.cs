using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationErrorCompensator : AnimationErrorCompensator
{
    CharacterController controller;

    Vector3 footRotatePoint;
    Ray ray = new Ray(Vector3.zero,Vector3.down);
    RaycastHit rightFrontHit = new RaycastHit();
    RaycastHit rightMidHit = new RaycastHit();
    RaycastHit rightBackHit = new RaycastHit();
    RaycastHit leftFrontHit = new RaycastHit();
    RaycastHit leftMidHit = new RaycastHit();
    RaycastHit leftBackHit = new RaycastHit();
    int mask;
    static PlayerAnimationErrorCompensator instance;
    public static PlayerAnimationErrorCompensator Instance { get => instance; }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this, 0.01f);
    }

    protected override void Start()
    {
        base.Start();
        controller = Player.Instance.GetComponent<CharacterController>();
        mask = LayerMask.GetMask("Terrain");
        action = () =>
        {
            if (GetRootMotionAvailable())
            {
                Vector3 oldPos = transform.parent.position;
                Vector3 estimatedPos = oldPos + anim.deltaPosition;
                anim.applyRootMotion = true;
                controller.SimpleMove(anim.deltaPosition * (1f / Time.deltaTime));
            }
            else
            {
                anim.applyRootMotion = false;
            }
        };
        idleAction = () =>
        {
            //ray.origin = rightFootFront.transform.position;
            //ray.origin = leftFootBack.transform.position;
            
            ////DebugDraw.DrawVector(rightFootFront.transform.position, Vector3.down, 10f, 1f, Color.red, 10f);
            ////DebugDraw.DrawVector(rightFootBack.transform.position, Vector3.down, 10f, 1f, Color.red, 10f);
            ////DebugDraw.DrawVector(leftFootFront.transform.position, Vector3.down, 10f, 1f, Color.red, 10f);
            ////DebugDraw.DrawVector(leftFootBack.transform.position, Vector3.down, 10f, 1f, Color.red, 10f);

            //bool Raycast(Vector3 point, out RaycastHit rayHit)
            //{
            //    ray.origin = point;
            //    return Physics.Raycast(ray, out rayHit, 10f, mask);
            //}

            //bool forwardHit = Raycast(rightFootFront.transform.position, out rightFrontHit) && Raycast(rightFootBack.transform.position, out rightBackHit)
            //&& Raycast(leftFootFront.transform.position, out leftFrontHit) && Raycast(leftFootBack.transform.position, out leftBackHit);

            //if (forwardHit)
            //{
            //    Vector3 rightIncVector = rightFrontHit.point - rightBackHit.point;
            //    Vector3 leftIncVector = leftFrontHit.point - leftBackHit.point;
            //    Vector3 avgIncVector = ((rightIncVector + leftIncVector) / 2f).normalized;
            //    Vector3 axis = anim.deltaPosition.x >= 0 ? Vector3.up : Vector3.down;

            //    float angleToRotate = 90f - Mathf.Acos(Vector3.Dot(axis, avgIncVector)) * Mathf.Rad2Deg;
            //    float angle = Mathf.Atan2(PlayerMovement.direction.x, PlayerMovement.direction.z)*Mathf.Rad2Deg;
            //    transform.parent.Rotate(new Vector3(angleToRotate * angle/90f, 0, 0));

            //    Vector3 backHorizontalVector = rightBackHit.point - leftBackHit.point;
            //    Vector3 midHorizontalVector = rightMidHit.point - leftMidHit.point;
            //    Vector3 frontHorizontalVector = rightFrontHit.point - leftFrontHit.point;
            //    Vector3 avgHorizontalVector = (backHorizontalVector + midHorizontalVector + frontHorizontalVector) / 3f;
            //    axis = anim.deltaPosition.z > 0 ? Vector3.right : Vector3.left;

            //    angleToRotate = 90f - Mathf.Acos(Vector3.Dot(axis, avgHorizontalVector)) * Mathf.Rad2Deg;
            //    transform.parent.Rotate(new Vector3(0, 0, angleToRotate*(90-angle)/90f));
            //}
        };
    }


    //void x()
    //{
    //    Vector3 oldPos = transform.parent.position;
    //    Vector3 estimatedPos = oldPos + anim.deltaPosition;
    //    controller.SimpleMove(anim.deltaPosition * (1f / Time.deltaTime));
    //    if ((transform.parent.position - estimatedPos).magnitude > toleratedError)
    //    {
    //        anim.applyRootMotion = false;
    //        transform.parent.position = oldPos;
    //        Debug.Log("revert required");
    //    }
    //    else
    //    {
    //        anim.applyRootMotion = true;
    //        Debug.Log("animation successful");
    //    }
    //}
    bool GetRootMotionAvailable()
    {
        return !(anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement") ||
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement2") ||
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement3"));
    }
}
