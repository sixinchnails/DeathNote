using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI songTitle;
    [SerializeField] GameObject paper;
    [SerializeField] GameObject content;
    [SerializeField] TextMeshProUGUI myGold;
    [SerializeField] TextMeshProUGUI myPercent;
    [SerializeField] TextMeshProUGUI myScore;
    [SerializeField] Animator animator;

    ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void ShowResult(string title, float percent, string score, int gold)
    {
        songTitle.text = title;
        myPercent.text = percent.ToString("F2");
        myScore.text = score;
        myGold.text = "+" + gold.ToString();
        gameObject.SetActive(true);
        StartCoroutine(ShowPaper());
    }

    IEnumerator ShowPaper()
    {
        animator.SetTrigger("showdown");
        yield return new WaitForSeconds(2.0f);

        content.SetActive(true);
    }
}
