using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SilderUi : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderEvent);
    }

    public void OnSliderEvent(float value)
    {
        text.text = $"{value * 100:F1}%";
    }
}
