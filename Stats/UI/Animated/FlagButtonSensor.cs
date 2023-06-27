using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;

[Obsolete(message:"This is replaced by a simpler solution",error:true)]
class FlagButtonSensor : Sensor
{
    [SerializeField] FlagButton flagButton;
    [SerializeField] int enteredSiblingIndex;
    int originalSiblingIndex;
    
    protected override void Start()
    {
        base.Start();
        originalSiblingIndex = transform.GetSiblingIndex();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        switch (currentState)
        {
            case SensorStates.Outside:
                flagButton.Animate();
                currentState = SensorStates.Inside;
                transform.SetSiblingIndex(enteredSiblingIndex);
                break;
            case SensorStates.InsideOnSomething:
                currentState = SensorStates.Inside;
                break;
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(currentState==SensorStates.Inside)
        {
            Vector2 localMousePosition = flagButton.rect.InverseTransformPoint(Input.mousePosition);

            if (!flagButton.rect.rect.Contains(localMousePosition))
            {
                currentState = SensorStates.Outside;
                transform.SetSiblingIndex(originalSiblingIndex);
                flagButton.StopAnimations();
            }
            else
                currentState = SensorStates.InsideOnSomething;
        }
    }
}
