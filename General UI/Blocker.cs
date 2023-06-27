using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Blocker : MonoBehaviour
{
    public enum BlockerType
    {
        Top,
        Bottom,
        Right,
        Left
    }

    BlockerType type;

    private void Start()
    {
        if (gameObject.name=="Right Blocker")
            type = BlockerType.Right;
        else if (gameObject.name=="Left Blocker")
            type = BlockerType.Left;
        else if (gameObject.name=="Top Blocker")
            type = BlockerType.Top;
        else
            type = BlockerType.Bottom;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerStay2D(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        UIDragger dragger;
        if (GetDragger(collision, out dragger))
            return;
        dragger.RevertDrag(type);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        UIDragger dragger;
        if(GetDragger(collision,out dragger))
        {
            var rect = dragger.target.outestCanvas.GetComponent<RectTransform>().rect;
            var menuSize = dragger.target.GetSize();
            var targetPos = dragger.target.GetLocalPosition();
            Vector3 directionVector = new Vector3();
            directionVector.z = 0;

            switch (type)
            {
                case BlockerType.Top:
                    directionVector.x = targetPos.x;
                    directionVector.y = rect.height / 2f - menuSize.y / 2f;
                    break;
                case BlockerType.Bottom:
                    directionVector.x = targetPos.x;
                    directionVector.y = -rect.height / 2f - menuSize.y / 2f;
                    break;
                case BlockerType.Right:
                    directionVector.y = targetPos.y;
                    directionVector.x = rect.width / 2f - menuSize.x / 2f;
                    break;
                case BlockerType.Left:
                    directionVector.y = targetPos.y;
                    directionVector.x = -rect.width / 2f - menuSize.x / 2f;
                    break;
            }

            dragger.target.SetLocalPosition(directionVector);
        }
    }

    bool GetDragger(Collider2D collider,out UIDragger dragger)
    {
        bool isNull = false;
        if (isNull = !(dragger = collider.GetComponent<UIDragger>()))
            isNull = !(dragger = collider.GetComponentInChildren<UIDragger>());

        return isNull;
    }
}
