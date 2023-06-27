using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LockButton : MonoBehaviour
{
    [SerializeField] InventoryUIManager manager;
    Button button;
    TMP_Text text;
    private void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(Lock);
    }

    public void Lock()
    {
        manager.Lock();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Unlock);
        text.text = "Unlock";
    }

    public void Unlock()
    {
        manager.Unlock();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Lock);
        text.text = "Lock";
    }
}