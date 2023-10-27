using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinAndFall : MonoBehaviour
{
    public OpeningManager manager;
    public GameObject book;

    float time;
    bool go = true;
    bool show = false;
    Vector2 originScale;
    float rotateSpeed = 250.0f;
    RectTransform rectTransform;
    Vector2 initialPosition;
    Vector2 targetPosition;

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
        rectTransform.anchoredPosition = new Vector2(initialPosition.x, 406);
        //목표 위치
        targetPosition = new Vector2(initialPosition.x, -300);
        //초기 회전 설정
        transform.rotation = Quaternion.Euler(0, 0, 130);

    }

    // Update is called once per frame
    void Update()
    {
        if (go)
        {
            //아래로 이동
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 250.0f);
            //회전
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime * 1.8f);
            //점점 커지기
            transform.localScale = Vector2.one * (1 + time*3) * originScale;
            time += Time.deltaTime;
        }
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetPosition);
        if(rectTransform.anchoredPosition.y <= targetPosition.y)
        {
            rotateSpeed = 0.0f;
            go = false;
            if(!show)
            {
                show = true;
                manager.BoxAppear();
            }
        }
    }
}
