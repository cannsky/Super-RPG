using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FilterSystem : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_InputField searchField;
    [SerializeField] Toggle toggle;
    [SerializeField] Button button;
    List<Type> types;

    Filter filter;
    private void Start()
    {
        types = new List<Type>();
        foreach (var item in dropdown.options)
        {
            types.Add(Type.GetType(item.text));
        }

        types[0] = typeof(Item);

        filter = new Filter();
        filter.type = typeof(Item);
        filter.searchFilter = "";

        dropdown.onValueChanged.AddListener(i => OnDropdownValueChanged());
        searchField.onValueChanged.AddListener(i => OnInputValueChanged());
        toggle.onValueChanged.AddListener(i => OnToggleValueChanged());
    }

    private void OnDropdownValueChanged()
    {
        filter.type = types[dropdown.value];
        UpdateFilterAndLoad();
    }

    private void OnInputValueChanged()
    {
        filter.searchFilter = searchField.text;
        if (toggle.isOn)
            UpdateFilterAndLoad();
    }

    private void OnToggleValueChanged()
    {
        if (toggle.isOn)
        {
            button.interactable = false;
            OnInputValueChanged();
        }
        else
            button.interactable = true;
    }

    public void OnButtonClicked()
    {
        filter.searchFilter = searchField.text;
        UpdateFilterAndLoad();
    }

    private void UpdateFilterAndLoad()
    {
        InventoryUI.Instance.currentFilter = filter;
        InventoryUI.Instance.FetchSlots();
    }
}
