using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class HotBarSlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler,IHotBar
{
    public IHotBar currentHotBarItem;
    TMP_Text amountText;
    Image itemImage;
    protected CanvasGroup group;
    static HotBarSlotUI draggedSlot; 

    private void Start()
    {
        amountText = GetComponentInChildren<TMP_Text>();
        itemImage = amountText.transform.parent.GetComponent<Image>();
        group = GetComponent<CanvasGroup>();
        UIController();
    }

    public void Load(IHotBar hotbar)
    {
        currentHotBarItem = hotbar;
        UIController();
    }

    public void UpdateHotBar()
    {
        if (GetAmount() == 0)
            currentHotBarItem = null;
        UIController();
    }

    public void UIController()
    {
        if(currentHotBarItem is null)
        {
            amountText.text = "";
            itemImage.enabled = false;
            return;
        }

        int amount = currentHotBarItem.GetAmount();
        amountText.text = amount == 0 || amount == 1 ? "" : amount.ToString();
        itemImage.enabled = true;
        itemImage.sprite = GetImage();
    }

    public void Use()
    {
        currentHotBarItem?.Use();
        UpdateHotBar();
    }

    public Sprite GetImage() => currentHotBarItem.GetImage();

    public int GetAmount() => currentHotBarItem is object ? currentHotBarItem.GetAmount() : 0;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(currentHotBarItem is object)
        {
            itemImage.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            draggedSlot = this;
            draggedSlot.transform.SetAsLastSibling();
            group.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData) => itemImage.transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);

    public void OnDrop(PointerEventData eventData)
    {
        IHotBar hotBar=null;
        if (InventorySlotUI.draggedItem is object)
        {
            try
            {
                hotBar = (IHotBar)InventorySlotUI.draggedItem.item;
            }
            catch (System.Exception)
            {
            }
        }
        else if (SkillSlot.draggedSlot is object)
        {
            try
            {
                hotBar = (IHotBar)SkillSlot.draggedSlot.skill;
            }
            catch (System.Exception)
            {
            }
        }
        else if (draggedSlot is object)
        {
            hotBar = draggedSlot.currentHotBarItem;
            draggedSlot.Load(null);
        }
        else
            return;
        Load(hotBar);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.transform.localScale = Vector3.one;
        itemImage.transform.localPosition = Vector3.zero;
        draggedSlot = null;
        group.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        currentHotBarItem?.Use();
        //skill cooldown and stuff
    }
}
