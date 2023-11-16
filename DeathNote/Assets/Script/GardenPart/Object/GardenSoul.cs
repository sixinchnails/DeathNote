using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GardenSoul : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float speed = 10.0f;  // 캐릭터의 움직임 속도
    private Vector2 direction;  // 캐릭터가 움직일 방향
    public TextMeshProUGUI soulName;
    public Transform boundaryTransform = null; // 움직임을 제한할 이미지의 Transform
    private Vector2 boundarySize;  // 제한할 이미지의 크기
    private bool isMoving; // 움직이고 있는지
    private bool timer;
    public Canvas userCanvas;
    private bool isPointerDown = false;
    private float pointerDownTimer = 0;

    public bool zoomed;

    private Coroutine myCoroutine;
    public float requiredHoldTime = 3.0f;

    public Transform[] transforms;

    [SerializeField] SpriteRenderer[] sprites;
    public SoulDetail soulDetail;
    GardenCamera gardenCamera;

    // 스프라이트를 바꿔야 하는 요소들
    Animator animator;
    public Soul soul { get; set; }

    void Awake()
    {
        timer = false;

        gardenCamera = FindObjectOfType<GardenCamera>();
        // 스프라이트와 애니메이터 초기화
        animator = GetComponent<Animator>();
        // 움직임을 제한할 이미지의 크기를 SpriteRenderer 컴포넌트를 통해 가져옵니다.

        // 초기 방향을 선택합니다.
        ChooseDirection();

    }

    void OnEnable()
   {
        if (boundaryTransform != null && boundaryTransform.GetComponent<SpriteRenderer>() != null)
        {
            boundarySize = boundaryTransform.GetComponent<SpriteRenderer>().bounds.size;
        }
        if(soul != null)
        {
            animator.SetInteger("body", soul.customizes[0]);
            animator.SetInteger("eyes", soul.customizes[1]);
            animator.SetInteger("bcolor", soul.customizes[2]);
            animator.SetInteger("ecolor", soul.customizes[3]);
            soulName.text = soul.name;
        }

    }

    public void ChangeName(string name)
    {
        soulName.text = name;
        soul.name = name;
        UserManager.instance.SaveData();
    }

    public void ReRender()
    {
        animator.SetInteger("body", soul.customizes[0]);
        animator.SetInteger("eyes", soul.customizes[1]);
        animator.SetInteger("bcolor", soul.customizes[2]);
        animator.SetInteger("ecolor", soul.customizes[3]);
        animator.SetTrigger("spawn");
    }


    void Update()
    {
        if (!timer)
        {
            timer = true;

            int random = UnityEngine.Random.Range(1, 100);

            if (random >= 50)
            {
                isMoving = true;
                ChooseDirection();
                StartCoroutine(Stop());
                StartCoroutine(Moving());
            }

            StartCoroutine(TimeUp());
        }

        // Y 값이 낮을수록, 즉 화면 아래에 있을수록 높은 오더 값을 가집니다.
        // 이 예제에서는 Y 위치에 -1000을 곱하여 오더 값으로 설정합니다.
        // -1000은 오더 값이 충분히 변할 수 있는 범위를 주기 위함입니다.
        // 이 값은 프로젝트의 필요에 따라 조정해야 할 수 있습니다.
        for(int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder = i + Mathf.RoundToInt(transform.position.y * -1000);
        }
       

    }
    IEnumerator TimeUp()
    {
        yield return new WaitForSeconds(2.0f);

        timer = false;
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(1.8f);

        isMoving = false;
    }

    // isMoving동안 움직이는 코루틴 
    IEnumerator Moving()
    {
        animator.SetTrigger("move");

        while (isMoving)
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

            yield return null;
        }

        animator.SetTrigger("idle");
    }


    // 랜덤한 방향을 선택하는 함수입니다.
    void ChooseDirection()
    {

        float h = Random.Range(-1f, 1f);  // 가로 방향 랜덤 값
        float v = Random.Range(-1f, 1f);  // 세로 방향 랜덤 값

        if (h >= 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
            Vector3 canvasScale = userCanvas.transform.localScale;
            canvasScale.x = Mathf.Abs(canvasScale.x);
            userCanvas.transform.localScale = canvasScale;

        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            Vector3 canvasScale = userCanvas.transform.localScale;
            canvasScale.x = -Mathf.Abs(canvasScale.x);
            userCanvas.transform.localScale = canvasScale;
        }

       

        direction = new Vector2(h, v).normalized;  // 방향을 정규화하여 길이가 1이 되도록 합니다.
    }

    public void CameraZoom()
    {
        gardenCamera.SetTarget(transform);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        myCoroutine = StartCoroutine(Countdown());
        gardenCamera.SetTarget(transform);
        soulDetail.OpenBook(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    private void Reset()
    {
        StopCoroutine(myCoroutine);
        isPointerDown = false;
        pointerDownTimer = 0;
    }

    private void OnLongPress()
    {
        SoulManager.instance.jumpSoul = soul;
        SceneManager.LoadScene("PlayScene");
    }

    private IEnumerator Countdown()
    {
        
        while (isPointerDown && pointerDownTimer < requiredHoldTime)
        {
            pointerDownTimer += Time.deltaTime;
            yield return null;
        }

        if (pointerDownTimer >= requiredHoldTime)
        {
            OnLongPress();
        }
    }
}

