using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class ClickNote : MonoBehaviour, IPointerDownHandler
{
    public int noteNum; // 노트번호
    private Image image; // 이미지
    private Animator animator; // 노트 애니메이션
    public EffectController effectController; // 이펙트 관리
    private ScoreManager scoreManager; // 점수 관리

    private double currentTime = 0; // 노트의 현재 시간 
    private double enabledTime = 0; // 노트의 출현 시간
    private double timeUnit = 0; // 비트당 시간
    public float speed;

    private float lerpValue; // 보간을 위한 수치
    private bool isClicked; // 눌렀는지 안눌렀는지 판단
    // private int unbreakRatio; // 스킬 중, 콤보 안깨질 확률
    public double checkTime = 0; // 노트의 판정 기준

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        image = transform.GetComponent<Image>();
        animator = transform.GetComponent<Animator>();
    }

    void OnEnable()
    {
        transform.localScale = Vector3.one; // 스케일 변경
        isClicked = false; // 클릭여부 초기화
        enabledTime = AudioSettings.dspTime; // 시간 초기화
        animator.speed = speed; 
    }

    void Update()
    {
        currentTime = AudioSettings.dspTime; // 현재 시간
        // lerpValue = Mathf.Clamp01((float)((currentTime - enabledTime) / (checkTime - enabledTime))); // 보간(Clamp는 0~1로 제한)

        // 판정시간의 반비트 뒤에, 클릭이 되지 않았다면 이미지가 사라짐
        if (currentTime >= checkTime + 0.25 * timeUnit)
        {
            scoreManager.IncreaseCombo(false); //콤보 제거
            isClicked = true; // 클릭되었다고 가정
            effectController.JudgeEffect("miss"); // Dismiss 출력
            SetActive(false);
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    // 노트의 정보를 추가
    public void SetNoteInfo(double t, double u)
    {
        this.checkTime = t;
        this.timeUnit = u;
    }

    float EvaluatePress(float pressTime)
    {
        // checkTime과 pressTime 사이의 절대 차이 계산
        float absDifference = Mathf.Abs((float)(checkTime - pressTime));

        // 허용된 최대 차이 시간 설정
        float maxDifference = (float)(1/speed);

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

    // 노트를 클릭했을 때 메서드
    public void OnPointerDown(PointerEventData eventData)
    {
        
        isClicked = true; // 클릭되었다고 표시
        double pressTime = AudioSettings.dspTime; // 누른 시간 측정
        double bestTime = 0.1; // 최고 판정 기준
        if (Math.Abs(pressTime - checkTime) <= bestTime)
        {
      
            scoreManager.IncreaseCombo(true); // 콤보 추가
            scoreManager.IncreaseScore(100); // 점수 추가
            effectController.NoteHitEffect(); // 타격이펙트 출력
            effectController.JudgeEffect("perfect");
            SetActive(false);
            
            return;
        }

        else
        {
            float lerpValue = EvaluatePress((float)pressTime);
            // 1초 이내인 경우
                scoreManager.IncreaseCombo(true); // 콤보 추가
                scoreManager.IncreaseScore((int)(lerpValue*100)); // 점수는 break로 줌
                effectController.NoteHitEffect();
                effectController.JudgeEffect("good"); // 이펙트는 good으로
                SetActive(false);
                return;
        }
    }
}
