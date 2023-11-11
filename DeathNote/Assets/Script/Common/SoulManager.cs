using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 정령
[System.Serializable]
public class Soul
{
    public int id; // 고유값
    public string name; // 이름
    public int equip; // 장착 위치
    public int[] parameters; // 스킬1, 스킬2, 스킬3, 무게
    public int[] customizes; // 바디, 눈, 악세서리, 스킨
    public int[] emotions; // 6가지 수치
    public int revive; // 총 환생 횟수
    public int garden; // 현재 가든 위치
}

public class SoulManager : MonoBehaviour
{
    public static SoulManager instance;
    [SerializeField] public List<Soul> Souls;
 
    public Soul[] Equip // 내 장착
    {
        get; private set;
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환시 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스를 파괴
        }
        
        // 장착 정령 초기화
        Equip = new Soul[16];
    }
    
    // 내 정령을 등록
    public void SetSoul(List<Soul> souls)
    {
        // 장착된 정령의 경우, Equip 프로퍼티에 넣음
        foreach(Soul soul in souls)
        {
            if (soul.equip != 0)
            {
                Equip[soul.equip - 1] = soul;
            }       
        }

        Souls = souls;
    }

    // 정령 변경
    public void reviveSouls(int idx, int now)
    {
        Debug.Log("정령 환생");
    }

}
