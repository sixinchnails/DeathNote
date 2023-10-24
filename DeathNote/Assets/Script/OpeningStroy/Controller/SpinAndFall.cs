using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinAndFall : MonoBehaviour
{
    float time;
    bool go = true;
    Vector2 originScale;
    float rotateSpeed = 250.0f;
    RectTransform rectTransform;
    Vector2 initialPosition;
    Vector2 targetPosition;

    public OpeningManager manager;
    public GameObject book;
    public GameObject scriptBox;

    // Start is called before the first frame update
    void Start()
    {
        //원래 크기 저장
        originScale = transform.localScale;
        //UI 요소의 RectTransfrom 가져오기
        rectTransform = GetComponent<RectTransform>();
        //초기 위치 저장
        initialPosition = rectTransform.anchoredPosition;
        //시작 위치 설정
        rectTransform.anchoredPosition = new Vector2(initialPosition.x, 200);
        //목표 위치
        targetPosition = new Vector2(initialPosition.x, -115);
        //초기 회전 설정
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (go)
        {
            //아래로 이동
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 100.0f);
            //회전
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime * 1.8f);
            //점점 커지기
            transform.localScale = Vector2.one * (1 + time*8) * originScale;
            time += Time.deltaTime;
        }
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetPosition);
        if(rectTransform.anchoredPosition.y <= targetPosition.y)
        {
            rotateSpeed = 0.0f;
            go = false;
            Invoke("disappear", 1.5f);
        }
    }

    public void disappear()
    {
        book.SetActive(false);
        if (!scriptBox.activeSelf)
        {
            manager.BoxAppear();
        }
    }
}
