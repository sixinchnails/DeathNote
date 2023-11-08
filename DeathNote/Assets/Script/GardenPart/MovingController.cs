using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingController : MonoBehaviour
{
    public float speed = 10.0f;  // 캐릭터의 움직임 속도
    private Vector2 direction;  // 캐릭터가 움직일 방향
    private float changeDirectionTime = 2.0f;  // 방향을 바꾸는 시간 간격
    public Transform boundaryTransform; // 움직임을 제한할 이미지의 Transform
    private Vector2 boundarySize;  // 제한할 이미지의 크기
    private bool isMoving; // 움직이고 있는지
    private bool timer;
    Animator animator;
    
    void Start()
    {
        timer = false;
        animator = GetComponent<Animator>();
        // 움직임을 제한할 이미지의 크기를 SpriteRenderer 컴포넌트를 통해 가져옵니다.
        if (boundaryTransform.GetComponent<SpriteRenderer>() != null)
        {
            boundarySize = boundaryTransform.GetComponent<SpriteRenderer>().bounds.size;
        }

        // 초기 방향을 선택합니다.
        ChooseDirection();

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

    }
    IEnumerator TimeUp()
    {
        yield return new WaitForSeconds(2.0f);

        timer = false;
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(2.0f);

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
        
        if(h >= 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        direction = new Vector2(h, v).normalized;  // 방향을 정규화하여 길이가 1이 되도록 합니다.
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class MovingController : MonoBehaviour
//{
//    [SerializeField] RectTransform background; // 배경
//    Animator animator; // 
//    float unitX; // 움직이는 범위
//    bool isMoving; // 현재 움직이고 있는지
//    bool isAnimating; // 애니메이션이 재생중인지
//    // Start is called before the first frame update
//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        unitX = background.rect.width / 10;
//        animator.SetTrigger("Idle");
//    }

//    // Update is called once per frame

//}
