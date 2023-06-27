using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleSlider : MonoBehaviour
{
    [SerializeField] UIManager manager;
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnSliderValueChanged()
    {
        manager.ChangeScale(slider.value);
    }
}
