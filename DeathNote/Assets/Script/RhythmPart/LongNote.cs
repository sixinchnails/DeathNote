using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class LongNote : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public Color white = Color.white; // 원래 색상
    public Color transparent; // 목표 색상 (회색)

    private int noteNum = 0;
    // 노트의 생성 위치(x좌표)
    private float posX = 0;
    // 노트의 생성 위치(y좌표)
    private float posY = 0;
    // 거리 측정 기본 단위
    private float deltaX = 0;
    // 노트의 판정 시작 시간
    private double checkTime = 0;
    // 노트의 판정 마감 시간
    private double endTime = 0;
    // 현재 시간
    private double currentTime = 0;
    // 눌러야하는 시간
    private double enabledTime = 0;
    // 노트가 사라지기 전까지의 시간을 구하기 위한 변수(비트당 시간)
    private double timeUnit = 0;
    // 노트의 좌/우
    public bool isLeft;
 

    private bool holding = false;
    private bool isPressed = false;

    private EndNote endNote = null;

    Image image;

    Animator animator;
    EffectController effectController; // 이펙트
    ScoreManager scoreManager; // 점수
    StageManager stageManager; // 스테이지매니저


    void Awake()
    {
        effectController = transform.GetComponentInChildren<EffectController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        stageManager = FindObjectOfType<StageManager>();
        animator = GetComponentInChildren<Animator>();
        image = GetComponentInChildren<Image>();

        // 큐 안에 검사할 중앙 노트의 시간을 넣어둠

        transparent = new Color(white.r, white.g, white.b, 0);

    }
    void OnEnable()
    {
 
        // 투명색으로 설정
        image.color = Color.Lerp(white, transparent, 1);
       
        // 노트의 위치를 변경하고, 스케일을 1로 맞춤
        transform.localPosition = new Vector3 (posX, posY);
        transform.localScale = Vector3.one;

        // 필요한 변수(누르고 있는지 / 눌렀는지) 초기화
        isPressed = false;
        holding = false;



        //if (!isLeft)
        //{
        //    transform.localRotation = Quaternion.Euler(0, 180, 0);
        //}
        //else
        //{
        //    transform.localRotation = Quaternion.Euler(0,0, 0);
        //}

        // 시간 측정, 애니메이터 재생 후 이미지 띄움
        enabledTime = AudioSettings.dspTime;
        animator.Play("Idle");
        image.enabled = true;
    }

    void Update()
    {
        currentTime = AudioSettings.dspTime;
        

        // 누른 적이 없는 상태에서, 정해진 시간보다 0.5비트 이후에 누른경우 
        if (!isPressed && currentTime >= checkTime + 0.5 * timeUnit)
        {
            if (!isPressed)
            {
                isPressed = true;
                effectController.JudgeEffect("Dismiss");
            }

            HideImage();
            endNote.HideImage();
        }

        // 종료타임 1초 뒤에, 노트 풀에 집어넣음
        if (currentTime >= endTime + 1)
        {
            NotePool.instance.longQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
            
            endNote.Finish();

        }


        // 색상 애니메이션
        float lerpValue = (float)((currentTime - enabledTime) / (checkTime - enabledTime));
        lerpValue = Mathf.Clamp01(lerpValue);
        image.color = Color.Lerp(white, transparent, 1 - lerpValue);
    }


    // 노트의 위치와 정보를 넣습니다.
    public void SetNoteInfo(int i, float x, float y, double t, double t2, double t3, bool l, EndNote ed)
    {
        this.noteNum = i;
        this.posX = x;
        this.posY = y;
        this.checkTime = t;
        this.endTime = t2;
        this.timeUnit = t3;
        this.isLeft = l;
        this.endNote = ed;
    }

    public void HideImage()
    {
        image.enabled = false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        holding = true;
        stageManager.noteDown(noteNum, true);
        double pressTime = AudioSettings.dspTime;
        double[] checkList = new double[] { 0.08, 0.13 };

        for (int x = 0; x < checkList.Length; x++)
        {
            if (Math.Abs(pressTime - checkTime) <= checkList[x])
            {
                HideImage();

                effectController.NoteHitEffect();
                if (x == 0) effectController.JudgeEffect("Deadly");
                else effectController.JudgeEffect("Delicate");
                scoreManager.IncreaseCombo(true);
                scoreManager.IncreaseScore(x);

                return;
            }
        }

        effectController.NoteHitEffect();
        effectController.JudgeEffect("Discord");
        scoreManager.IncreaseCombo(false);
        scoreManager.IncreaseScore(2);

        return;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        stageManager.noteDown(noteNum, false);
        double taptime = AudioSettings.dspTime;
        endNote.Check(taptime);
        HideImage();
        endNote.HideImage();
    }
}