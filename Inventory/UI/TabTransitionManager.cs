using UnityEngine;
using UnityEngine.UI;

public class TabTransitionManager : MonoBehaviour
{
    [SerializeField] Canvas armourMenu;
    [SerializeField] Canvas jewelleryMenu;
    [SerializeField] Canvas companionMenu;
    [SerializeField] Button armourButton;
    [SerializeField] Button jewelleryButton;
    [SerializeField] Button companionButton;

    const float ratio = 1.2f;
    int openedTab;
    Vector2 sizeDelta;


    private void Start()
    {
        openedTab = 1;
        OpenArmourMenu();
    }

    public void OpenMenuAbove()
    {
        switch (openedTab)
        {
            case 0:
                OpenCompanionMenu();
                break;
            case 1:
                OpenArmourMenu();
                break;
            case 2:
                OpenJewelleryMenu();
                break;
            default:
                break;
        }
    }

    public void OpenMenuBelow()
    {
        switch (openedTab)
        {
            case 0:
                OpenJewelleryMenu();
                break;
            case 1:
                OpenCompanionMenu();
                break;
            case 2:
                OpenArmourMenu();
                break;
            default:
                break;
        }

    }

    public void OpenArmourMenu()
    {
        jewelleryMenu.enabled = false;
        companionMenu.enabled = false;
        armourMenu.enabled = true;
        
        if (openedTab == 1)
        {
            sizeDelta = jewelleryButton.GetComponent<RectTransform>().sizeDelta;
            jewelleryButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / ratio, sizeDelta.y / ratio);
        }
        else if (openedTab == 2)
        {
            sizeDelta = companionButton.GetComponent<RectTransform>().sizeDelta;
            companionButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / ratio, sizeDelta.y / ratio);
        }
        else return;

        sizeDelta = armourButton.GetComponent<RectTransform>().sizeDelta;
        armourButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x * ratio, sizeDelta.y * ratio);
        openedTab = 0;
    }

    public void OpenJewelleryMenu()
    {
        armourMenu.enabled = false;
        companionMenu.enabled = false;
        jewelleryMenu.enabled = true;

        if (openedTab == 0)
        {
            sizeDelta = armourButton.GetComponent<RectTransform>().sizeDelta;
            armourButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / ratio, sizeDelta.y / ratio);
        }
        else if (openedTab == 2)
        {
            sizeDelta = companionButton.GetComponent<RectTransform>().sizeDelta;
            companionButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / ratio, sizeDelta.y / ratio);
        }
        else return;

        sizeDelta = jewelleryButton.GetComponent<RectTransform>().sizeDelta;
        jewelleryButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x * ratio, sizeDelta.y * ratio);
        openedTab = 1;
    }

    public void OpenCompanionMenu()
    {
        armourMenu.enabled = false;
        jewelleryMenu.enabled = false;
        companionMenu.enabled = true;

        if (openedTab == 0)
        {
            sizeDelta = armourButton.GetComponent<RectTransform>().sizeDelta;
            armourButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / ratio, sizeDelta.y / ratio);
        }
        else if (openedTab == 1)
        {
            sizeDelta = jewelleryButton.GetComponent<RectTransform>().sizeDelta;
            jewelleryButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x / ratio, sizeDelta.y / ratio);
        }
        else return;

        sizeDelta = companionButton.GetComponent<RectTransform>().sizeDelta;
        companionButton.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x * ratio, sizeDelta.y * ratio);
        openedTab = 2;
    }
}
