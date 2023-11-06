using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] TextMeshProUGUI score = null;
    
    private int increaseScore = 100; // 노래의 기본 수치
    private int[] weight = { 5, 3, 1 }; // 판정별 계수
    private int[] scoreBonus;
    private int[] scoreCrit;
    private int[] comboBonus;
    public int currentScore = 0; // 현재 점수
    public int currentCombo = 0; // 현재 콤보
    

    void Start()
    {
        instance = this;
        score.text = "000,000,000"; // 스코어 텍스트 초기화
        scoreBonus = SkillManager.instance.scoreBonus;
        scoreCrit = SkillManager.instance.scoreCrit;
        comboBonus = SkillManager.instance.comboBonus;
    }


   
    // 점수를 올리는 메서드
    public void IncreaseScore(int judge, bool safe)
    {
        int basicScore = (increaseScore * weight[judge]) + scoreBonus[judge] + scoreBonus[2]; // 기본점수x계수 + 판정보너스 + 기본보너스
        float critical = (float)(1 + (scoreCrit[judge] + scoreCrit[2]) / 100.0); // 1 + (판정보너스+기본보너스)/100
        int comboScore = (currentCombo/10) * 10 + comboBonus[0]; // 콤보는 십의자리수만큼 점수( 251 -> 250) + 기본보너스
        if (currentCombo != 0 && currentCombo % 50 == 0 && !safe) comboScore += comboBonus[1]; // 50점 보너스 추가 : 0이거나, safe판정인 경우엔 안됨
        currentScore += (int)(basicScore*critical+comboScore);
        score.text = currentScore.ToString("N0").PadLeft(9, '0');
    }

    // 콤보를 올리거나 초기화 

    public void IncreaseCombo(bool isIncrease)
    {
        if (isIncrease)
        {
            currentCombo++;
      
        }
        else
        {
            currentCombo = 0;
        }
    }
}
    