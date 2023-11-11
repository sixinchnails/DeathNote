using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Effect : MonoBehaviour
{
    public Animator hitAnimator = null; // 노트 클릭 애니메이션
    public Animator judgeAnimator = null; // 판정 애니메이션
    TextMeshProUGUI comboData = null; // 콤보
    int currentCombo;


    public void Awake()
    {
        hitAnimator = transform.GetChild(0).GetComponent<Animator>();
        judgeAnimator = transform.GetChild(1).GetComponent<Animator>();
        comboData = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void NoteHitEffect()
    {
        hitAnimator.SetTrigger("Hit"); // 타격 상태로 변경
    }

    // 판정에 따라서 콤보나 break를 나타냄
    public void JudgeEffect(string judge)
    {
        currentCombo = ScoreManager.instance.currentCombo;
        comboData.text = string.Format("{0}", currentCombo);
        judgeAnimator.SetTrigger(judge); // 특정 상태로 변경
    }

}
/**
 *  노트의 효과를 처리하는 컨트롤러
 */
//public class EffectController : MonoBehaviour
//{
//    Animator hitAnimator = null; // 노트 클릭 애니메이션
//    Animator judgeAnimator = null; // 판정 애니메이션
//    TextMeshProUGUI comboData = null;
//    int currentCombo;


//    public void Awake()
//    {
//        hitAnimator = transform.GetChild(0).GetComponent<Animator>();
//        judgeAnimator = transform.GetChild(1).GetComponent<Animator>();
//        comboData = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
//    }

//    public void NoteHitEffect()
//    {
//        hitAnimator.SetTrigger("Hit"); // 타격 상태로 변경
//    }

//    // 판정에 따라서 콤보나 break를 나타냄
//    public void JudgeEffect(string judge)
//    {
//        currentCombo = ScoreManager.instance.currentCombo;
//        comboData.text = string.Format("{0}", currentCombo);
//        judgeAnimator.SetTrigger(judge); // 특정 상태로 변경
//    }


//}
