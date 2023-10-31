using UnityEngine;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 offset; // 마우스 포인터와 이미지의 위치 차이
    private Vector2 lastMousePosition;
    private Vector2 dragStartPos;
    private Vector2 dragEndPos;
    private RectTransform rectTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 터치 입력이나 마우스 입력을 감지
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localMousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localMousePosition))
            {
                if (rectTransform.rect.Contains(localMousePosition))
                {
                    // 드래그 시작
                    isDragging = true;
                    offset = localMousePosition; // 현재의 마우스 위치를 기록
                    dragStartPos = Input.mousePosition;
                    rb.isKinematic = true; // 물리 계산을 멈춤
                }
            }
        }

        // 드래그 중
        if (isDragging)
        {
            Vector2 localCursor;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, Input.mousePosition, null, out localCursor))
            {
                rectTransform.anchoredPosition = localCursor - offset; // 위치 차이를 유지하면서 이미지를 움직임
            }
        }

        // 드래그 종료
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            dragEndPos = Input.mousePosition;
            Vector2 throwDirection = (dragEndPos - dragStartPos).normalized; // 던지는 방향
            float throwForce = Vector2.Distance(dragStartPos, dragEndPos) / Time.deltaTime; // 던지는 힘
            rb.isKinematic = false; // 물리 계산을 시작
            rb.AddForce(throwDirection * throwForce); // 계산된 방향과 힘으로 던짐
        }

        lastMousePosition = Input.mousePosition; // 마지막 마우스 위치를 기록
    }
}
