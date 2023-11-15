using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveBug : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector2 direction;
    private float changeDirectionTime = 2.0f;
    public Transform boundaryTransform;
    private Vector2 boundarySize;
    private bool isMoving = true;
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

        // 새로운 방향이 너무 느리지 않도록 최소 크기를 설정합니다.
        Vector2 newDirection = new Vector2(h, v);
        if (newDirection.magnitude < 0.5f) // 최소 크기가 0.5인지 확인합니다.
        {
            newDirection = newDirection.normalized * 0.5f; // 최소 크기를 보장합니다.
        }

        direction = newDirection;
    }
}
