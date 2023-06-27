using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueCalibrator : StatsUIControl,IButton
{
    public enum CalibratorType
    {
        Decreasor = 0,
        Increasor
    }

    //Config Params
    [SerializeField] TMP_Text targetText;
    [SerializeField] TMP_Text availablePoints;
    [SerializeField] TMP_Text increment;
    
    public CalibratorType type;


    //Cached Fields
    private List<ValueCalibrator> increasors;
    private List<ValueCalibrator> decreasors;
    private Commit commitBtn;
    private Button theButton;
    private Image theImage;
    private Color theColor;
    private StatsUIStatTypes statType;

    protected override void Start()
    {
        base.Start();
        List<ValueCalibrator> allCalibrators = FindObjectsOfType<ValueCalibrator>().ToList();
        increasors = allCalibrators.Where(vc => vc.type == CalibratorType.Increasor).ToList();
        decreasors = allCalibrators.Where(vc => vc.type == CalibratorType.Decreasor).ToList();
        commitBtn = FindObjectOfType<Commit>();
        theButton = GetComponent<Button>();
        theImage = theButton.GetComponent<Image>();
        theColor = theImage.color;
        statType = targetText.GetComponent<StatInfoReceiver>().statType;

        if (type == CalibratorType.Decreasor)
        {
            theButton.onClick.AddListener(Decrease);
        }
        else
        {
            theButton.onClick.AddListener(Increase);
        }
    }

    public override void Load()
    {
        if (targetText.GetComponent<UIControl>().GetLoaded() == false)
        {
            targetText.GetComponent<UIControl>().Load();
        }

        if (availablePoints.GetComponent<UIControl>().GetLoaded() == false)
        {
            availablePoints.GetComponent<UIControl>().Load();
        }

        if (increment.GetComponent<UIControl>().GetLoaded() == false)
        {
            increment.GetComponent<UIControl>().Load();
        }

        HandleUpdates();

        loaded = true;
    }

    public void Increase()
    {
        targetText.text = (int.Parse(targetText.text) + 1).ToString();
        availablePoints.text = (int.Parse(availablePoints.text) - 1).ToString();

        CalculateIncrementPoints();

        HandleUpdates();
    }

    public void Decrease()
    {
        targetText.text = (int.Parse(targetText.text) - 1).ToString();
        availablePoints.text = (int.Parse(availablePoints.text) + 1).ToString();

        CalculateIncrementPoints();

        HandleUpdates();
    }

    public bool isValidForDecrement()
    {
        bool isValid = false;

        switch (statType)
        {
            case StatsUIStatTypes.Strength:
                isValid = int.Parse(targetText.text) > stats.allStats.strength;
                break;
            case StatsUIStatTypes.Vitality:
                isValid = int.Parse(targetText.text) > stats.allStats.vitality;
                break;
            case StatsUIStatTypes.Focus:
                isValid = int.Parse(targetText.text) > stats.allStats.focus;
                break;
            case StatsUIStatTypes.Dexterity:
                isValid = int.Parse(targetText.text) > stats.allStats.dexterity;
                break;
            default:
                Debug.LogWarning("These buttons are not supposed to update available points directly,are you trying to implement some kind of a hack?");
                break;
        }

        return isValid;
    }

    private void CalculateIncrementPoints()
    {
        if (type == CalibratorType.Decreasor)
        {
            if (increment.text == "+1")
            {
                increment.text = string.Empty;
            }
            else
            {
                increment.text = "+" + (int.Parse(increment.text.Substring(1, increment.text.Length - 1)) - 1);
            }
        }
        else
        {
            if (increment.text == string.Empty)
            {
                increment.text = "+1";
            }
            else
            {
                increment.text = "+" + (int.Parse(increment.text.Substring(1, increment.text.Length - 1)) + 1);
            }
        }
    }

    public void HandleUpdates()
    {
        if (int.Parse(availablePoints.text) == 0)
        {
            increasors.ToList().ForEach(vc => vc.DisableButton());
        }
        else
        {
            increasors.Where(vc => vc.theButton.interactable == false)
                .ToList().ForEach(vc => vc.EnableButton());
        }

        decreasors.Where(vc => vc.isValidForDecrement() == true).ToList().ForEach(vc => vc.EnableButton());
        decreasors.Where(vc => vc.isValidForDecrement() == false).ToList().ForEach(vc => vc.DisableButton());

        commitBtn.HandleUpdates();
    }

    public void DisableButton()
    {
        theButton.interactable = false;
        theColor.a = 125;
    }

    public void EnableButton()
    {
        theButton.interactable = true;
        theColor.a = 255;
    }

    //public void Peek()
    //{
    //    if(increment.name== "IncreaseIndicatorStr_txt" || increment.name== "IncreaseIndicatorDex_txt")
    //    {
    //        int strIncrease;
    //        int dexIncrease;

    //        if(increment.name== "IncreaseIndicatorStr_txt")
    //        {
    //            strIncrease = int.Parse(increment.text.Substring(1, increment.text.Length - 1));

    //            string dexIncreaseText = FindObjectsOfType<StatInfoReceiver>().Where(sir => sir.name == "IncreaseIndicatorDex_txt").ToList()
    //                          .FirstOrDefault().GetComponent<TMP_Text>().text;

    //            dexIncrease = int.Parse(dexIncreaseText.Substring(1, dexIncreaseText.Length - 1));


    //            Tuple<int, int, int> values = stats.PeekForStrengthandDexterity(strIncrease, dexIncrease);

    //            for (int i = 0; i < targetStats.Count; i++)
    //            {
    //                targetStats[i].text = values.Item1.ToString();
    //                targetStats[i].text = values.
    //            }
    //        }
    //        else
    //        {
    //            dexIncrease = int.Parse(increment.text.Substring(1, increment.text.Length - 1));

    //            string strIncreaseText = FindObjectsOfType<StatInfoReceiver>().Where(sir => sir.name == "IncreaseIndicatorStr_txt").ToList()
    //                                     .FirstOrDefault().GetComponent<TMP_Text>().text;

    //            strIncrease = int.Parse(strIncreaseText.Substring(1, strIncreaseText.Length - 1));


    //            Tuple<int, int, int> values = stats.PeekForStrengthandDexterity(strIncrease, dexIncrease);
    //        }



    //    }
    //    else
    //    {

    //    }

    //}
}
