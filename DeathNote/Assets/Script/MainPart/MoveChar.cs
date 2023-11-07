using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveChar : MonoBehaviour
{
    public float speed = 10.0f;  // 캐릭터의 움직임 속도
    private Vector2 direction;  // 캐릭터가 움직일 방향
    private float changeDirectionTime = 2.0f;  // 방향을 바꾸는 시간 간격
    public RectTransform boundaryRectTransform; // 움직임을 제한할 이미지의 RectTransform
    private RectTransform rectTransform; // 캐릭터의 RectTransform 컴포넌트
    private Vector2 parentSize;  // 부모 이미지의 크기

    void Start()
    {
        // 현재 게임 오브젝트에 붙어있는 RectTransform 컴포넌트를 가져옵니다.
        rectTransform = GetComponent<RectTransform>();

        // 부모 이미지의 크기를 가져옵니다.
        parentSize = boundaryRectTransform.sizeDelta;

        // 초기 방향을 선택합니다.
        ChooseDirection();

        // 2초마다 방향을 바꾸도록 설정합니다.
        InvokeRepeating("ChooseDirection", changeDirectionTime, changeDirectionTime);
    }

    void Update()
    {
        // 움직일 양을 계산합니다.
        Vector2 moveAmount = direction * speed * Time.deltaTime;

        // 새로운 위치를 계산합니다.
        Vector2 newPos = rectTransform.anchoredPosition + moveAmount;

        // 캐릭터가 부모 이미지를 넘어가지 않도록 x와 y 위치를 조절합니다.
        newPos.x = Mathf.Clamp(newPos.x, -parentSize.x / 2 + rectTransform.sizeDelta.x / 2, parentSize.x / 2 - rectTransform.sizeDelta.x / 2);
        newPos.y = Mathf.Clamp(newPos.y, -parentSize.y / 2 + rectTransform.sizeDelta.y / 2, parentSize.y / 2 - rectTransform.sizeDelta.y / 2);

        // 캐릭터의 위치를 업데이트합니다.
        rectTransform.anchoredPosition = newPos;
    }

    // 랜덤한 방향을 선택하는 함수입니다.
    void ChooseDirection()
    {
        float h = Random.Range(-1f, 1f);  // 가로 방향 랜덤 값
        float v = Random.Range(-1f, 1f);  // 세로 방향 랜덤 값
        direction = new Vector2(h, v).normalized;  // 방향을 정규화하여 길이가 1이 되도록 합니다.
    }
}
