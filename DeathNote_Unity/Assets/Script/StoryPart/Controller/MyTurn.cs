using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTurn : MonoBehaviour
{
    RectTransform rectTransform;

    float upMax = 1.0f;
    int n = 0;
    bool up = true;
    Vector2 initialPosition;
    Vector2 targetPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);

    }

    void Update()
    { 
        if(n >= 2)
        {
            up = false;
        }
        //µÎ ¹ø¸¸ µÕµÕ ÇÒ°ÅÀÓ
        if (up)
        {
            targetPosition = initialPosition + new Vector2(0, upMax);
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 50.0f);
            if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
            {
                n++;
                rectTransform.anchoredPosition = initialPosition;
            }
        }
    }
}
