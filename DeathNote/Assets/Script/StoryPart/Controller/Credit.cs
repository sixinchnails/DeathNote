using System.Collections;
using System.Collections.Generic;
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
    }

    public void up()
    {
        //캔버스 위치로 바로 이동
        rectTransform.anchoredPosition = new Vector2(0, -2060);
        StartCoroutine(scroll());
    }

    IEnumerator scroll()
    {
        yield return null;
    }
}
