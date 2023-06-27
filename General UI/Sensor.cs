using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CanvasGroup))]
abstract class Sensor : UIToggle,IPointerEnterHandler,IPointerExitHandler
{
    CanvasGroup group;
    protected SensorStates currentState = SensorStates.Outside;

    protected enum SensorStates
    {
        Outside,
        Inside,
        InsideOnSomething
    }
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
    protected virtual void Start()
    {
        group = GetComponent<CanvasGroup>();
    }
}
