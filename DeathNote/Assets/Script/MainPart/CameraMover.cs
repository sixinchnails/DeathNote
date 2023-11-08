using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float speed = 5.0f;               // 카메라의 이동 속도
    public SpriteRenderer backgroundSprite;  // 배경 스프라이트

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

            // 터치 위치에 캐릭터가 있는지 확인합니다.
            if (touch.phase == TouchPhase.Began)
            {
                // 캐릭터가 있으면 이동하지 않습니다.
                if (IsTouchingCharacter(touch.position))
                    return;

                SetMoveDirection(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                moveDirection = Vector3.zero;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 클릭 위치에 캐릭터가 있는지 확인합니다.
                if (IsTouchingCharacter(Input.mousePosition))
                    return;

                SetMoveDirection(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                moveDirection = Vector3.zero;
            }
        }

        MoveCamera();
    }

    // 캐릭터가 터치되었는지 확인하는 메소드
    private bool IsTouchingCharacter(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        return hit.collider != null && hit.collider.CompareTag("Character");
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
