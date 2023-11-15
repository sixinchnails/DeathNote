using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveChar : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector2 direction;
    private float changeDirectionTime = 2.0f;
    public Transform boundaryTransform;
    private Vector2 boundarySize;
    private bool isMoving = true;
    private MoveChar selectedCharacter;
    public Button playButton;
    private bool isDragging = false;
    private Coroutine pulseCoroutine; // Pulse 애니메이션을 위한 코루틴 참조

    void Start()
    {
        if (boundaryTransform.GetComponent<SpriteRenderer>() != null)
        {
            boundarySize = boundaryTransform.GetComponent<SpriteRenderer>().bounds.size;
        }

        ChooseDirection();
        InvokeRepeating("ChooseDirection", changeDirectionTime, changeDirectionTime);
    }

    IEnumerator PulseButtonAnimation(Button button, bool isOverButton)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector3 originalScale = rectTransform.localScale;
        Vector3 minScale = new Vector3(1f, 1f, 1f);
        Vector3 maxScale = new Vector3(1.2f, 1.2f, 1f);

        while (true)
        {
            // 버튼 위에 있을 때는 크기를 고정합니다.
            if (isOverButton)
            {
                rectTransform.localScale = maxScale;
            }
            else
            {
                // 버튼에서 벗어났을 때는 펄스 애니메이션을 실행합니다.
                float scale = Mathf.PingPong(Time.time, maxScale.x - minScale.x) + minScale.x;
                rectTransform.localScale = new Vector3(scale, scale, 1f);
            }
            yield return null;
        }
    }

    void Update()
    {
        //눌렀을 때 캐릭터 멈추게 하기
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0f;

            Collider2D collider = Physics2D.OverlapPoint(new Vector2(touchPosition.x, touchPosition.y));
            if (collider != null && collider.GetComponent<MoveChar>() == this)
            {
                isDragging = true;
                isMoving = false;
                selectedCharacter = this;
                playButton.gameObject.SetActive(true); // 버튼 활성화
                pulseCoroutine = StartCoroutine(PulseButtonAnimation(playButton));
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            DragCharacter();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                isMoving = true;

                // 스크린 좌표로 변경
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(selectedCharacter.transform.position);

                if (IsOverButton(screenPoint, playButton))
                {
                    SceneManager.LoadScene("PlayScene");
                }
                else
                {
                    // PlayBtn 애니메이션을 중지합니다.
                    if (pulseCoroutine != null)
                    {
                        StopCoroutine(pulseCoroutine);
                        pulseCoroutine = null;
                    }
                    playButton.GetComponent<RectTransform>().localScale = Vector3.one; // 버튼 스케일 초기화
                    playButton.gameObject.SetActive(false); // 버튼을 숨깁니다.
                }

                selectedCharacter = null;  // 선택 해제합니다.
            }
        }

        if (isMoving)
        {
            MoveCharacter();
        }
    }


    void DragCharacter()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPosition.z = 0f;
        transform.position = touchPosition;
    }

    bool IsOverButton(Vector2 screenPosition, Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Camera camera = null;

        // Canvas가 Render Mode가 Screen Space - Overlay인지 Screen Space - Camera인지에 따라 
        // 적절한 카메라를 찾아서 넘겨줘야 합니다.
        Canvas canvas = button.GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            camera = null; // Overlay 모드에서는 카메라를 사용하지 않습니다.
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            camera = canvas.worldCamera; // Camera 모드에서는 해당 Canvas의 카메라를 사용합니다.
        }

        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition, camera);
    }

    IEnumerator PulseButtonAnimation(Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        Vector3 minScale = new Vector3(1f, 1f, 1f);
        Vector3 maxScale = new Vector3(1.2f, 1.2f, 1f);
        Vector3 fixedScale = new Vector3(1.2f, 1.2f, 1f); // 고정 크기

        while (true)
        {
            // 드래그 중이고 버튼 위에 있지 않을 때만 애니메이션 실행
            if (isDragging && !IsOverButton(Camera.main.WorldToScreenPoint(selectedCharacter.transform.position), playButton))
            {
                float scale = Mathf.PingPong(Time.time, maxScale.x - minScale.x) + minScale.x;
                rectTransform.localScale = new Vector3(scale, scale, 1f);
            }
            else
            {
                rectTransform.localScale = fixedScale; // 드래그 중이고 버튼 위에 있을 때는 고정 크기
            }

            yield return null;
        }
    }


    void MoveCharacter()
    {
        Vector3 moveAmount = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        Vector3 newPos = transform.position + moveAmount;
        newPos.x = Mathf.Clamp(newPos.x, -boundarySize.x / 2, boundarySize.x / 2);
        newPos.y = Mathf.Clamp(newPos.y, -boundarySize.y / 2, boundarySize.y / 2);
        transform.position = newPos;
    }

    void ChooseDirection()
    {
        float h = Random.Range(-1f, 1f);
        float v = Random.Range(-1f, 1f);
        direction = new Vector2(h, v).normalized;
    }
}