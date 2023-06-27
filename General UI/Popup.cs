using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] Canvas stackablePopup;
    [SerializeField] Canvas yesNoPopup;
    [SerializeField] TMP_Text questionText;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] Button acceptButton;
    [SerializeField] Button backButton;
    [SerializeField] Button closeButton;
    [SerializeField] Slider slider;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text maximumText;
    [SerializeField] Image popupBlocker;

    private static Popup instance;
    public static Popup Instance { get => instance; }

    private void Awake()
    {
        if (instance)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        noButton.onClick.AddListener(DisableMainCanvas);
        backButton.onClick.AddListener(DisableMainCanvas);
        closeButton.onClick.AddListener(DisableMainCanvas);
        slider.onValueChanged.AddListener(
            delegate
            {
                inputField.text = ((int)slider.value).ToString();
            });

        inputField.onValueChanged.AddListener(
            delegate
            {
                if (int.Parse(inputField.text) > slider.maxValue)
                    inputField.text = ((int)slider.maxValue).ToString();

                slider.value = int.Parse(inputField.text);
            });

        DisableMainCanvas();
    }

    private void DisableMainCanvas()
    {
        ClearEvents();
        popupBlocker.enabled = false;
        gameObject.SetActive(false);
    }

    private void EnableMainCanvas()
    {
        gameObject.SetActive(true);
        popupBlocker.enabled = true;
    }

    private void ClearEvents()
    {
        yesButton.onClick.RemoveAllListeners();
        acceptButton.onClick.RemoveAllListeners();
    }

    private void EnableYesNoCanvas()
    {
        EnableMainCanvas();
        stackablePopup.enabled = false;
        yesNoPopup.enabled = true;
    }

    private void EnableStackablePopup()
    {
        EnableMainCanvas();
        yesNoPopup.enabled = false;
        stackablePopup.enabled = true;
    }

    public void LoadYesNoPopup(string text,Action<SlotUI> action,SlotUI slot,Vector2 position)
    {
        EnableYesNoCanvas();
        gameObject.transform.position = position;
        questionText.text = text;

        yesButton.onClick.AddListener(
            delegate 
            {
                action.Invoke(slot);
                DisableMainCanvas();
            });
    }

    public void LoadStackablePopup(string text,Action<SlotUI,int> action,SlotUI slot,Vector2 position)
    {
        EnableStackablePopup();
        gameObject.transform.position = position;
        questionText.text = text;
        slider.minValue = 0;
        slider.maxValue = slot.currentSlot.amount;
        slider.value = 0;
        maximumText.text = "/ " + slider.maxValue.ToString();

        acceptButton.onClick.AddListener(
            delegate 
            {
                action.Invoke(slot, (int)slider.value);
                DisableMainCanvas();
            });
    }

    public void LoadStackablePopup(string text, Action<IHotBar,int> action, IHotBar hotBar, Vector2 position)
    {
        EnableStackablePopup();
        gameObject.transform.position = position;
        questionText.text = text;
        slider.minValue = 1;
        slider.maxValue = 10;
        maximumText.text = "/" + slider.maxValue.ToString();

        acceptButton.onClick.AddListener(
            delegate
            {
                action.Invoke(hotBar,(int)slider.value);
                DisableMainCanvas();
            });
    }
}