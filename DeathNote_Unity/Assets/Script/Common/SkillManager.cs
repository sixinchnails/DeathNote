using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 스킬 매니저입니ㅏ.
public class Skill
{
    public string name;
    public int tier;
    public string description;
    public int percent;
    public int[] bonus;
    public int[] combo;
    public int[] perfect;
    public int[] gold;

    public Skill(string name, int tier, string descrition, int percent, int[] bonus, int[] combo, int[] perfect, int[] gold)
    {
        this.name = name; // 스킬명
        this.tier = tier; // 티어
        this.description = descrition; // 스킬설명
        this.percent = percent; // 발동 확률
        this.bonus = bonus; // 보너스
        this.combo = combo; // 콤보 비례 점수
        this.perfect = perfect; // 최대판정 추가점수
        this.gold = gold;
    }
}
public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    // 장착한 소울 (총 16개)
    public List<Soul> equip;
   
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

        equip = new List<Soul>(6);
        for(int i = 0; i < 6; i++)
        {
            equip.Add(null);
        }
        Debug.Log(equip[1]);
        Debug.Log("우후후훟후");
    }


    // 스킬과 그 효과를 반환하는 메서드
    public Skill GetSkillInfo(int idx)
    {
        switch (idx)
        {
            case 0:
                Console.WriteLine("num:10");
                return new Skill("업 비트", 0, "30%의 확률로 비트감에 비례한 낮은 보너스 점수를 얻는다.",
                    30, new int[] { 0, 10 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 1:
                return new Skill("그대안의 블루", 0, "30%의 확률로 감성에 비례한 낮은 보너스 점수를 얻는다.",
                    30, new int[] { 1, 10 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 2:
                return new Skill("탈무드", 0, "30%의 확률로 무드에 비례한 낮은 보너스 점수를 얻는다.",
                    30, new int[] { 2, 10 }, new int[] { 0, 10 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 3:
                return new Skill("평온한음율", 0, "30%의 확률로 평온함에 비례한 낮은 보너스 점수를 얻는다.",
                    30, new int[] { 3, 10 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 4:
                return new Skill("설레임덕", 0, "30%의 확률로 설렘에 비례한 낮은 보너스 점수를 얻는다.",
                    30, new int[] { 4, 10 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 5:
                return new Skill("실화냐가슴이웅장", 0, "30%의 확률로 웅장함에 비례한 낮은 보너스 점수를 얻는다.",
                    30, new int[] { 5, 10 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 6:
                return new Skill("비트마스킹", 0, "30%의 확률로 비트감에 비례한 낮은 콤보 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 0, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 7:
                return new Skill("그랑블루", 0, "30%의 확률로 감성에 비례한 낮은 콤보 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 1, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 8:
                return new Skill("무드등", 0, "30%의 확률로 무드에 비례한 낮은 콤보 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 2, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 9:
                return new Skill("평다운", 0, "30%의 확률로 평온함에 비례한 낮은 콤보 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 3, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 10:
                return new Skill("쾅쾅둥둥", 0, "30%의 확률로 웅장함에 비례한 낮은 콤보 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 5, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 11:
                return new Skill("설레임민트맛", 0, "30%의 확률로 설렘에 비례한 낮은 콤보 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 4, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 100:
                return new Skill("비트에 몸을 맡겨", 1, "30%의 확률로 비트감에 비례한 퍼펙트 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 40 }, new int[] { 0, 0 });
            case 101:
                return new Skill("블루베리", 1, "30%의 확률로 감성에 비례한 퍼펙트 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 1, 40 }, new int[] { 0, 0 });
            case 102:
                return new Skill("무드유라잌", 1, "30%의 확률로 무드감에 비례한 퍼펙트 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 2, 40 }, new int[] { 0, 0 });
            case 103:
                return new Skill("치유의 방울", 1, "30%의 확률로 평온함에 비례한 퍼펙트 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 3, 40 }, new int[] { 0, 0 });
            case 104:
                return new Skill("두근대는 하트", 1, "30%의 확률로 설렘에 비례한 퍼펙트 점수를 얻는다.",
                    30, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 4, 40 }, new int[] { 0, 0 });
            case 105:
                return new Skill("신세계 고향곡", 1, "30%의 확률로 웅장함에 비례한 퍼펙트 점수를 얻는다.",
                     30, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 5, 40 }, new int[] { 0, 0 });
            case 106:
                return new Skill("돈버는 비트", 1, "20%의 확률로 비트감에 비례한 영감을 얻는다.",
                    20, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 20 });
            case 107:
                return new Skill("왕따호소", 1, "20%의 확률로 감성에 비례한 영감 얻는다.",
                    20, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 1, 20 });
            case 108:
                return new Skill("무드보단무등", 1, "20%의 확률로 무드감에 비례한 영감을 얻는다.",
                    20, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 2, 20 });
            case 109:
                return new Skill("명상", 1, "20%의 확률로 평온함에 비례한 영감을 얻는다.",
                    20, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 3, 20 });
            case 110:
                return new Skill("고백전날연습", 1, "20%의 확률로 설렘에 비례한 영감을 얻는다.",
                    20, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 4, 20 });
            case 111:
                return new Skill("잘난맛에살기", 1, "20%의 확률로 웅장함에 비례한 영감을 얻는다.",
                     20, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 5, 20 });
            case 200:
                return new Skill("싸버지의 강림", 2, "비트에 비례한 보너스와 무드에 비례한 콤보 점수를 얻는다.",
                    30, new int[] { 0, 50 }, new int[] { 2, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 201:
                return new Skill("평온속의 슬픔", 2, "감성에 비례한 보너스와 평온함에 비례한 콤보 점수를 얻는다.",
                    30, new int[] { 1, 50 }, new int[] { 3, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 202:
                return new Skill("설레서웅장한가", 2, "설렘에 비례한 보너스와 웅장함에 비례한 콤보 점수를 얻는다.",
                    30, new int[] { 4, 50 }, new int[] { 5, 20 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            default:
                Console.WriteLine(" num:?");
                return null;
        }
    }

    public int GetSkill(Soul turnSoul, ClickNote note)
    {
         
        // 정령의 세가지 스킬을 돌림
        for (int i = 0; i < 3; i++)
        {
            // i 번째 스킬
            int idx = turnSoul.parameters[i];
            Skill now = SkillManager.instance.GetSkillInfo(idx);
            // 발동 확률이 0퍼센트면 다음 스킬
            if (now.percent == 0) continue;
            // 0~99 사이의 랜덤 숫자
            int random = UnityEngine.Random.Range(0, 100);
            // 확률적으로 발동하면
            if (now.percent > random)
            {
                note.bonus = (int)(turnSoul.emotions[now.bonus[0]] * now.bonus[1] / 100.0f); // 뒤에는 계수
                note.combo = (int)(turnSoul.emotions[now.combo[0]] * now.combo[1] / 100.0f);
                note.perfect = (int)(turnSoul.emotions[now.perfect[0]] * now.perfect[1] / 100.0f);
                note.gold = (int)(turnSoul.emotions[now.gold[0]] * now.gold[1] / 100.0f);

                return UnityEngine.Random.Range(0,6);
            }          

        }
        return -1;
    }

    // 장비 반환
    public void SetEquip(List<Soul> soul)
    {
        equip = soul;
    }


}