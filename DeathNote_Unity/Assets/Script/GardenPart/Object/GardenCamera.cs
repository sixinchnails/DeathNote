using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GardenCamera : MonoBehaviour
{
    public float speed = 5.0f;               // 카메라의 이동 속도
    public SpriteRenderer backgroundSprite;  // 배경 스프라이트
    public float minSize = 10.0f; // 카메라의 최소 Orthographic Size
    public float maxSize = 80.0f; // 카메라의 최대 Orthographic Size
    private Camera cam;
    private Vector2 spriteHalfSize;
    private float camHalfHeight;
    private float camHalfWidth;
    private Vector3 moveDirection = Vector3.zero;

    public Transform followTarget; // 따라갈 대상
    public bool isFollowing = false; // 대상을 따라가는지 여부

    private Vector3 lastMousePosition;
    private bool isDragging = false; // 드래그 중인지 여부

    void Start()
    {
        cam = GetComponent<Camera>();
        spriteHalfSize = backgroundSprite.bounds.extents;

        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
    }

    void Update()
    {
        // isFollowing이 활성화가 되어있다면, 객체를 따라서 카메라가 이동.
        if (isFollowing && followTarget != null)
        {
            Vector3 targetPosition = followTarget.position; // 타겟의 포지션을 얻음
            targetPosition.z = transform.position.z; // 카메라의 뎁스는 변하지 않음
            cam.orthographicSize = 35.0f; // 캠의 사이즈를 위아래 35유닛으로
            transform.position = targetPosition; // 카메라의 포지션을 바꿈
        }

        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치가 처음감지되었을 때 그 지점을 저장하고 Drag활성


            if (touch.phase == TouchPhase.Began)
            {
                // 캐릭터가 있으면 이동하지 않습니다.
                if (IsTouchingCharacter(touch.position) || IsClickingUI(touch.position))
                    return;
                lastMousePosition = cam.ScreenToWorldPoint(touch.position);
                isDragging = true;

                
            }
            // 만약 터치가 끝나거나 취소되면 Drag비활성
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }

            HandleTouchCameraDrag(touch);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
    
                // 터치를 한 곳이 캐릭터거나 UI면 이 스크립트랑 무관
                if (!isDragging && (IsTouchingCharacter(Input.mousePosition) || IsClickingUI(Input.mousePosition)))
                    return;
                lastMousePosition = cam.ScreenToViewportPoint(Input.mousePosition);
                isDragging = true;

            }
            
        // 마우스를 떼면 Drag 비활성화
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            HandleCameraDrag();
        }

        HandleZoom();
    }
    
    // 카메라를 따라갑니다.
    public void SetTarget(Transform target)
    {
        if(!isFollowing)
        {
            isFollowing = true;
            followTarget = target;
        }

        else
        {
            if(followTarget.Equals(target))
            {
                isFollowing = false;
                followTarget = null;
            }
            else
            {
                followTarget = target;
            }
            
        }
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

            camHalfHeight = cam.orthographicSize;
            camHalfWidth = camHalfHeight * cam.aspect;
        }
    }

    // 캐릭터가 터치되었는지 확인하는 메소드
    private bool IsTouchingCharacter(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        return hit.collider != null && hit.collider.CompareTag("Character");
    }

    private bool IsClickingUI(Vector2 screenPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }


    void HandleCameraDrag()
    {
        // 첫번째 마우스 다운인 경우 그 지점을 저장하고 Drag 활성

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 currentTouchPosition = cam.ScreenToViewportPoint(Input.mousePosition); // 현재 위치
            Vector3 difference = lastMousePosition - currentTouchPosition; // 위치차이
            
            // 카메라의 위치를 범위밖으로 나가지 않게 보정
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(transform.position.x + difference.x, -spriteHalfSize.x + camHalfWidth, spriteHalfSize.x - camHalfWidth);
            clampedPosition.y = Mathf.Clamp(transform.position.y + difference.y, -spriteHalfSize.y + camHalfHeight, spriteHalfSize.y - camHalfHeight);
            transform.position = clampedPosition;

            
        }
    }
    void HandleTouchCameraDrag(Touch touch)
    {
       

        if (isDragging && touch.phase == TouchPhase.Moved)
        {
            Vector3 currentTouchPosition = cam.ScreenToWorldPoint(touch.position); // 현재 위치
            Vector3 difference = lastMousePosition - currentTouchPosition; // 위치차이
            transform.position += difference; // 캠의 위치 변경

            // 카메라의 위치를 범위밖으로 나가지 않게 보정
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(transform.position.x, -spriteHalfSize.x + camHalfWidth, spriteHalfSize.x - camHalfWidth);
            clampedPosition.y = Mathf.Clamp(transform.position.y, -spriteHalfSize.y + camHalfHeight, spriteHalfSize.y - camHalfHeight);
            transform.position = clampedPosition;
        }
    }
}
