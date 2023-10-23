using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Note : MonoBehaviour, IPointerDownHandler
{
    public float noteSpeed = 1;
    // 왼쪽으로 움직이는 노트인지
    public double distance;
    public bool isLeft;
    public double startTime;
    public int beatNum;
    private Image noteImage;
    public Color originalColor = Color.white; // 원래 색상
    public Color targetColor = Color.black; // 목표 색상 (회색)
    private double spaceBetween;
    public int upDown;
    public bool clickable;

    void Awake()
    {
        noteSpeed = 1;
        spaceBetween = GameObject.Find("LeftArea").transform.localPosition.x;
        // Debug.Log("spaceBetween 값: " + spaceBetween);
        
    }

    void OnEnable()
    {
        if(noteImage == null)
            noteImage = GetComponent<Image>();
        noteImage.enabled = true;
    }

    void Update()
    {
        double timeDiff = AudioSettings.dspTime - startTime;
        double arrivalTime = 2.0f;
        

        double currPosX = (spaceBetween / arrivalTime) * timeDiff;

        
        Vector3 newPos = transform.localPosition;

        if (isLeft)
            newPos.x = (float)currPosX;
        else
            newPos.x = -(float)currPosX;

        
        transform.localPosition = newPos;

        float lerpValue = Mathf.Clamp01((float)(currPosX / spaceBetween));
        noteImage.color = Color.Lerp(targetColor, originalColor, lerpValue);

    }

    public void setTime(double startTime)
    {
        this.startTime = startTime;

    }

    public void setBeat(int beat)
    {
        this.beatNum = beat;

    }

    // 노트의 이미지를 없앱니다.
    public void HideImage()
    {
        noteImage.enabled = false;
    }


    public bool GetIsLeft()
    {
        return this.isLeft;
    }
    public bool GetNoteFlag()
    {
        return noteImage.enabled;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}