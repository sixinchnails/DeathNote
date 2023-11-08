using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float speed = 5.0f;               // 카메라의 이동 속도
    public SpriteRenderer backgroundSprite;  // 배경 스프라이트
    public float minSize = 5.0f; // 카메라의 최소 Orthographic Size
    public float maxSize = 100.0f; // 카메라의 최대 Orthographic Size
    private Camera cam;
    private Vector2 spriteHalfSize;
    private float camHalfHeight;
    private float camHalfWidth;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        cam = GetComponent<Camera>();
        spriteHalfSize = backgroundSprite.bounds.extents;

        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                SetMoveDirection(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                moveDirection = Vector3.zero;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetMoveDirection(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                moveDirection = Vector3.zero;
            }
        }

        MoveCamera();
        HandleZoom();
    }

    void HandleZoom()
    {
        // 마우스 휠 입력을 기반으로 하는 줌
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * speed, minSize, maxSize);
        }

        // 핀치 줌 입력을 기반으로 하는 줌 - 모바일 장치에서 사용
        if (Input.touchCount == 2)
        {
            // 두 터치의 현재 위치와 이전 위치를 가져옵니다.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // 각 위치 사이의 벡터의 크기를 구합니다.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 두 벡터의 크기 차이를 구합니다.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // 카메라의 orthographicSize를 조정합니다.
            cam.orthographicSize += deltaMagnitudeDiff * speed;

            // 카메라의 크기가 최소값 또는 최대값을 넘지 않도록 합니다.
            cam.orthographicSize = Mathf.Max(cam.orthographicSize, minSize);
            cam.orthographicSize = Mathf.Min(cam.orthographicSize, maxSize);
        }
    }

void SetMoveDirection(Vector2 screenPosition)
    {
        Vector2 viewportPosition = cam.ScreenToViewportPoint(screenPosition);
        if (viewportPosition.x < 0.25f) moveDirection = Vector3.left;
        else if (viewportPosition.x > 0.75f) moveDirection = Vector3.right;
        else if (viewportPosition.y < 0.25f) moveDirection = Vector3.down;
        else if (viewportPosition.y > 0.75f) moveDirection = Vector3.up;
        else moveDirection = Vector3.zero;
    }

    void MoveCamera()
    {
        Vector3 moveAmount = moveDirection * speed * Time.deltaTime;
        transform.position += moveAmount;

        // 카메라의 위치를 스프라이트의 경계 내에서 제한
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, -spriteHalfSize.x + camHalfWidth, spriteHalfSize.x - camHalfWidth);
        clampedPosition.y = Mathf.Clamp(transform.position.y, -spriteHalfSize.y + camHalfHeight, spriteHalfSize.y - camHalfHeight);
        transform.position = clampedPosition;
    }
}
