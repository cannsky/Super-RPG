using UnityEngine;

public class PageButton : MonoBehaviour
{
    public void LoadNextPage()
    {
        InventoryUI.Instance.LoadPage(InventoryUI.Page.NextPage);
    }

    public void LoadPreviousPage()
    {
        InventoryUI.Instance.LoadPage(InventoryUI.Page.PreviousPage);
    }
}
