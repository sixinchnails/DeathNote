using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class CenterNote : MonoBehaviour
{
    public Image image; // 이미지
    [SerializeField] Sprite original;
    [SerializeField] Sprite change;

    public Color white = Color.white; // 원래 색상
    public Color transparent; // 목표 색상 (회색)

    private float posX = 0; // 노트의 생성 위치(x좌표)
    private float posY = 0; // 노트의 생성 위치(y좌표)

    private double checkTime = 0;
    private double currentTime = 0;
    private double enabledTime = 0;

    public float lerpValue;

    void Awake()
    {
        image = GetComponentInChildren<Image>();
        transparent = new Color(white.r, white.g, white.b, 0); // 투명색상

    }
    void OnEnable()
    {
        image.color = Color.Lerp(white, transparent, 1); // 투명색으로 설정
        image.sprite = original; // 스프라이트 기존으로 설정

        transform.localPosition = new Vector3(posX, posY); // 노트의 위치를 변경
        transform.localScale = Vector3.one; // 스케일 변경

        enabledTime = AudioSettings.dspTime;
    }

    void Update()
    {
        currentTime = AudioSettings.dspTime; // 현재 시간
        lerpValue = Mathf.Clamp01((float)((currentTime - enabledTime) / (checkTime - enabledTime))); // 보간(Clamp는 0~1로 제한)

        // 색상 애니메이션
        image.color = Color.Lerp(white, transparent, 1 - lerpValue);

        if(currentTime >= checkTime)
        {
            image.sprite = change;
        }

    }

    // 노트의 이미지를 없앱니다.
    public void HideImage()
    {
        image.enabled = false;
    }

    public void SetNoteInfo(float x, float y, double t)
    {
        this.posX = x;
        this.posY = y;
        this.checkTime = t;
    }

    public void Finish()
    {
        // NotePool.instance.centerQueue.Enqueue(gameObject);
        gameObject.SetActive(false);
    }


}