using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill
{
    public string name;
    public string tier;
    public string description;
    public int percent;
    public int[] bonus;
    public int[] combo;
    public int[] perfect;

    public Skill(string name, string tier, string descrition, int percent, int[] bonus, int[] combo, int[] perfect)
    {
        this.name = name; // 스킬명
        this.tier = tier; // 티어
        this.description = descrition; // 스킬설명
        this.percent = percent; // 발동 확률
        this.bonus = bonus; // 보너스
        this.combo = combo; // 콤보 비례 점수
        this.perfect = perfect; // 최대판정 추가점수
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
    }

    void Start()
    {
        equip = SoulManager.instance.Souls;
    }

    // 스킬과 그 효과를 반환하는 메서드
    public Skill GetSkillInfo(int idx)
    {
        switch (idx)
        {
            case 1:
                Console.WriteLine("num:10");
                return new Skill("업 비트", "일반", "30%의 확률로 비트감에 비례한 낮은 보너스 점수를 얻는다.",
                    10, new int[] { 0, 10 }, new int[] { 0, 0 }, new int[] { 0, 0 });
            case 2:
                return new Skill("그대안의 블루", "일반", "30%의 확률로 감성에 비례한 낮은 보너스 점수를 얻는다.",
                    10, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 1, 10 });
            case 3:
                return new Skill("콤보팡", "일반", "50%의 확률로 무드에 비례한 낮은 콤보 점수를 얻는다.",
                    10, new int[] { 0, 0 }, new int[] { 2, 10 }, new int[] { 0, 0 });
            case 4:
                return new Skill("평오프", "일반", "30%의 확률로 평온함에 비례한 낮은 콤보 점수를 얻는다.",
                    10, new int[] { 0, 0 }, new int[] { 3, 10 }, new int[] { 0, 0 });
            case 5:
                return new Skill("설레임덕", "일반", "30%의 확률로 설렘에 비례한 낮은 퍼펙 점수를 얻는다.",
                    10, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 4, 10 });
            case 6:
                return new Skill("실화냐가슴이웅장", "일반", "30%의 확률로 웅장함에 비례한 낮은 보너스 점수를 얻는다.",
                    10, new int[] { 0, 0 }, new int[] { 0, 0 }, new int[] { 5, 10 });
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
                note.bonus = (int)(turnSoul.emotions[now.bonus[0]] * now.bonus[1] / 100.0f);
                note.combo = (int)(turnSoul.emotions[now.combo[0]] * now.combo[1] / 100.0f);
                note.perfect = (int)(turnSoul.emotions[now.combo[0]] * now.combo[1] / 100.0f);

                return idx;
            }          

        }
        return 0;
    }

    // 장비 반환
    public void SetEquip(List<Soul> soul)
    {
        equip = soul;
    }


}