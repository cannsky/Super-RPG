using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class SkillSlot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
{
    [SerializeField] Image unselectedFrame;
    [SerializeField] Image selectedFrame;
    [SerializeField] Image skillImage;
    [System.NonSerialized] public Skill skill;
    SkillUIManager manager;
    public static SkillSlot draggedSlot;

    private void Awake()
    {
        selectedFrame.enabled = false;
    }

    private void Start()
    {
        manager = FindObjectOfType<SkillUIManager>();
    }

    public void Load(Skill skill)
    {
        skillImage.sprite = skill.skillIcon;
        this.skill = skill;
        selectedFrame = GetComponentsInChildren<Image>().Where(i => i.name == "Selected Frame").First();
        unselectedFrame = GetComponentsInChildren<Image>().Where(i => i.name == "Unselected Frame").First();
        skillImage = GetComponentsInChildren<Image>().Where(i => i.name == "Skill").First();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.parent.SetAsLastSibling();
        skillImage.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        draggedSlot = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        skillImage.transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        skillImage.transform.localScale = new Vector3(1f, 1f, 1f);
        skillImage.transform.localPosition = Vector3.zero;
        draggedSlot = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            manager.SelectSkill(this);
            ToggleSelection();
        }
    }

    public void ToggleSelection()
    {
        selectedFrame.enabled = !selectedFrame.enabled;
        unselectedFrame.enabled = !unselectedFrame.enabled;
    }
}
