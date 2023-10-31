using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    public int totalNoteNum;
    
    [SerializeField] GameObject resultUI = null;

    
    [SerializeField] Text[] txtCount = null;
    [SerializeField] Text songTitle = null;
    [SerializeField] Text totalNote = null;
    [SerializeField] Text totalCombo = null;
    [SerializeField] Text score = null;
    [SerializeField] Text grade = null;

    ScoreManager scoreManager = null;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

    }

    public void ShowResult()
    {
        resultUI.SetActive(true);

        for(int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = "0";
        }
        songTitle.text = "";
        totalNote.text = "0";
        totalCombo.text = "0";
        score.text = "0";
        grade.text ="";
    }
}
