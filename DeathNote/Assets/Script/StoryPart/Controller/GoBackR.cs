using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoBackR : MonoBehaviour
{
    RectTransform rectTransform;
    public Image reaper;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void back()
    {
        rectTransform.SetAsFirstSibling();
        reaper.color = new Color(255, 255, 255, 0.3f);
    }

    public void forward()
    {
        rectTransform.SetAsLastSibling();
        reaper.color = new Color(255, 255, 255, 1);
    }
}
