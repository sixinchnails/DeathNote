//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class SilderUi : MonoBehaviour
//{
//    [SerializeField]
//    private Text text;
//    [SerializeField]
//    private Slider slider;

//    private void Awake()
//    {
//        slider.onValueChanged.AddListener(OnSliderEvent);
//        OnSliderEvent(slider.value); // 슬라이더의 초기 값을 사용하여 텍스트를 설정
//    }

//    public void OnSliderEvent(float value)
//    {
//        text.text = $"{value * 100:F0}%";
//    }
//}
