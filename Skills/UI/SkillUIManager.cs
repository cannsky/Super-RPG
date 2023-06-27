using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkillUIManager : UIManager
{
    [SerializeField] GridLayoutGroup gridContentParent;
    public List<Skill> skills;
    [NonSerialized] public int skillPoints;
    [SerializeField] SkillSlot sampleSlot;
    [SerializeField] new TMP_Text name;
    [SerializeField] TMP_Text desc;
    [SerializeField] TMP_Text level;
    [SerializeField] Image nameBackground;
    [SerializeField] Image image;
    [SerializeField] Button levelUp;
    [SerializeField] Button assign;
    [SerializeField] Popup popup;
    Skill selectedSkill;
    SkillSlot selectedSlot;
    HotBarUIManager hotBarManager;
    List<SkillSlot> slots = new List<SkillSlot>();

    public override void Start()
    {
        hotBarManager = FindObjectOfType<HotBarUIManager>();
        ToggleMenu();
    }

    public override void ToggleMenu()
    {
        //primaryMenu.Toggle();
        //if(primaryMenu.GetActive())
        //    Load();
        //else
        //    ClearSelectedMenu();
    }

    public void Load()
    {
        foreach (var slot in slots)
            Destroy(slot.gameObject);

        slots.Clear();

        foreach (var skill in skills)
        {
            var slot = Instantiate(sampleSlot, gridContentParent.transform);
            slots.Add(slot);
            slot.Load(skill);
        }
    }
    public void ClearSelectedMenu()
    {
        desc.text = string.Empty;
        name.text = string.Empty;
        level.text = string.Empty;
        image.enabled = false;
        nameBackground.enabled = false;
        levelUp.interactable = false;
        assign.interactable = false;
    }

    public void SelectSkill(SkillSlot slot)
    {
        selectedSlot?.ToggleSelection();
        selectedSlot = slot;
        selectedSkill = slot.skill;
        UpdateUI();
    }

    public void UpdateUI()
    {
        nameBackground.enabled = true;
        name.text = selectedSkill.skillName;
        desc.text = selectedSkill.skillDescription;
        level.text = "Level : " + selectedSkill.skillLevel;
        assign.interactable = true;
        if (selectedSkill.skillLevel == 10)
        {
            level.text += "(Max)";
            levelUp.interactable = false;
        }
        else levelUp.interactable = true;
        image.sprite = selectedSkill.GetImage();
    }

    public void AssignButton()
    {
        if(!hotBarManager.Add(selectedSkill))
        {
            popup.LoadStackablePopup("At which slot do you want to assign?", hotBarManager.AddToSlot, selectedSkill, transform.position);
        }
    }

    public void LevelUpButton()
    {
        skillPoints--;
        selectedSkill.skillLevel++;
    }

    public override void ChangeScale(float scale)
    {
        throw new NotImplementedException();
    }

    public override void ArrangePositions()
    {
        throw new NotImplementedException();
    }

    public override void ResetPositions()
    {
        throw new NotImplementedException();
    }

    protected override float GetCurrentLength(float maxScaleDistance)
    {
        throw new NotImplementedException();
    }
    public override Vector2 GetMaximumMenuSize(int index)
    {
        throw new NotImplementedException();
    }
    public override Vector2 GetCurrentMenuSize(int index)
    {
        throw new NotImplementedException();
    }
}
