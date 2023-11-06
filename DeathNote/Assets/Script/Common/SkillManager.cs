using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Soul
{
    public string soulName;
    public bool isEquip;
    public int passive;
    public int[] active;

}

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public List<Soul> souls; // 내 정령 정보
    public int[] equip = { -1, -1 }; // 왼쪽 장착 정령, 오른쪽 장착 정령
    public int[] scoreBonus = { 0, 0, 0, 0 }; // 점수보너스(deadly, delicate, discord, bonus)
    public int[] scoreCrit = { 0, 0, 0, 0 }; // 크리티컬확률(deadly, delicate, discord, bonus)
    public int comboUnbreakRatio = 0; // 콤보가 안끊길 확률
    public int[] comboBonus = { 0, 0 }; // 콤보 추가점수(1콤보 당, 50콤보 당)
    public float judgeBonus = 0.0f; // 판정 완화
    public int[] expBonus = { 0, 0, 0, 0, 0, 0 };
    public float[] worldBonus = { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f }; // 월드 보너스

    private int[][] skillSheet = new int[26][];

    void Awake()
    {
        instance = this;
        skillSheet[1] = new int[] { 50, 80, 130, 200 }; // 기본 추가점수
        skillSheet[2] = new int[] { 100, 200, 300, 500 }; // deadly 추가점수
        skillSheet[3] = new int[] { 50, 100, 150, 300 }; // delicate 추가점수
        skillSheet[4] = new int[] { 20, 40, 60, 90 }; // discord 추가점수
        skillSheet[5] = new int[] { 2, 4, 7, 10 }; // 콤보 안끊길 확률 (2% ~)
        skillSheet[6] = new int[] { 10, 20, 30, 40 }; // 콤보 추가 점수
        skillSheet[7] = new int[] { 2000, 4000, 6000, 8000 }; // 50콤보 추가 점수
        skillSheet[8] = new int[] { 5, 10, 15, 20 }; // 판정 완화(0.005초 ~)
        skillSheet[9] = new int[] { 5, 10, 15, 20 }; // 기본 크리티컬확률(5% ~)
        skillSheet[10] = new int[] { 2, 4, 6, 8 }; // deadly 크리티컬(2.5% ~)
        skillSheet[11] = new int[] { 2, 4, 6, 8 }; // delicate 크리티컬(2.5% ~)
        skillSheet[12] = new int[] { 2, 4, 6, 8 }; // discord 크리티컬(2.5% ~)
        skillSheet[13] = new int[] { 1, 2, 3, 4 }; // 편해
        skillSheet[14] = new int[] { 1, 2, 3, 4 }; // 신나
        skillSheet[15] = new int[] { 1, 2, 3, 4 }; // 멋져
        skillSheet[16] = new int[] { 1, 2, 3, 4 }; // 게이지 크리티컬 (5%~)
        skillSheet[17] = new int[] { 1, 2, 3, 4 }; // dismiss discord로 판정 (5% ~)
        skillSheet[18] = new int[] { 1, 2, 3, 4 }; // 웅장해
        skillSheet[19] = new int[] { 10, 20, 30, 40 }; // 월드 1 곱 (1.1배 ~)
        skillSheet[20] = new int[] { 10, 20, 30, 40 }; // 월드 2 곱 (1.1배 ~)
        skillSheet[21] = new int[] { 10, 20, 30, 40 }; // 월드 3 곱 (1.1배 ~)
        skillSheet[22] = new int[] { 10, 20, 30, 40 }; // 월드 4 곱 (1.1배 ~)
        skillSheet[23] = new int[] { 10, 20, 30, 40 }; // 월드 5 곱 (1.1배 ~)
        skillSheet[24] = new int[] { 10, 20, 30, 40 }; // 월드 6 곱 (1.1배 ~)
        skillSheet[25] = new int[] { 10, 20, 30, 40 }; // 전체 곱 (1.1배 ~)

        // Http 통신을 통해서, 각 public 필드에 대입
    }
    
    // 스킬의 총합을 계산하는 함수
    public void calculateSkill()
    {
        int[] skillSum = new int[26];

        foreach(Soul soul in souls)
        {
            int skillType = soul.passive / 10; // 해당하는 스킬
            int skillLevel = soul.passive % 10; // 스킬 레벨(Normal, Rare, Mythic, Devil) 

            skillSum[skillType] += skillSheet[skillType][skillLevel];

            // 장착한 정령은 따로 더해줌
            if (soul.isEquip)
            {
                for(int i = 0; i < 3; i++)
                {
                    skillType = soul.active[i] / 10; // 해당하는 스킬
                    skillLevel = soul.active[i] % 10; // 스킬 레벨(Normal, Rare, Mythic, Devil) 

                    skillSum[skillType] += skillSheet[skillType][skillLevel];
                }
            }
            
        }
        
        // 점수 보너스
        for(int i = 0; i < 4; i++)
        {
            scoreBonus[i] = skillSum[i + 1];
        }

        // 크리티컬 보너스
        for (int i = 0; i < 4; i++)
        {
            scoreCrit[i] += skillSum[i + 9];
        }

        // 콤보 안끊김
        comboUnbreakRatio = skillSum[5];

        // 콤보 보너스
        for (int i = 0; i < 2; i++)
        {
            comboBonus[i] += skillSum[i + 6];
        }

        // 판정 완화
        judgeBonus = (float)skillSum[8] / 1000;

        // 게이지 보너스
        for (int i = 0; i < 6; i++)
        {
            expBonus[i] += skillSum[i + 13];
        }

        // 월드 보너스
        for (int i = 0; i < 7; i++)
        {
            worldBonus[i] += skillSum[i + 19];
        }
    }


    // 정령 변경
    public void changeSouls(int idx, int now)
    {
        // 만약 한쪽에 장착하고 있다면
        if (equip[idx] != -1)
        {
            // 장착한 정령을 비장착 상태로 바꾸고 본인의 상태도 비장착 상태로 바꿈
            souls[equip[idx]].isEquip = false;
            equip[idx] = -1;
        }

        // 새롭게 장착하는 정령이 있다면
        if (now != -1)
        {
            // 장착할 정령을 장착 상태로 바꾸고 본인의 상태로 장착 상태로 바꿈
            souls[idx].isEquip = true;
            equip[idx] = now;

        }

        calculateSkill();
    }

    public void changeSkill(int idx)
    {
        // idx번째의 정령의 skill을 바꾸고 저장
    }
}
