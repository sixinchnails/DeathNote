using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class Note : MonoBehaviour, IPointerDownHandler
{
    private Image noteImage;
    public Color originalColor = Color.white; // 원래 색상
    public Color targetColor = Color.black; // 목표 색상 (회색)

    // 노트의 생성 위치(x좌표)
    private float posX = 0;
    // 노트의 생성 위치(y좌표)
    private float posY = 0;
    // 노트의 판정 시간
    public double checkTime = 0;
    public double currentTime = 0;
    public double enabledTime = 0;
    private double collapseTime = 0;
    public float lerpValue;

    private bool isClicked;

    Transform image;
    EffectController effectController; // 이펙트
    ScoreManager scoreManager; // 점수
    Animator animator;

    void Awake()
    {
        effectController = transform.GetComponentInChildren<EffectController>();
        scoreManager = FindObjectOfType<ScoreManager>();
        animator = GetComponentInChildren<Animator>();
        image = transform.GetChild(0);
    }
    void OnEnable()
    {
        if (noteImage == null)
            noteImage = image.GetComponent<Image>();
        Vector3 newPos = transform.localPosition;

        newPos.x = posX;
        newPos.y = posY;

        image.rotation = Quaternion.Euler(0, 0, 0);
        Color transparentOriginal = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // 원래 색의 투명 버전
        noteImage.color = Color.Lerp(originalColor, transparentOriginal, 1);


        enabledTime = AudioSettings.dspTime;
        transform.localPosition = newPos;
        transform.localScale = Vector3.one;
        animator.Play("Idle");
        isClicked = false;
        noteImage.enabled = true;
    }

    void Update()
    {
        currentTime = AudioSettings.dspTime;
        float lerpValue = (float)((currentTime-enabledTime)/(checkTime - enabledTime));

        if (currentTime >= checkTime + 0.5* collapseTime)
        {
            if (!isClicked)
            {
                isClicked = true;
                effectController.JudgeEffect("Dismiss");
            }

            HideImage();
            
        }

        if (currentTime >= checkTime + 1)
        {
            NotePool.instance.normalQueue.Enqueue(gameObject);
            gameObject.SetActive(false);

        }

        lerpValue = Mathf.Clamp01(lerpValue);


        // 색상 애니메이션
        Color transparentOriginal = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // 원래 색의 투명 버전
        noteImage.color = Color.Lerp(originalColor, transparentOriginal, 1 - lerpValue);


    }


    // 노트의 이미지를 없앱니다.
    public void HideImage()
    {
        noteImage.enabled =  false;
    }

    public void SetNoteInfo(float x, float y, double t, double t2)
    {
        this.posX = x;
        this.posY = y;
        this.checkTime = t;
        this.collapseTime = t2;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        double pressTime = AudioSettings.dspTime;
        double[] checkList = new double[] { 0.06, 0.1 };

        for (int x = 0; x < checkList.Length; x++)
        {   
            if (Math.Abs(pressTime - checkTime) <= checkList[x])
            {
                HideImage();

                effectController.NoteHitEffect();
                if(x==0) effectController.JudgeEffect("Deadly");
                else effectController.JudgeEffect("Delicate");
                scoreManager.IncreaseCombo(true);
                scoreManager.IncreaseScore(x);

                return;
            }
        }

        HideImage();

        effectController.NoteHitEffect();
        effectController.JudgeEffect("Discord");
        scoreManager.IncreaseCombo(false);
        scoreManager.IncreaseScore(2);

        return;

    }


}