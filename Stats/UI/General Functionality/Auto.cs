using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Auto : StatsUIControl,IButton
{
    //Config Params
    [SerializeField] TMP_Text availablePoints;

    //Cached Fields
    List<ValueCalibrator> increasors;
    List<ValueCalibrator> decreasors;
    Button theButton;
    Color theColor;

    new private void Start()
    {
        base.Start();
        theButton = GetComponent<Button>();
        theColor = theButton.GetComponent<Image>().color;

        theButton.onClick.AddListener(AutoAssignPoints);

        var calibrators = FindObjectsOfType<ValueCalibrator>().ToList();
        increasors = calibrators.Where(vc => vc.type == ValueCalibrator.CalibratorType.Increasor).ToList();
        decreasors = calibrators.Where(vc => vc.type == ValueCalibrator.CalibratorType.Decreasor).ToList();
    }

    public override void Load()
    {
        increasors.Where(vc => vc.GetLoaded() == false).ToList().ForEach(vc => vc.Load());

        HandleUpdates();
    }

    public void AutoAssignPoints()
    {
        int pointsRemaining = stats.idlestatPoints;
        int increaseAmount = 0;

        foreach (var item in decreasors)
        {
            while (item.isValidForDecrement())
            {
                item.Decrease();
            }
        }

        int counter = 1;

        while (pointsRemaining!=0)
        {
            increaseAmount = Random.Range(1, pointsRemaining);
            //Find out why 0 crashes unity!!!
            pointsRemaining -= increaseAmount;

            for (int i = 0; i < increaseAmount; i++)
            {
                increasors[counter%increasors.Count].Increase();
            }
            counter++;
        }      
    }

    public void HandleUpdates()
    {
        if(stats.idlestatPoints==0)
        {
            DisableButton();
        }
        else
        {
            EnableButton();
        }
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
