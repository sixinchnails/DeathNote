using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
 
    [SerializeField] Text score = null;
    [SerializeField] Text combo = null;
    [SerializeField] Text comboText = null;
    [SerializeField] int increaseScore = 10;

    int currentScore = 0;
    int currentCombo = 0; 
    [SerializeField] float[] weight = null;

    // Start is called before the first frame update
    void Start()
    {
        score.text = "000,000,000";
        combo.text = "0";
    }

    public void IncreaseScore(int judge)
    {
        int upScore = (int)(increaseScore * weight[judge]) + (currentCombo/10) * 10;
        currentScore += upScore;
        score.text = string.Format("{0:000,000,000}", currentScore);
    }
    public void IncreaseCombo(bool isGreat)
    {
        if (isGreat)
        {
            currentCombo++;
            Debug.Log(currentCombo);
            combo.text = string.Format("{0:###0}", currentCombo);
        }
        else
        {
            currentCombo = 0;
            combo.text = "0";
        }
    }
}
    