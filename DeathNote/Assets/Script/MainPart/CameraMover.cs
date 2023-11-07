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
