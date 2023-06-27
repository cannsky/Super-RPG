using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class InventoryUIManager : UIManager
{
    [SerializeField] Image draggingImage;

    InventoryUI inventoryUI;
    ItemDetailsMenu detailsMenu;

    private static InventoryUIManager instance;
    public static InventoryUIManager Instance { get => instance; }

    #region Constants
    //Every constant here is valid only at maximum scale 
    const float InventoryMenuXPosition = 433f;
    const float GearMenuXPosition = -294.5f;
    const float DistanceBetweenMenus = 90f;
    #endregion

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    public override void Start()
    {
        inventoryUI = InventoryUI.Instance;
        detailsMenu = ItemDetailsMenu.Instance;
        currentScale = inventoryUI.transform.localScale.x;
        //ToggleMenu();
    }

    public override void ToggleMenu()
    {
        if (menus[1].GetActive() && !menus[0].GetActive())
            menus[0].Open();
        else if (menus[0].GetActive() && !menus[1].GetActive())
            menus[1].Open();
        else
        {
            menus.ForEach(m => m.Toggle());

            if(menus[0].GetActive())
            {
                gameObject.SetActive(true);
                inventoryUI.currentFilter = new Filter(typeof(Item), "");
                inventoryUI.FetchSlots();
                InputManager.mode = InputManager.UIMode.Inventory;
            }
            else
            {
                InputManager.mode = InputManager.UIMode.None;
                gameObject.SetActive(false);
                if (detailsMenu.isOpened)
                    detailsMenu.Unload();
            }
        }
    }

    public override void ChangeScale(float scale)
    {
        currentScale = scale < MinScale ? MinScale : scale > MaxScale ? MaxScale : scale;
        Vector3 newScale = new Vector3(scale, scale, scale);
        menus.ForEach(m => m.SetLocalScale(newScale));
        draggingImage.transform.localScale = newScale;
        ArrangePositions();
    }

    public override void ArrangePositions()
    {
        ResetPositions();
        Vector3 approachVector = GetApproachVector(false,true);
        menus[0].SetLocalPosition(menus[0].GetLocalPosition() - approachVector);
        menus[1].SetLocalPosition(menus[1].GetLocalPosition() + approachVector);
    }

    public override void ResetPositions()
    {
        Vector3 pos = new Vector3(InventoryMenuXPosition, YPosition, 0);
        menus[0].SetLocalPosition(pos);
        pos.x = GearMenuXPosition;
        menus[1].SetLocalPosition(pos);
    }

    protected Vector3 GetApproachVector(bool snapping,bool halved)
    {
        float newGapBetweenMenus = GetCurrentLength(DistanceBetweenMenus);
        float currentGap = (menus[0].GetLocalPosition().x - GetCurrentMenuSize(0).x / 2f) - (menus[1].GetLocalPosition().x + GetCurrentMenuSize(1).x / 2f);
        float approach = currentGap - newGapBetweenMenus;

        return new Vector3(halved ? approach / 2f : approach, 0, 0);
    }


    //Vector3 GetSnapVector()
    //{
    //    if (inventoryRect.localPosition.x >= gearRect.localPosition.x)
    //        return new Vector3(GetGap(inventoryRect.localPosition.x, InventoryMenuSize.x, gearRect.localPosition.x, GearMenuSize.x), 0, 0);
    //    else
    //        return new Vector3(GetGap(gearRect.localPosition.x, GearMenuSize.x, inventoryRect.localPosition.x, InventoryMenuSize.x), 0, 0);

    //    float GetGap(float right,float sizeRight,float left,float sizeLeft)
    //    {
    //        float gap = (right - GetNewLength(sizeRight) / 2f) - (left + GetNewLength(sizeLeft) / 2f);
    //        Debug.Log(gap);
    //        return gap;
    //    }
    //}
}