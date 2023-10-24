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

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
        //targetPosition = new Vector2(initialPosition.x, initialPosition.y + upMax);
    }

    // Update is called once per frame
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
        //if (rectTransform.anchoredPosition.y >= targetPosition.y)
        //{
        //Debug.Log(rectTransform.anchoredPosition + " " + targetPosition.y);
        //    //아래로
        //    rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, initialPosition, Time.deltaTime * 50.0f);
        //}
        //else
        //{
        //    //위로
        //    rectTransform.anchoredPosition  = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 50.0f);

        //}
        time += Time.deltaTime;
    }
}
