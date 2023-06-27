using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void Notify(int page);

public class InventoryUI : MonoBehaviour
{
    public enum Page
    {
        PreviousPage = 0,
        NextPage = 1
    }

    [SerializeField] TMP_Text totalItemText;
    [SerializeField] TMP_Text pageText;
    [SerializeField] TMP_Text itemText;
    [SerializeField] Button nextPageButton;
    [SerializeField] Button previousPageButton;
    [SerializeField] public List<InventorySlotUI> inventorySlots;
    [SerializeField] public List<EquipmentSlotUI> equipmentSlots;

    Inventory inventory;
    [NonSerialized] public TabTransitionManager tabManager;
    [NonSerialized] public int currentTotalPageCount;
    [NonSerialized] public int currentPage;
    [NonSerialized] public int currentTotalItems;
    [NonSerialized] public int emptySlots;
    [NonSerialized] public bool isStarted = false;

    public event Notify PageChange;

    public Filter currentFilter;
    [NonSerialized] public List<Slot> loadedInventorySlots;

    #region Singleton

    private static InventoryUI instance;
    public static InventoryUI Instance { get => instance; }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this, 0.01f);
    }

    #endregion

    public void Start()
    {
        if (isStarted)
        {
            return;
        }

        tabManager = FindObjectOfType<TabTransitionManager>();
        inventory = Inventory.Instance;

        foreach (var item in equipmentSlots)
        {
            item.Start();
        }

        foreach (var item in inventorySlots)
        {
            item.Start();
        }
        inventory.Start();
        currentFilter = new Filter(typeof(Item),"");
        FetchSlots();
        isStarted = true;
    }

    public void FetchSlots()
    {
        while (currentFilter.searchFilter.StartsWith(" "))
        {
            currentFilter.searchFilter.Remove(0, 1);
        }

        inventory.ClearEmptySlots();

        loadedInventorySlots = inventory.inventorySlots.Where(i => i.item != null).Where(i =>
            {
                return (i.item.GetType() == currentFilter.type || i.item.GetType().IsSubclassOf(currentFilter.type));
            })
            .Where(i =>
            {
                return (String.IsNullOrEmpty(currentFilter.searchFilter)) ? true
            : i.item.name.ToUpper().StartsWith(currentFilter.searchFilter.ToUpper());
            }).ToList();


        loadedInventorySlots.Where(i => i.currentSlot).ToList().ForEach(i => i.DropSlot());

        currentTotalItems = loadedInventorySlots.Count;
        currentTotalPageCount = (currentTotalItems / 40) + 1;
        currentPage = 1;

        emptySlots = 40 - (currentTotalItems % 40);

        for (int i = 0; i < emptySlots; i++)
        {
            loadedInventorySlots.Add(inventory.inventorySlots[inventory.inventorySlots.Count - 40 + i]);
        }

        LoadSlots();
    }

    public void LoadPage(Page page)
    {
        if (page == Page.PreviousPage)
        {
            if (currentPage == 1)
                return;

            currentPage--;
        }
        else
        {
            if (currentPage == currentTotalPageCount)
                return;

            currentPage++;
        }

        FindObjectOfType<ClickHandler>().OnPointerClick(null);
        LoadSlots();
        OnPageChanged();
    }

    public void LoadSlots()
    {
        for (int i = 0; i < 40; i++)
        {
            inventorySlots[i].LoadSlot(loadedInventorySlots[(currentPage - 1) * 40 + i]);
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            equipmentSlots[i].LoadSlot(inventory.equipmentSlots[i]);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateTexts();
        HandleButtons();
    }

    private void UpdateTexts()
    {
        totalItemText.text = currentTotalItems.ToString() + " Items";
        pageText.text = currentPage + "/" + currentTotalPageCount + " Pages";
    }

    public void UpdateItemText(string name)
    {
        itemText.text = name;
    }

    private void HandleButtons()
    {
        if (currentPage == 1)
            previousPageButton.interactable = false;
        else
            previousPageButton.interactable = true;

        if ((currentPage < currentTotalPageCount))
            nextPageButton.interactable = true;
        else
            nextPageButton.interactable = false;
    }

    public void DecreaseItem()
    {
        currentTotalItems--;
        CheckPageFormation();
    }
    public void IncreaseItem()
    {
        currentTotalItems++;
        CheckPageFormation();
    }

    public void CheckPageFormation()
    {
        int count = loadedInventorySlots.Count;
        int lastPageEmptyCount = loadedInventorySlots.GetRange(count - 40, 40).FindAll(i => i.item == null).ToList().Count;

        List<int> emptyPages = new List<int>();

        for (int i = 0; i < currentTotalPageCount-1; i++)
        {
           if(loadedInventorySlots.GetRange(40 * i, 40).Where(s => s.item == null).ToList().Count == 40)
                emptyPages.Add(i+1);
        }

        if (lastPageEmptyCount == 0)
        {
            for (int i = 0; i < 40; i++)
            {
                loadedInventorySlots.Add(inventory.inventorySlots[inventory.inventorySlots.Count - 40 + i]);
            }

            currentTotalPageCount++;
        }

        foreach (var item in emptyPages)
        {
            loadedInventorySlots.RemoveRange((item-1) * 40, 40);
            currentTotalPageCount--;
            if (currentPage >= item)
                currentPage--;
        }

        UpdateUI();
    }
    protected virtual void OnPageChanged()
    {
        PageChange?.Invoke(currentPage);
    }
}