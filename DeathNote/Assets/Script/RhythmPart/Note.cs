using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class WideNote : MonoBehaviour, IPointerDownHandler
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

    Transform image;
    EffectManager effectManager; // 이펙트
    ScoreManager scoreManager; // 점수
    Animator[] animators;

    void Awake()
    {
        effectManager = transform.GetComponentInChildren<EffectManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        image = transform.GetChild(2);
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

        float startScale = 0.5f; // 시작 크기
        float endScale = 1f; // 끝 크기
        float currentScale = Mathf.Lerp(startScale, endScale, 0);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        enabledTime = AudioSettings.dspTime;
        transform.localPosition = newPos;

        noteImage.enabled = true;
    }

    void Update()
    {
        currentTime = AudioSettings.dspTime;
        float lerpValue = (float)((currentTime-enabledTime)/(checkTime - enabledTime));

        if (currentTime >= checkTime + collapseTime)
        {
                NotePool.instance.noteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
            
        }

        lerpValue = Mathf.Clamp01(lerpValue);

        // 회전 애니메이션
        image.rotation = Quaternion.Euler(0, 0, lerpValue * 360f);

        // 색상 애니메이션
        Color transparentOriginal = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // 원래 색의 투명 버전
        noteImage.color = Color.Lerp(originalColor, transparentOriginal, 1 - lerpValue);

        float startScale = 0.5f; // 시작 크기
        float endScale = 1f; // 끝 크기
        float currentScale = Mathf.Lerp(startScale, endScale, lerpValue);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

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
   
        double pressTime = AudioSettings.dspTime;
        double[] checkList = new double[] { 0.06, 0.1 };

        for (int x = 0; x < checkList.Length; x++)
        {   
            if (Math.Abs(pressTime - checkTime) <= checkList[x])
            {
                Debug.Log("되고 있어여");
                HideImage();

                effectManager.JudgeEffect(x);
                effectManager.NoteHitEffect();
                scoreManager.IncreaseCombo(true);
                scoreManager.IncreaseScore(x);

                return;
            }
        }
    }


}