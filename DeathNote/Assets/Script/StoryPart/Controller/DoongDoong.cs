using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoongDoong : MonoBehaviour
{
    RectTransform rectTransform;

    float time;
    float upMax = 8.0f;
    float downMax = -8.0f;
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
        if (up)
        {
            targetPosition = initialPosition + new Vector2(0, upMax);
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 50.0f);
            if(Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
            {
                up = false;
            }
        }
        else
        {
            targetPosition = initialPosition + new Vector2(0, downMax);
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 50.0f);
            if(Vector2.Distance(rectTransform.anchoredPosition, targetPosition) < 0.1f)
            {
                up = true;
            }
        }
        time += Time.deltaTime;
    }
}
