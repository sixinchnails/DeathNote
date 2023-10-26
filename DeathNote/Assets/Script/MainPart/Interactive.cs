using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리를 위한 네임스페이스

public class Interactive : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector2 startPosition;

    // 추가된 변수
    private Vector2 previousMousePosition;
    private Vector2 throwVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;  // 초기에 중력의 영향을 받지 않도록 설정
    }

    private void Update()
    {
        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (GetComponent<Collider2D>().OverlapPoint(currentMousePosition))
            {
                isDragging = true;
                startPosition = currentMousePosition;
            }
        }

        if (isDragging)
        {
            // 움직임 속도 계산
            throwVelocity = (currentMousePosition - previousMousePosition) / Time.deltaTime;
            transform.position = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            rb.isKinematic = false;
            rb.velocity = throwVelocity * 0.1f; // 여기에서 0.5f는 스케일링 팩터입니다. 이 값을 조절하여 던지는 힘의 크기를 변경할 수 있습니다.
        }


        previousMousePosition = currentMousePosition;
    }
}