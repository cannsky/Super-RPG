using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System.Reflection;

public class ItemDetailsMenu : MonoBehaviour,IDragHandler
{
    [NonSerialized] public SlotUI currentItem;
    [NonSerialized] public static Vector4 limits;
    [SerializeField] TMP_Text itemName;
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemDescription;
    [SerializeField] Button splitButton;
    [SerializeField] Button stackButton;
    [SerializeField] Button dropButton;
    [SerializeField] Button useButton;
    [SerializeField] Button equipButton;
    [SerializeField] Button unequipButton;
    [SerializeField] List<TMP_Text> stats;
    ItemDetailsControls controls;

    private static ItemDetailsMenu instance;
    public static ItemDetailsMenu Instance { get => instance; }

    public bool isOpened = true;

    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        controls = ItemDetailsControls.Instance;
    }

    public void Load(SlotUI slot)
    {
        if (slot.currentSlot.item == null)
        {
            if (isOpened) Unload();
            return;
        }

        gameObject.SetActive(true);
        currentItem = slot;
        controls.Load(slot);
        transform.position = slot.transform.position;
        limits = new Vector4(transform.position.x - 168.3487f, transform.position.y - 280.7951f, transform.position.x + 168.3487f, transform.position.y + 280.7951f);
        isOpened = true;

        Item item = slot.currentSlot.item;
        int amount = slot.currentSlot.amount;
        bool isEquipment = false;

        if(item.GetType().IsSubclassOf(typeof(Equipment)))
        {
            useButton.gameObject.SetActive(false);
            isEquipment = true;

            bool isEquipped = currentItem.GetType() == typeof(EquipmentSlotUI);

            unequipButton.gameObject.SetActive(isEquipped);
            equipButton.gameObject.SetActive(!isEquipped);
            
        }
        else if(item.GetType().IsSubclassOf(typeof(Consumable)) || item.GetType()==typeof(Consumable))
        {
            useButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(false);
        }
        else
        {
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(false);
        }

        if (item.stackable)
        {
            splitButton.gameObject.SetActive(true);
            stackButton.gameObject.SetActive(true);
        }
        else
        {
            splitButton.gameObject.SetActive(false);
            stackButton.gameObject.SetActive(false);
        }

        itemName.text = item.name;
        itemDescription.text = item.description;
        itemImage.sprite = item.icon;

        Type type = item.GetType();

        stats[0].text = "Item Type : " + type.ToString();

        stats[1].text = "Amount : " + amount;

        if (isEquipment)
        {
            Equipment eq = (Equipment)item;
            string stat = "";

            var fields = eq.equipmentStats.GetType().GetFields().Where(i=>(int)i.GetValue(eq.equipmentStats)!=0).ToList<FieldInfo>();
            
            for (int i = 0; i < fields.Count; i++)
            {
                stat = FixName(fields[i].Name) + " : " + fields[i].GetValue(eq.equipmentStats);

                stats[i + 2].text = stat;
            }
        }

        string FixName(string x)
        {
            x = x.Substring(0, 1) + x.Substring(1, x.Length - 1);
            List<int> indices = new List<int>();
            for (int i = 1; i < x.Length; i++)
                if(x[i]>64 && x[i]<91)
                    indices.Add(i + indices.Count);
            foreach (var index in indices)
                x = x.Substring(0, index) + " " + x.Substring(index + 1, x.Length - index);
            return x;
        }
    }

    public void Unload()
    {
        Clean();
        isOpened = false;
        gameObject.SetActive(false);
    }

    [Obsolete(message:"This is maybe a future feature",error:false)]
    private int CalculateLines(string text)
    {
        float lowerCaseCount = 0;
        float upperCaseCount = 0;
        float numberCount = 0;
        float spaceCount = 0;
        float newLineCount = 0;
        float others = 0;

        foreach (var item in text)
        {
            if (char.IsLower(item))
                lowerCaseCount++;
            else if (char.IsUpper(item))
                upperCaseCount++;
            else if (char.IsNumber(item))
                numberCount++;
            else if (item.Equals(' '))
                spaceCount++;
            else if (item.Equals('\n'))
                newLineCount++;
            else
                others++;
        }

        spaceCount += ((upperCaseCount * 85) / 36);
        spaceCount += ((lowerCaseCount * 85) / 49);
        spaceCount += ((numberCount * 85) / 43);
        spaceCount += ((others * 85) / 53.25f);

        newLineCount += (spaceCount / 85);

        return Mathf.RoundToInt(newLineCount) + 1;
    }

    public void Clean()
    {
        itemName.text = "";
        itemImage.sprite = null;
        itemDescription.text = "";
        currentItem = null;
        foreach (var item in stats)
        {
            item.text = "";
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }
}
