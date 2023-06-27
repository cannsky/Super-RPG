using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class tempscript : MonoBehaviour
{
    TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        foreach (var item in Enum.GetValues(typeof(Languages)))
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(((Languages)item).ToString()));
        }

        dropdown.onValueChanged.AddListener(ValueChanged);
    }

    void ValueChanged(int newValue)
    {

        foreach (var item in Enum.GetValues(typeof(Languages)))
        {
            if (dropdown.options[newValue].text == ((Languages)item).ToString())
                Settings.Instance.CurrentLanguage = (Languages)item;
        } 
    }
}
