using System;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class ClickNote : MonoBehaviour, IPointerDownHandler
{
    //ㅎㅇㅎㅇ
    public int noteNum; // 노트번호
    public Animator animator; // 노트 애니메이터
    private NotePool notePool; // 연결된 노트풀
    public Effect effect; // 연결된 이펙터

    private double currentTime = 0; // 현재 시간 
    public double checkTime = 0; // 노트의 정확한 판정 시간
    private double timeUnit = 0; // 비트당 시간
    public float speed = 1; // 노트의 스피드

    public int bonus; // 보너스 점수
    public int combo; // 콤보 보너스
    public int perfect; // 퍼펙트 보너스

    void OnEnable()
    {
        transform.localScale = Vector3.one; // 스케일 변경

    }

    void Update()
    {
        currentTime = AudioSettings.dspTime; // 현재 시간

        // 판정시간의 반비트 뒤에, 클릭이 되지 않았다면 이미지가 사라짐
        if (currentTime >= checkTime + 0.25 * timeUnit)
        {
            ScoreManager.instance.IncreaseCombo(false); //콤보 제거
            effect.JudgeEffect("miss"); // 놓쳤다고 출력
            SetActive(false);// 노트 비활성화 
            notePool.clickQueue.Enqueue(gameObject); // 오브젝트를 다시 풀에 넣음
        }
    }

    public void SetActive(bool status)
    {
        gameObject.SetActive(status);


    }

    // 노트에 이펙트와 노트 풀을 연결
    public void SetNoteInfo(Effect effect, NotePool pool, double t, double u)
    {
        this.effect = effect;
        this.notePool = pool;
        this.checkTime = t;
        this.timeUnit = u;
    }

    // 노트를 클릭했을 때의 메서드
    public void OnPointerDown(PointerEventData eventData)
    {

        double pressTime = AudioSettings.dspTime; // 누른 시간 측정
        double bestTime = 0.08; // 최고 판정 기준 : 0.1초
        if (Math.Abs(pressTime - checkTime) <= bestTime)
        {
            ScoreManager.instance.IncreaseCombo(true); // 콤보 추가
            ScoreManager.instance.IncreaseScore(100); // 점수 추가
            effect.NoteHitEffect(); // 타격 이펙트 출력
            effect.JudgeEffect("perfect"); // 판정 이벤트 출력
            SetActive(false); // 노트 비활성화

            notePool.clickQueue.Enqueue(gameObject); // 오브젝트를 다시 풀에 넣음
            
        }

        else
        {
            float lerpValue = EvaluatePress((float)pressTime); // 판정 시간의 정확도를 계산
            Debug.Log("러프밸류:" + lerpValue);
            if (lerpValue < 0.1)
            {
                ScoreManager.instance.IncreaseCombo(false); //콤보 제거
                effect.JudgeEffect("miss"); // 놓쳤다고 출력
                SetActive(false);// 노트 비활성화 
                notePool.clickQueue.Enqueue(gameObject); // 오브젝트를 다시 풀에 넣음

            }
            else
            {
                ScoreManager.instance.IncreaseCombo(true); // 콤보 추가
                ScoreManager.instance.IncreaseScore((int)(lerpValue * 100)); // 점수는 계산에 따라
                effect.NoteHitEffect(); // 판정 이벤트 출력
                effect.JudgeEffect("good"); // 이펙트는 good으로
                SetActive(false); // 노트 비활성화
                notePool.clickQueue.Enqueue(gameObject); // 오브젝트를 다시 풀에 넣음
            }

        }
    }

    // 판정 정확도 계산 메서드
    float EvaluatePress(float pressTime)
    {
        // checkTime과 pressTime 사이의 절대 차이 계산
        float absDifference = Mathf.Abs((float)(checkTime - pressTime));

        // 허용된 최대 차이 시간 설정
        float maxDifference = (float)(0.5 * timeUnit);
        // absDifference가 maxDifference 이내일 경우 보간값 계산, 그렇지 않으면 0
        if (absDifference <= maxDifference)
        {
            // absDifference가 0이면 완벽하게 눌렀으므로 1 반환
            // absDifference가 maxDifference와 같으면 가장 낮은 점수인 0 반환
            // 그 사이의 값은 선형 보간으로 계산
            return 1 - (absDifference / maxDifference);
        }
        else
        {
            // 허용된 최대 차이를 벗어났으므로 0 반환
            return 0;
        }
    }
}
