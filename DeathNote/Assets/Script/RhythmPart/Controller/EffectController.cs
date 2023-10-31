using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *  노트의 효과를 처리하는 컨트롤러
 */
public class EffectController : MonoBehaviour
{

    Animator noteAnimator = null;
    Animator hitAnimator = null;    
    Animator judgeAnimator = null;
    

    public void Awake()
    {
        Debug.Log(transform.childCount);
        noteAnimator = GetComponent<Animator>();
        hitAnimator = transform.GetChild(0).GetComponent<Animator>();
        judgeAnimator = transform.GetChild(1).GetComponent<Animator>();
    }

    public void RepeatNote()
    {
        noteAnimator.SetTrigger("Repeat");
    }

    public void NoteHitEffect()
    {
        hitAnimator.SetTrigger("Hit");
    }

    public void JudgeEffect(string judge)
    {
        judgeAnimator.SetTrigger(judge);
    }


}
