using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoBackM : MonoBehaviour
{
    RectTransform rectTransform;
    public Image me;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void back()
    {
        me.color = new Color(255, 255, 255, 0.5f);
        rectTransform.SetAsFirstSibling();
    }

    public void forward()
    {
        me.color = new Color(255, 255, 255, 1);
        rectTransform.SetAsLastSibling();
    }
}
