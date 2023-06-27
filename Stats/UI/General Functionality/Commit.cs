using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Commit : StatsUIControl, IButton
{
    //Config Params
    public TMP_Text availablePoints;
    public TMP_Text strengthValue;
    public TMP_Text vitalityValue;
    public TMP_Text focusValue;
    public TMP_Text dexterityValue;

    //Cached Fields
    private Button theButton;
    private Color theColor;

    new private void Start()
    {
        base.Start();

        theButton = GetComponent<Button>();
        theColor = theButton.GetComponent<Image>().color;

        theButton.onClick.AddListener(CommitChanges);
    }

    public override void Load()
    {
        HandleUpdates();
        loaded = true;
    }

    private void CommitChanges()
    {
        stats.UpdateStats(int.Parse(strengthValue.text), int.Parse(dexterityValue.text), int.Parse(focusValue.text), int.Parse(vitalityValue.text), int.Parse(availablePoints.text));

        StatsUIManager.Instance.ToggleMenu();
        StatsUIManager.Instance.ToggleMenu();

        FindObjectOfType<BarUIManager>().Load();
    }

    public void HandleUpdates()
    {
        bool valuesChanged = (int.Parse(strengthValue.text) != stats.allStats.strength)
                              || (int.Parse(dexterityValue.text) != stats.allStats.dexterity)
                              || (int.Parse(focusValue.text) != stats.allStats.focus) || (int.Parse(vitalityValue.text) != stats.allStats.vitality);

        if (valuesChanged)
            EnableButton();
        else
            DisableButton();
    }

    public void EnableButton()
    {
        theButton.interactable = true;
        theColor.a = 255;
    }

    public void DisableButton()
    {
        theButton.interactable = false;
        theColor.a = 125;
    }

}
