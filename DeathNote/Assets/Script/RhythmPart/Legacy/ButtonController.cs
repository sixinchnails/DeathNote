using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Image image; // 버튼 컴포넌트에 사용한 이미지
    public Sprite defaultButton; // 스프라이트 기본값
    public Sprite pressedButton; // 눌렀을때의 스프라이트
    public bool isLeft; // 왼쪽인지, 오른쪽인지
    public int number; // 순서대로 0,1,2,3
    public int songSpeed; // 스피드

    EffectController effectManager; // 이펙트
    ScoreManager scoreManager; // 점수

    public Queue<GameObject> noteList = new Queue<GameObject>();

    public KeyCode keyToPress; // 눌러야할 키

    void Start()
    {
        effectManager = transform.GetComponentInChildren<EffectController>();
        scoreManager = FindObjectOfType<ScoreManager>();

        image = GetComponent<Image>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress)){ // 키를 눌렀을 때
            CheckTiming(isLeft);
            image.sprite = pressedButton;
        }

        if (Input.GetKeyUp(keyToPress)) // 키를 뗐을 때
        {
            image.sprite = defaultButton;
        }
    }

    public void CheckTiming(bool isLeft)
    {
        double pressTime = AudioSettings.dspTime;
        int effectIdx = isLeft ? 0 : 1;
        bool findNote = false;
        while (!findNote)
        {
            if (noteList.TryPeek(out GameObject result))
            {
                LegacyNote note = result.GetComponent<LegacyNote>();
                double checkTime = note.startTime + 2;
                if (pressTime - checkTime > 0.04)
                {
                    noteList.Dequeue();
                    continue;

                }
                double[] checkList = new double[] { 0.06, 0.08, 0.12, 0.2 };

                for (int x = 0; x < checkList.Length; x++)
                {
                    if (Math.Abs(pressTime - checkTime) <= checkList[x])
                    {
                        Debug.Log(isLeft + ":" + note.beatNum);
                        note.HideImage();
                        noteList.Dequeue();
                        effectManager.JudgeEffect("Discord");
                        effectManager.NoteHitEffect();
                        if (x < 3) scoreManager.IncreaseCombo(true);
                        else scoreManager.IncreaseCombo(false);
                        scoreManager.IncreaseScore(x);

                        return;
                    }
                }
                findNote = true;
            }
            else { findNote = true; }
        }
        
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CheckTiming(isLeft);
        image.sprite = pressedButton;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.sprite = defaultButton;
    }
}
