using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 네임스페이스 추가
using UnityEngine.SceneManagement;

public class MoveChar : MonoBehaviour
{
    public float speed = 10.0f;  // 캐릭터의 움직임 속도
    private Vector2 direction;  // 캐릭터가 움직일 방향
    private float changeDirectionTime = 2.0f;  // 방향을 바꾸는 시간 간격
    public Transform boundaryTransform; // 움직임을 제한할 이미지의 Transform
    private Vector2 boundarySize;  // 제한할 이미지의 크기
    private bool isMoving = true; // 캐릭터가 움직이고 있는지 추적하는 변수
    private MoveChar selectedCharacter; // 현재 터치 또는 클릭으로 선택된 캐릭터를 추적합니다.
    public Button playButton; // UI의 PlayBtn 버튼 참조
    private bool isDragging = false; // 드래그 상태를 추적하는 변수

    void Start()
    {
        // 움직임을 제한할 이미지의 크기를 SpriteRenderer 컴포넌트를 통해 가져옵니다.
        if (boundaryTransform.GetComponent<SpriteRenderer>() != null)
        {
            boundarySize = boundaryTransform.GetComponent<SpriteRenderer>().bounds.size;
        }

        // 초기 방향을 선택합니다.
        ChooseDirection();

        // 2초마다 방향을 바꾸도록 설정합니다.
        InvokeRepeating("ChooseDirection", changeDirectionTime, changeDirectionTime);
    }

    void Update()
    {
        // PC에서의 마우스 클릭 또는 모바일에서의 터치를 감지합니다.
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 터치 위치를 화면 상의 좌표로 변환합니다.
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0f; // 2D 게임이므로 Z 위치는 변경하지 않습니다.

            // 터치된 위치에 캐릭터가 있는지 확인합니다.
            Collider2D collider = Physics2D.OverlapPoint(new Vector2(touchPosition.x, touchPosition.y));
            if (collider != null && collider.GetComponent<MoveChar>() == this) // 확인: 현재 스크립트 인스턴스에 대응하는 객체를 터치했는지
            {
                isDragging = true;
                isMoving = false; // 드래그 중이면 자동 움직임을 중단합니다.
                selectedCharacter = this; // 현재 캐릭터를 선택합니다.
                                          // PlayBtn 애니메이션을 시작합니다.
                StartCoroutine(StartButtonAnimation(playButton, true));
            }
        }

        if (isDragging)
        {
            // 드래그하는 동안 캐릭터를 마우스 또는 터치 위치로 이동시킵니다.
            DragCharacter();
        }

        // 마우스 버튼이 떼어졌을 때의 로직...
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            isMoving = true; // 드래그가 끝났으므로 움직임을 다시 허용합니다.

            // 버튼 위에 드래그가 끝났는지 확인합니다. 여기서 selectedCharacter.transform.position을 그대로 넘기는 대신,
            // Camera.main.WorldToScreenPoint를 사용하여 스크린 좌표로 변환해야 합니다.
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(selectedCharacter.transform.position);
            if (IsOverButton(screenPoint, playButton))
            {
                // 씬 전환 로직
                SceneManager.LoadScene("PlayScene");
            }
            else
            {
                // PlayBtn 애니메이션을 중지합니다.
                StartCoroutine(StartButtonAnimation(playButton, false));
            }

            selectedCharacter = null;  // 선택 해제합니다.
        }


        // 캐릭터가 움직일 수 있도록 허용된 경우에만 움직임을 계산합니다.
        if (isMoving)
        {
            MoveCharacter();
        }
    }


    void DragCharacter()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPosition.z = 0f; // 2D 게임이므로 Z 위치는 변경하지 않습니다.
        transform.position = touchPosition;
    }


    // 캐릭터가 버튼 위에 있는지 확인하는 함수
    bool IsOverButton(Vector3 characterPosition, Button button)
    {
        // 월드 좌표를 스크린 좌표로 변환합니다.
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(characterPosition);

        // UI의 RectTransform을 가져옵니다.
        RectTransform rectTransform = button.GetComponent<RectTransform>();

        // 스크린 좌표와 UI의 RectTransform을 사용하여 버튼 위에 있는지 확인합니다.
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint, Camera.main);
    }


    IEnumerator StartButtonAnimation(Button button, bool show)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        float time = 0.25f; // 애니메이션에 걸리는 시간
        float currentTime = 0f;
        Vector3 startScale = show ? Vector3.zero : Vector3.one;
        Vector3 endScale = show ? Vector3.one : Vector3.zero;

        if (show)
        {
            button.gameObject.SetActive(true);
        }

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / time;
            rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        rectTransform.localScale = endScale;

        if (!show)
        {
            button.gameObject.SetActive(false);
        }
    }

    void MoveCharacter()
    {
        // 움직일 양을 계산합니다.
        Vector3 moveAmount = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;

        // 새로운 위치를 계산합니다.
        Vector3 newPos = transform.position + moveAmount;

        // 캐릭터가 이미지를 넘어가지 않도록 x와 y 위치를 조절합니다.
        newPos.x = Mathf.Clamp(newPos.x, -boundarySize.x / 2, boundarySize.x / 2);
        newPos.y = Mathf.Clamp(newPos.y, -boundarySize.y / 2, boundarySize.y / 2);

        // 캐릭터의 위치를 업데이트합니다.
        transform.position = newPos;
    }
    
    // 랜덤한 방향을 선택하는 함수입니다.
        void ChooseDirection()
    {
        float h = Random.Range(-1f, 1f);  // 가로 방향 랜덤 값
        float v = Random.Range(-1f, 1f);  // 세로 방향 랜덤 값
        direction = new Vector2(h, v).normalized;  // 방향을 정규화하여 길이가 1이 되도록 합니다.
    }
}