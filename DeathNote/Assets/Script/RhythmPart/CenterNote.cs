using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class CenterNote : MonoBehaviour
{
    public Color white = Color.white; // 원래 색상
    public Color transparent; // 목표 색상 (회색)

    // 노트의 생성 위치(x좌표)
    private float posX = 0;
    // 노트의 생성 위치(y좌표)
    private float posY = 0;
    // 노트의 판정 시간
    private float length = 0;
    private bool isLeft;

    public double enterTime = 0;
    public double checkTime = 0;
    public double currentTime = 0;
    public double enabledTime = 0;

    public float lerpValue;

    [SerializeField] Image image;
    [SerializeField] Sprite original;
    [SerializeField] Sprite change;
    Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        image = GetComponentInChildren<Image>();
        
    }
    void OnEnable()
    {
        // 투명색으로 설정
        image.color = Color.Lerp(white, transparent, 1);
        image.sprite = original;

        // 노트의 위치를 변경하고, 스케일을 1로 맞춤
        transform.localPosition = new Vector3(posX, posY);
        transform.localScale = Vector3.one;
        

        if (!isLeft)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        enabledTime = AudioSettings.dspTime;
        animator.Play("Idle");
    }

    void Update()
    {
        currentTime = AudioSettings.dspTime;

        float lerpValue = (float)((currentTime - enabledTime) / (enterTime - enabledTime));
        lerpValue = Mathf.Clamp01(lerpValue);
        image.color = Color.Lerp(white, transparent, 1 - lerpValue);

    }


    // 노트의 이미지를 없앱니다.
    public void HideImage()
    {
        image.enabled = false;
    }

    public void Check()
    {
        image.sprite = change;
    }

    public float Length()
    {
        return length;
    }

    public void SetNoteInfo(float x, float y, double e, double t, float len, bool l)
    {
        this.posX = x;
        this.posY = y;
        this.enterTime = e;
        this.checkTime = t;
        this.isLeft = l;
        this.length = len;
    }


    public void Finish()
    {
        NotePool.instance.centerQueue.Enqueue(gameObject);
        gameObject.SetActive(false);
    }


}