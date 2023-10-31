using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
 
    [SerializeField] TextMeshProUGUI score = null;
    [SerializeField] TextMeshProUGUI combo = null;
    [SerializeField] TextMeshProUGUI comboText = null;
    [SerializeField] int increaseScore = 10;

    int currentScore = 0;
    int currentCombo = 0; 
    float[] weight = {5, 3, 1};

    // Start is called before the first frame update
    void Start()
    {
        score.text = "000,000,000";
        combo.text = "0";
    }

    public void IncreaseScore(int judge)
    {
        float upScore = (int)(increaseScore * weight[judge] + SkillManager.instance.scoreBonus[judge] * (1 + SkillManager.instance.scoreCrit[judge]) + SkillManager.instance.scoreBonus[4] * (1 + SkillManager.instance.scoreCrit[judge]));
        float comboScore = (currentCombo/10) * 10 + SkillManager.instance.comboBonus;
        currentScore += (int)(upScore+comboScore);
        score.text = string.Format("{0:000,000,000}", currentScore);
    }
    
    public void IncreaseBonusScore(int level)
    {
        float upScore = 100 + SkillManager.instance.scoreBonus[3] * SkillManager.instance.scoreCrit[3] *  SkillManager.instance.addLevel;
        currentScore += (int)upScore;
        
    }

    public void IncreaseCombo(bool isGreat)
    {
        if (isGreat)
        {
            currentCombo++;
        
            combo.text = string.Format("{0:###0}", currentCombo);
        }
        else
        {
            int randomNumber = UnityEngine.Random.Range(1, 100); // 0에서 100까지의 난수 생성
            if(randomNumber > SkillManager.instance.comboChance)
            {
                currentCombo = 0;
                combo.text = "0";
            }
        }
    }
}
    