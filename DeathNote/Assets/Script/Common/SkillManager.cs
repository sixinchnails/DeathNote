using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public int[] scoreBonus = new int[5]; // 점수보너스
    public float[] scoreCrit = { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }; // 크리티컬
    public int comboBonus = 0; // 콤보 추가점수
    public int comboCool = 0; // 콤보 완화
    public float comboChance = 0; // 콤보 안끊기기
    public float addTime = 0; // 보너스노트 추가시간
    public float addLevel = 1; // 보너스점수 누적
    
    public float expBonus ; // 경험치
    public float expCrit; // 경험치부스트
    public float[] worldBonus = new float[7]; // 1월드


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
