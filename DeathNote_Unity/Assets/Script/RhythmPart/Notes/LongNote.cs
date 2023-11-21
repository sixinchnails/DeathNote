using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class LongNote : MonoBehaviour
{
    //public Image image; // 이미지
    //private EffectController effectController; // 이펙트 관리
    //private ScoreManager scoreManager; // 점수 관리
    //Animator animator; // 애니메이터
    //public List<CenterNote> gauges; // 게이지 노트의 리스트

    //public Color white = Color.white; // 원래 색상
    //public Color transparent; // 목표 색상 (회색)

    //private float posX = 0; // 노트의 생성 위치(x좌표)
    //private float posY = 0; // 노트의 생성 위치(y좌표)

    //private double currentTime = 0; // 노트의 현재 시간 
    //private double enabledTime = 0; // 노트의 출현 시간
    //private double timeUnit = 0; // 비트당 시간

    //public double checkTime = 0; // 노트의 판정 기준
    //public double endTime = 0; // 노트의 판정 마감 시간

    //private float lerpValue; // 보간을 위한 수치
    //private bool isClicked; // 눌렀는지 안눌렀는지 판단
    //private bool holding = false; // 누르고 있는지 아닌지 판단
    //private int unbreakRatio; // 스킬 중, 콤보 안깨질 확률

    //private int scoreSum; // 판정용 수치

    //void Awake()
    //{
    //    effectController = transform.GetComponentInChildren<EffectController>();
    //    scoreManager = FindObjectOfType<ScoreManager>();
    //    image = transform.GetChild(0).GetComponent<Image>();
    //    transparent = new Color(white.r, white.g, white.b, 0); // 투명색상
    //    unbreakRatio = SkillManager.instance.comboUnbreakRatio; // 스킬 객체에서 가져옴
    //    animator = transform.GetChild(0).GetComponent<Animator>();

    //}
    //void OnEnable()
    //{

    //    image.color = Color.Lerp(white, transparent, 1); // 투명색으로 설정

    //    transform.localPosition = new Vector3(posX, posY); // 노트의 위치를 변경
    //    transform.localScale = Vector3.one; // 스케일 변경

    //    isClicked = false; // 클릭여부 초기화
    //    holding = false; // 홀드여부 초기화
    //    scoreSum = 0; // 판정용 수치 초기화
    //    image.enabled = true; // 이미지 표시
    //    enabledTime = AudioSettings.dspTime; // 시간 초기화

    //}

    //void Update()
    //{
    //    currentTime = AudioSettings.dspTime; // 현재 시간
    //    lerpValue = Mathf.Clamp01((float)((currentTime - enabledTime) / (checkTime - enabledTime))); // 보간(Clamp는 0~1로 제한)

    //    // 색상 애니메이션
    //    image.color = Color.Lerp(white, transparent, 1 - lerpValue);


    //    // 판정시간의 2비트 뒤에, 노트 풀로 객체보관
    //    if (currentTime >= endTime + 2 * timeUnit)
    //    {
    //        NotePool.instance.longQueue.Enqueue(gameObject);
    //        gameObject.SetActive(false); // Active된 것을 false로 돌려놓음

    //    }

    //    // 누른 적이 없는 상태에서, 정해진 시간보다 0.5비트 이후에 누른경우 
    //    else if (!isClicked && currentTime >= checkTime + 0.5 * timeUnit)
    //    {

    //        scoreManager.IncreaseCombo(false); //콤보 제거
    //        isClicked = true; // 클릭되었다고 가정
    //        effectController.JudgeEffect("miss"); // Dismiss 출

    //        HideImage(); // 이미지 감추기
    //    }

    //    // 누르고 있는 상태에서, 정해진 시간보다 0.5비트 이후에 누른경우 
    //    else if (holding && currentTime >= endTime + 0.5 * timeUnit)
    //    {
    //        // 만약 제대로 된 판정이 아닌 경우 롱노트는 그대로 사라짐
    //        holding = !holding;
    //        HideImage();

    //        // 스킬 발동시에 콤보는 안끊기게
    //        int random = UnityEngine.Random.Range(1, 100);

    //        if (unbreakRatio >= random)
    //        {
    //            scoreManager.IncreaseScore(2, true); // 점수는 break로 줌
    //            effectController.NoteHitEffect();
    //            effectController.JudgeEffect("good"); // 이펙트는 good으로

    //        }
    //        scoreManager.IncreaseCombo(false); //콤보 제거
    //        scoreManager.IncreaseScore(2, false); // 점수는 break로 줌
    //        effectController.NoteHitEffect();
    //        effectController.JudgeEffect("break");
    //        return;
    //    }

    //}


    //// 노트의 위치와 정보를 넣습니다.
    //public void SetNoteInfo(float x, float y, double t, double t2, double u)
    //{
    //    this.posX = x;
    //    this.posY = y;
    //    this.checkTime = t;
    //    this.endTime = t2;
    //    this.timeUnit = u;
    //}

    //public void HideImage()
    //{
    //    Debug.Log("ㅎㅇ");
    //    image.enabled = false;
    //    foreach (CenterNote note in gauges) // 게이 노트도 같이 없애줌
    //    {
    //        note.Finish();
    //    }
    //}

    //// 클릭 포인터가 눌렸을 때 발동
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    animator.SetTrigger("hold");
    //    isClicked = true; // 눌러졌다고 표시
    //    holding = true; // 누르고있다고 표시
    //    double pressTime = AudioSettings.dspTime; // 누른 시간 측정
    //    double[] checkList = new double[] { 0.06, 0.12 };

    //    for (int x = 0; x < checkList.Length; x++)
    //    {
    //        if (Math.Abs(pressTime - checkTime) <= checkList[x])
    //        {
    //            animator.SetTrigger("hold"); // 누르는 동안은, scoreSum만 올려주고 애니메이션을 실행한 뒤 빠져나감
    //            scoreSum += x;

    //            return;
    //        }
    //    }
    //    // 만약 제대로 된 판정이 아닌 경우 롱노트는 그대로 사라짐
    //    HideImage();

    //    scoreSum = 3;

    //    // 스킬 발동시에 콤보는 안끊기게
    //    int random = UnityEngine.Random.Range(1, 100);
    //    if (unbreakRatio >= random)
    //    {
    //        scoreManager.IncreaseScore(2, true); // 점수는 break로 줌
    //        effectController.NoteHitEffect();
    //        effectController.JudgeEffect("good"); // 이펙트는 good으로

    //    }
    //    scoreManager.IncreaseCombo(false); //콤보 제거
    //    scoreManager.IncreaseScore(2, false); // 점수는 break로 줌
    //    effectController.NoteHitEffect();
    //    effectController.JudgeEffect("break");
    //    return;

    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    if (scoreSum == 3) return;
    //    HideImage(); // 뗄 때는 이미지는 없어짐

    //    holding = false; // 누르고있다고 표시
    //    double finishTime = AudioSettings.dspTime; // 누른 시간 측정
    //    double[] checkList = new double[] { 0.12 };

    //    for (int x = 0; x < checkList.Length; x++)
    //    {
    //        if (Math.Abs(finishTime - endTime) <= checkList[x])
    //        {

    //            if (scoreSum <= 1)
    //            {
    //                scoreManager.IncreaseCombo(true); // 콤보 추가
    //                scoreManager.IncreaseScore(0, false); // 점수 추가
    //                effectController.NoteHitEffect(); // 타격이펙트 출력
    //                effectController.JudgeEffect("perfect");

    //                return;
    //            }

    //            else if (scoreSum <= 2)
    //            {
    //                scoreManager.IncreaseCombo(true); // 콤보 추가
    //                scoreManager.IncreaseScore(1, false); // 점수 추가
    //                effectController.NoteHitEffect(); // 타격이펙트 출력
    //                effectController.JudgeEffect("good");

    //                return;
    //            }
    //        }
    //    }

    //    // 스킬 발동시에 콤보는 안끊기게
    //    int random = UnityEngine.Random.Range(1, 100);

    //    if (unbreakRatio >= random)
    //    {
    //        scoreManager.IncreaseScore(2, true); // 점수는 break로 줌
    //        effectController.NoteHitEffect();
    //        effectController.JudgeEffect("good"); // 이펙트는 good으로

    //    }
    //    scoreManager.IncreaseCombo(false); //콤보 제거
    //    scoreManager.IncreaseScore(2, false); // 점수는 break로 줌
    //    effectController.NoteHitEffect();
    //    effectController.JudgeEffect("break");
    //    return;

    //}
}