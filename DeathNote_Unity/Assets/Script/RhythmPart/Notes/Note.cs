using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class Note : MonoBehaviour   
{
    //private Image image; // 이미지
    //private EffectController effectController; // 이펙트 관리
    //private ScoreManager scoreManager; // 점수 관리

    //private Color white = Color.white; // 흰색
    //private Color transparent; // 투명색

    //private float posX = 0; // 노트의 생성 위치(x좌표)
    //private float posY = 0; // 노트의 생성 위치(y좌표)

    //private double currentTime = 0; // 노트의 현재 시간 
    //private double enabledTime = 0; // 노트의 출현 시간
    //private double timeUnit = 0; // 비트당 시간
    
    //private float lerpValue; // 보간을 위한 수치
    //private bool isClicked; // 눌렀는지 안눌렀는지 판단
    //private int unbreakRatio; // 스킬 중, 콤보 안깨질 확률
    //public double checkTime = 0; // 노트의 판정 기준

    //void Awake()
    //{
    //    effectController = transform.GetComponentInChildren<EffectController>();
    //    scoreManager = FindObjectOfType<ScoreManager>();
    //    image = transform.GetChild(0).GetComponent<Image>();
    //    transparent = new Color(white.r, white.g, white.b, 0); // 투명색상
    //    unbreakRatio = SkillManager.instance.comboUnbreakRatio; // 스킬 객체에서 가져옴
    //}
    
    //void OnEnable()
    //{
    //    image.color = Color.Lerp(white, transparent, 1); // 투명색으로 설정

    //    transform.localPosition = new Vector3(posX, posY); // 노트의 위치를 변경
    //    transform.localScale = Vector3.one; // 스케일 변경

    //    isClicked = false; // 클릭여부 초기화
    //    image.enabled = true; // 이미지 표시
    //    enabledTime = AudioSettings.dspTime; // 시간 초기화
    //}

    //void Update()
    //{
    //    currentTime = AudioSettings.dspTime; // 현재 시간
    //    lerpValue =  Mathf.Clamp01((float)((currentTime-enabledTime)/(checkTime - enabledTime))); // 보간(Clamp는 0~1로 제한)

    //    // 색상 애니메이션
    //    image.color = Color.Lerp(white, transparent, 1 - lerpValue);


    //    // 판정시간의 반비트 뒤에, 클릭이 되지 않았다면 이미지가 사라짐
    //    if (currentTime >= checkTime + 0.5 * timeUnit)
    //    {
    //        if (!isClicked)
    //        {
    //            scoreManager.IncreaseCombo(false); //콤보 제거
    //            isClicked = true; // 클릭되었다고 가정
    //            effectController.JudgeEffect("miss"); // Dismiss 출력
    //        }

    //        HideImage(); // 이미지 감추기
    //    }

    //    // 판정시간의 2비트 뒤에, 노트 풀로 객체보관
    //    if (currentTime >= checkTime + 2 * timeUnit)
    //    {
    //        NotePool.instance.normalQueue.Enqueue(gameObject);
    //        gameObject.SetActive(false); // Active된 것을 false로 돌려놓음

    //    }
    //}


    //// 노트의 이미지를 없앰
    //public void HideImage()
    //{
    //    image.enabled =  false;
    //}

    //// 노트의 정보를 추가
    //public void SetNoteInfo(float x, float y, double t, double u)
    //{
    //    this.posX = x;
    //    this.posY = y;
    //    this.checkTime = t;
    //    this.timeUnit = u;
    //}

    //// 노트를 클릭했을 때 메서드
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    isClicked = true; // 클릭되었다고 표시
    //    HideImage(); // 이미지 숨김
    //    double pressTime = AudioSettings.dspTime; // 누른 시간 측정
    //    double[] checkList = new double[] { 0.06, 0.12 }; // deadly, delicate 판정 기준

    //    for (int x = 0; x < 2; x++)
    //    {   
    //        // 누른 시간과 실제시간과의 차이가 판정 기준보다 작을 경우
    //        if (Math.Abs(pressTime - checkTime) <= checkList[x])
    //        {
    //            scoreManager.IncreaseCombo(true); // 콤보 추가
    //            scoreManager.IncreaseScore(x, false); // 점수 추가
    //            effectController.NoteHitEffect(); // 타격이펙트 출력
    //            if(x==0) effectController.JudgeEffect("perfect");
    //            else effectController.JudgeEffect("good");


    //            return;
    //        }
    //    }

    //    int random = UnityEngine.Random.Range(1, 100);
    //    if(unbreakRatio >= random)
    //    {
    //        scoreManager.IncreaseScore(2, true); // 점수는 break로 줌
    //        effectController.NoteHitEffect();
    //        effectController.JudgeEffect("good"); // 이펙트는 good으로
            
    //    }
    //    scoreManager.IncreaseCombo(false); //콤보 제거
    //    scoreManager.IncreaseScore(2, false);
    //    effectController.NoteHitEffect();
    //    effectController.JudgeEffect("break");
    //    return;
    //}
}