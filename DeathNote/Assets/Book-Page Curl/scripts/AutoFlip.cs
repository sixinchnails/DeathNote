using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// 필요한 컴포넌트를 자동으로 추가합니다.
[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour
{
    // 페이지 넘기는 방식 (오른쪽으로 넘기거나 왼쪽으로 넘기거나)
    public FlipMode Mode;

    // 각 변수의 시간 설정
    public float PageFlipTime = 1;           // 페이지를 넘기는 데 걸리는 시간
    public float TimeBetweenPages = 1;       // 페이지를 넘길 때마다 기다리는 시간
    public float DelayBeforeStarting = 0;    // 시작하기 전에 기다리는 시간
    public bool AutoStartFlip = true;          // 시작할 때 자동으로 페이지를 넘길지의 여부
    public Book ControledBook;               // 제어하는 책 객체
    public int AnimationFramesCount = 40;    // 애니메이션 프레임 수
    bool isFlipping = false;                 // 현재 페이지를 넘기는 중인지 여부
    public Text rightPageNumberText;
    // 시작할 때 실행되는 함수
    void Start()
    {
        // 제어하는 책 객체를 설정

        if (!ControledBook)
            ControledBook = GetComponent<Book>();
        if (AutoStartFlip)
            StartFlipping();
        // 페이지를 넘길 때마다 이벤트 리스너를 추가
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
    }

    // 페이지가 넘어갔을 때 호출되는 함수
    void PageFlipped()
    {
        isFlipping = false;  // 넘기기 상태를 false로 변경
        UpdatePageNumber();
    }

    // 페이지 번호를 갱신하는 함수
    void UpdatePageNumber()
    {
        if (rightPageNumberText)
        {
            int pageNumber = (ControledBook.currentPage / 2) + 1;
            rightPageNumberText.text = pageNumber.ToString();
        }
    }


    // 페이지 넘기기를 시작하는 함수
    public void StartFlipping()
    {
        StartCoroutine(FlipToEnd()); // 코루틴으로 페이지를 끝까지 넘김
    }

    // 오른쪽 페이지를 넘기는 함수
    public void FlipRightPage()
    {
        // 이미 넘기는 중이거나 마지막 페이지인 경우 리턴
        if (isFlipping) return;
        if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;

        isFlipping = true;
        // 여기서는 페이지를 넘기는데 필요한 좌표와 시간을 계산
        // 그 후에 FlipRTL 코루틴을 시작
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl)*2 / AnimationFramesCount;
        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
    }

    // 왼쪽 페이지를 넘기는 함수
    public void FlipLeftPage()
    {
        // 이미 넘기는 중이거나 첫 페이지인 경우 리턴
        if (isFlipping) return;
        if (ControledBook.currentPage <= 0) return;

        isFlipping = true;
        // 여기서는 페이지를 넘기는데 필요한 좌표와 시간을 계산
        // 그 후에 FlipLTR 코루틴을 시작
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
    }
    // 페이지를 끝까지 넘기는 함수 (코루틴)
    IEnumerator FlipToEnd()
    {
        // 시작 전에 기다림
        yield return new WaitForSeconds(DelayBeforeStarting);

        // 페이지를 넘기는데 필요한 좌표와 시간 계산
        float frameTime = PageFlipTime / AnimationFramesCount;
        // 설정된 모드에 따라 오른쪽 또는 왼쪽으로 페이지를 계속 넘김
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2)*0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y)*0.9f;
        //y=-(h/(xl)^2)*(x-xc)^2          
        //               y         
        //               |          
        //               |          
        //               |          
        //_______________|_________________x         
        //              o|o             |
        //           o   |   o          |
        //         o     |     o        | h
        //        o      |      o       |
        //       o------xc-------o      -
        //               |<--xl-->
        //               |
        //               |
        float dx = (xl)*2 / AnimationFramesCount;
        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount)
                {
                    StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 0)
                {
                    StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
        }
    }
    // 오른쪽으로 페이지를 넘기는 코루틴
    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        // 여기서는 페이지를 넘기는 애니메이션을 처리
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();
    }
    // 왼쪽으로 페이지를 넘기는 코루틴
    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
    {
        // 여기서는 페이지를 넘기는 애니메이션을 처리

        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();
    }
}
