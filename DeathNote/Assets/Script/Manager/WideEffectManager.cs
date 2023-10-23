using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WideEffectManager : MonoBehaviour
{

    [SerializeField] Animator noteHitAnimator = null;
    string hit = "Hit";
    [SerializeField] Animator judgementAnimator = null;
    [SerializeField] Image image = null;
    [SerializeField] Sprite[] sprite = null;

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }

    public void JudgeEffect(int num)
    {
        image.sprite = sprite[num];
        judgementAnimator.SetTrigger(hit);
    }
}

