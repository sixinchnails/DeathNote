using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI score = null;
    [SerializeField] public TextMeshProUGUI grade = null;
    [SerializeField] Image[] blur = null;
    [SerializeField] Image background = null;
    [SerializeField] Sprite[] sprites = null;
   
    private int totalNote; // 전체 노트 갯수


    public int currentScore = 0; // 현재 점수
    public int currentCombo = 0; // 현재 콤보
    public int maxCombo = 0; // 최대 콤보
    public int totalPerfect = 0; // 총 퍼펙트
    public int totalInspirit = 0; // 총 골드


    public int totalPercent = 0; // 총 퍼센트

    public Color white = Color.white; // 어두운 색
    public Color[] originColor;
    public Color[] finalColor;
    public Color endparent; // 최종 색상
    public Color transparent; // 처음 색상 (투명색)


    void Start()
    {
        totalNote = MusicManager.instance.totalNote;//전체 노트
        int code = MusicManager.instance.code;
        switch (code)
        {
            case 401: background.sprite = sprites[0]; break;
            case 402: background.sprite = sprites[1]; break;
            case 403: background.sprite = sprites[2]; break;
            case 404: background.sprite = sprites[3]; break;
            case 501: background.sprite = sprites[4]; break;
            case 502: background.sprite = sprites[5]; break;
            case 503: background.sprite = sprites[6]; break;
            case 504: background.sprite = sprites[7]; break;
            case 601: background.sprite = sprites[8]; break;
            case 602: background.sprite = sprites[9]; break;
            case 603: background.sprite = sprites[10]; break;
            case 604: background.sprite = sprites[11]; break;
        }
        originColor = new Color[12];
        finalColor = new Color[12];
        transparent = new Color(Color.white.r, Color.white.g, Color.white.b, 0);
        endparent = new Color(Color.white.r, Color.white.g, Color.white.b, 0.8f);

        for (int i = 0; i < originColor.Length; i++)
        {
            originColor[i] = blur[i].color; // 블러의 칼라를 originColor[i]
        }

        for (int i = 0; i < 6; i++)
        {
            Soul soul = SkillManager.instance.equip[i];
            // 블러의 칼라를 다시 재설정
            if (SkillManager.instance.equip.Count > i)
            {
                blur[2 * i].color = new Color(originColor[11].r, originColor[11].b, originColor[11].b, 0);
                blur[2 * i + 1].color = new Color(originColor[11].r, originColor[11].b, originColor[11].b, 0);
            }
            else
            {
                int body = soul.customizes[2];
                blur[2 * i].color = new Color(originColor[body].r, originColor[body].b, originColor[body].b, 0);
                int eyes = soul.customizes[3];
                blur[2 * i + 1].color = new Color(originColor[eyes].r, originColor[eyes].b, originColor[eyes].b, 0);
            }
        }

        for(int i = 0; i < 12; i++)
        {
            originColor[i] = new Color(blur[i].color.r, blur[i].color.g, blur[i].color.b, 0); // 시작색상
            finalColor[i] = new Color(blur[i].color.r, blur[i].color.g, blur[i].color.b, 0.3f); // 투명색상
        }

        score.text = "000,000"; // 스코어 텍스트 초기화
        grade.text = "0.00%"; // 그레이드 텍스트 초기화
    }

    // 점수를 올리는 메서드
    public void IncreaseScore(int percent, int bonus, int combo, int perfectB, int gold)
    {
        totalPercent += percent; // 퍼센트 점수를 더함
        float basicScore = (percent / 10.0f) + bonus; // 기본점수x계수 + 판정보너스 + 기본보너스  

        if (percent == 100)
        {
            totalPerfect++; // 퍼펙트일 경우 퍼펙트를 더함
            basicScore += perfectB;

        }
            
        if(currentCombo != 0) // 50 콤보마다 추가점수
        {
            basicScore += combo;
        }

        totalInspirit += gold;

        currentScore += (int)(basicScore); // 현재 스코어에 추가
        score.text = currentScore.ToString("000,000");  // 형식
      
        float realGrade = (float)totalPercent / totalNote; // 백분율
        realGrade = Mathf.Round(realGrade * 1000f) / 1000f; // 소숫점 셋째자리에서 반올림
        grade.text = realGrade.ToString("F2")+"%"; // 출력


        for (int i = 0; i < 12; i++)
        {
            // lerpValue 계산

            // 각 blur[i]에 대한 최대 lerpValue 범위 조정 (i가 증가함에 따라 더 천천히 바뀌도록)
            float maxLerpRange = 100f - (i * 5f); // i가 증가할수록 maxLerpRange 감소

            // baseLerpValue를 조정된 범위에 맞게 스케일링
            float scaledLerpValue = Mathf.Clamp01(realGrade / maxLerpRange);

            // 색상 변경
            blur[i].color = Color.Lerp(originColor[i], finalColor[i], scaledLerpValue);
            if(i==0) background.color = Color.Lerp(transparent, endparent, scaledLerpValue);
            // 크기 변경
            Vector3 newScale = Vector3.one * 4 * scaledLerpValue; // Vector3.one은 (1, 1, 1)을 의미
            blur[i].GetComponent<RectTransform>().localScale = newScale;

        }


    }

    // 콤보를 올리거나 초기화 

    public void IncreaseCombo(bool isIncrease)
    {
        if (isIncrease)
        {
            currentCombo++;
            if (currentCombo > maxCombo) maxCombo++;

        }
        else
        {
            currentCombo = 0;
        }
    }
}


/**
 * 기존 시스템
 */

//public class ScoreManager : MonoBehaviour
//{
//    public static ScoreManager instance;
//    [SerializeField] TextMeshProUGUI score = null;

//    private int increaseScore = 100; // 노래의 기본 수치
//    private int[] weight = { 5, 3, 1 }; // 판정별 계수
//    private int[] scoreBonus;
//    private int[] scoreCrit;
//    private int[] comboBonus;
//    public int currentScore = 0; // 현재 점수
//    public int currentCombo = 0; // 현재 콤보


//    void Start()
//    {
//        instance = this;
//        score.text = "000,000,000"; // 스코어 텍스트 초기화
//        scoreBonus = SkillManager.instance.scoreBonus;
//        scoreCrit = SkillManager.instance.scoreCrit;
//        comboBonus = SkillManager.instance.comboBonus;
//    }



//    // 점수를 올리는 메서드
//    public void IncreaseScore(int judge, bool safe)
//    {
//        int basicScore = (increaseScore * weight[judge]) + scoreBonus[judge] + scoreBonus[2]; // 기본점수x계수 + 판정보너스 + 기본보너스
//        float critical = (float)(1 + (scoreCrit[judge] + scoreCrit[2]) / 100.0); // 1 + (판정보너스+기본보너스)/100
//        int comboScore = (currentCombo/10) * 10 + comboBonus[0]; // 콤보는 십의자리수만큼 점수( 251 -> 250) + 기본보너스
//        if (currentCombo != 0 && currentCombo % 50 == 0 && !safe) comboScore += comboBonus[1]; // 50점 보너스 추가 : 0이거나, safe판정인 경우엔 안됨
//        currentScore += (int)(basicScore*critical+comboScore);
//        score.text = currentScore.ToString("N0").PadLeft(9, '0');
//    }

//    // 콤보를 올리거나 초기화 

//    public void IncreaseCombo(bool isIncrease)
//    {
//        if (isIncrease)
//        {
//            currentCombo++;

//        }
//        else
//        {
//            currentCombo = 0;
//        }
//    }
//}
