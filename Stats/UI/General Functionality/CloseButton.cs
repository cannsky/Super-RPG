using UnityEngine;
using UnityEngine.EventSystems;
public class CloseButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] float enteredScale = 1.25f;
    RectTransform rect;
    Vector3 enteredScaleVector;
    Vector3 normalScaleVector;
    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = enteredScaleVector;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = normalScaleVector;
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        normalScaleVector = rect.localScale;
        enteredScaleVector = new Vector3(enteredScale, enteredScale, enteredScale);
    }
}
