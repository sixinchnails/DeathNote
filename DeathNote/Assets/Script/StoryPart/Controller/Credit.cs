using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    RectTransform rectTransform;
    GameObject credit;
    public GameObject logo;
    public GameObject img1;
    public GameObject img2;
    public GameObject img3;
    public GameObject img4;
    public GameObject name;
    public GameObject name2;

    Vector2 targetPosition;
    //float t = 0.01f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        credit = GetComponent<GameObject>();
        logo.SetActive(false);
        img1.SetActive(false);
        img2.SetActive(false);
        img3.SetActive(false);
        img4.SetActive(false);
        name.SetActive(false);
        name2.SetActive(false);

        targetPosition = new Vector2 (0, 2060);
    }

    public void up()
    {
        //캔버스 위치로 바로 이동
        rectTransform.anchoredPosition = new Vector2(0, -2060);
        logo.SetActive(true);
        StartCoroutine(delay(1.5f));
    }

    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine (scroll());
    }

    IEnumerator scroll()
    {
        float duration = 10.0f; // 전체 이동을 마치는 데 걸리는 시간 (초)
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float yPos = Mathf.Lerp(-2060, targetPosition.y, elapsedTime / duration);
            rectTransform.anchoredPosition = new Vector2(0, yPos);

            elapsedTime += Time.deltaTime;

            show();

            yield return null;
        }
    }

    void show()
    {
        Invoke("showImg1", 1f);
        Invoke("showImg2", 3f);
        Invoke("showImg3", 5f);
        Invoke("showImg4", 7f);
        Invoke("showName", 9f);
    }

    void showImg1()
    {
        img1.SetActive(true);
    }

    void showImg2()
    {
        img2.SetActive(true);
    }

    void showImg3()
    {
        img3.SetActive(true);
    }

    void showImg4()
    {
        img4.SetActive(true);
    }
    void showName()
    {
        name.SetActive(true);
        name2.SetActive(true);
    }
}
