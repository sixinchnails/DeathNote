using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoulDetail : MonoBehaviour
{

    [SerializeField] GameObject BookUI; // 전체 UI를 의미
    [SerializeField] GameObject[] BookPage; // 북페이지를 의미 : 정령 / 스킬 / 환생
    [SerializeField] Animator[] soulImages; // 정령의 생김새를 나타내는 부분
    [SerializeField] TMP_InputField nameInputField; // 정령의 이름 입력받고 나타냄
    [SerializeField] TextMeshProUGUI[] emotions; // 정령의 각 수치를 알려줌

    [SerializeField] TextMeshProUGUI[] tier; // 스킬 티어
    [SerializeField] TextMeshProUGUI[] skillname; // 스킬 이름
    [SerializeField] TextMeshProUGUI[] skillDescription; // 스킬에 대한 설명
   
    [SerializeField] TextMeshProUGUI revive; // 환생 횟수
    [SerializeField] Image[] button; // 환생 체크
    [SerializeField] TextMeshProUGUI reviveCost; // 환생 가격 
    [SerializeField] Sprite notChecked; // 체크 안된 스프라이트
    [SerializeField] Sprite isChecked; // 체크 된 스프라이트
    
    [SerializeField] GardenManager gardenManager; // 가든매니저

    [SerializeField] Image[] selectArea; // 누르는거
    [SerializeField] Animator[] mySession; // 세션
    [SerializeField] TextMeshProUGUI[] targetEmotions; // 세션 감정
    [SerializeField] TextMeshProUGUI[] targetSkills; // 세션 스킬

    public GardenSoul activeSoul;
    public Soul targetSoul;
    public int targetIdx;
    public Soul nowSoul; // 지금 누른 소울
    bool[] ascendSwitch;


    // 책 UI의 활성화 상태를 변경
    public void OpenBook(GardenSoul Gsoul)
    {

        BookUI.SetActive(!BookUI.activeSelf); // 상태 변경 후, 활성화 되었다면 ?
        if (BookUI.activeSelf)
        {
            activeSoul = Gsoul;

            InitBook(Gsoul.soul); // 초기화
            ChangeBookPage1(); // 책 1페이지로
        }
        else
        {
            Gsoul.CameraZoom();
        }
    }

    // 책 초기화 메서드
    public void InitBook(Soul soul)
    {
        nowSoul = soul;
        nameInputField.text = soul.name; // 이름을 표시
        for (int i = 0; i < 6; i++)
        {
            emotions[i].text = soul.emotions[i].ToString(); // 감정을 표시
        }


        // 환생 횟수 설정
        revive.text = "총 " + soul.revive.ToString() + " 번째";
        // 버튼 초기화
        for(int i  = 0; i < 5; i++)
        {
            // 환생 옵션 꼭 없앨것
            button[i].sprite = notChecked;
        }

        ascendSwitch = new bool[6];
        reviveCost.text = CalcuateCost().ToString();

        // 스킬 텍스트 설정
        for (int i = 0; i < 3; i++)
        {
            Skill skill = SkillManager.instance.GetSkillInfo(soul.parameters[i]);
            skillname[i].text = skill.name;
            skillDescription[i].text = skill.description;
            string star = "";
            for (int j = 0; j < skill.tier; j++)
            {
                star += "★"; // 티어만큼 별을 설정
            }   
            tier[i].text = star;
        }
    }

    // 정령 페이지로 변경
    public void ChangeBookPage1()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 0) BookPage[i].SetActive(true); // 이 페이지를 활성화하고 
            else BookPage[i].SetActive(false); // 다른 페이지를 활성화하지 않음
            Debug.Log(nowSoul.customizes[i]);
        }
        soulImages[0].SetInteger("body", nowSoul.customizes[0]);
        soulImages[0].SetInteger("eyes", nowSoul.customizes[1]);
        soulImages[0].SetInteger("bcol", nowSoul.customizes[2]);
        soulImages[0].SetInteger("ecol", nowSoul.customizes[3]);
    }

    // 스킬 페이지로 변경
    public void ChangeBookPage2()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 1) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }
    }

    // 환생 페이지로 변경
    public void ChangeBookPage3()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 2) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }
        ascendSwitch = new bool[6];

        soulImages[1].SetInteger("body", nowSoul.customizes[0]);
        soulImages[1].SetInteger("eyes", nowSoul.customizes[1]);
        soulImages[1].SetInteger("bcol", nowSoul.customizes[2]);
        soulImages[1].SetInteger("ecol", nowSoul.customizes[3]);

        for(int i = 0; i < 6; i++)
        {
            button[i].sprite = notChecked;
        }

    }

    public void ChangeBookPage4()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i == 3) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }

        // 애니메이터를 변경
        for(int i = 0; i < 6; i++)
        {
            Soul equipSoul = SkillManager.instance.equip[i];
            if(equipSoul != null)
            {
                mySession[i].SetInteger("body", equipSoul.customizes[0]);
                mySession[i].SetInteger("eyes", equipSoul.customizes[1]);
                mySession[i].SetInteger("bcol", equipSoul.customizes[2]);
                mySession[i].SetInteger("ecol", equipSoul.customizes[3]);
            }
            else
            {
                mySession[i].SetInteger("body", 0);
                mySession[i].SetInteger("eyes", -1);
                mySession[i].SetInteger("bcol", -1);
                mySession[i].SetInteger("ecol", 0);
            }

            TargetSelect(0);
        }
    }
    // 윗 정령
    public void GoUp()
    {
        int length = UserManager.instance.userData.souls.Count;
        int prevIdx = (nowSoul.id + length - 1) % length;
        Soul prevSoul = UserManager.instance.userData.souls[prevIdx];
        GardenSoul prev = prevSoul.gardenSoul;
        activeSoul = prev;
        prev.CameraZoom();
        InitBook(UserManager.instance.userData.souls[prevIdx]);
        ChangeBookPage1();
    }

    // 아랫 정령
    public void GoDown()
    {
        int length = UserManager.instance.userData.souls.Count;
        int nextIdx = (nowSoul.id + 1) % length;
        Soul nextSoul = UserManager.instance.userData.souls[nextIdx];
        GardenSoul next = nextSoul.gardenSoul;
        activeSoul = next;
        next.CameraZoom();
        InitBook(UserManager.instance.userData.souls[nextIdx]);
        ChangeBookPage1();
    }


    public void ChangeName()
    {
        string name = nameInputField.text;
        if (name.Length >= 2 && name.Length <= 6) activeSoul.ChangeName(name);
    }

    public void TargetSelect(int idx)
    {
        targetSoul = SkillManager.instance.equip[idx];
        targetIdx = idx;

        for (int i = 0; i < 6; i++)
        {
            if (i != idx)
            {
                selectArea[i].color = new Color(0, 0, 0, 0.4f);
            }
            else selectArea[i].color = new Color(0, 0, 0, 1);
        }

        for (int i = 0; i < 6; i++)
        {
            if(targetSoul == null) targetEmotions[i].text = "-";
            else targetEmotions[i].text = targetSoul.emotions[i].ToString();
            if(i < 3)
            {
                if (targetSoul == null) targetSkills[i].text = "-";
                else targetSkills[i].text = SkillManager.instance.GetSkillInfo(targetSoul.parameters[i]).name;
            }
        }
    }

    public void Equip()
    {
        if (targetSoul != null && targetSoul.Equals(nowSoul)) return;

        int exchangeIdx = -1;

        for (int i = 0; i < 6; i++)
        {
            if (SkillManager.instance.equip[i] != null && SkillManager.instance.equip[i].Equals(nowSoul)) exchangeIdx = i ;
        }

        // 교환
        if(exchangeIdx != -1 && SkillManager.instance.equip[targetIdx] != null)
        {
            SkillManager.instance.equip[exchangeIdx] = SkillManager.instance.equip[targetIdx];
            SkillManager.instance.equip[exchangeIdx].equip = exchangeIdx;
            SkillManager.instance.equip[targetIdx] = nowSoul;
            nowSoul.equip = targetIdx;
        }

        // 없는곳
        else if(exchangeIdx != -1 && SkillManager.instance.equip[targetIdx] == null)
        {
            SkillManager.instance.equip[targetIdx] = nowSoul;
            nowSoul.equip = targetIdx;
            SkillManager.instance.equip[exchangeIdx] = null;
        }

        else
        {
            if (SkillManager.instance.equip[targetIdx] != null) SkillManager.instance.equip[targetIdx].equip = -1;
            nowSoul.equip = targetIdx;
            SkillManager.instance.equip[targetIdx] = nowSoul;
        }     

        UserManager.instance.SaveData();
        ChangeBookPage4();
    }

    public void EquipClick1()
    {
        TargetSelect(0);
    }

    public void EquipClick2()
    {
        TargetSelect(1);
    }
    public void EquipClick3()
    {
        TargetSelect(2);
    }
    public void EquipClick4()
    {
        TargetSelect(3);
    }
    public void EquipClick5()
    {
        TargetSelect(4);
    }
    public void EquipClick6()
    {
        TargetSelect(5);
    }


    // 체크하기
    public void AscendClick1()
    {
        ascendSwitch[0] = !ascendSwitch[0];
        if (button[0].sprite.Equals(isChecked)) button[0].sprite = notChecked;
        else button[0].sprite = isChecked;

        reviveCost.text = CalcuateCost().ToString();
    }

    public void AscendClick2()
    {
        ascendSwitch[1] = !ascendSwitch[1];
        if (button[1].sprite.Equals(isChecked)) button[1].sprite = notChecked;
        else button[1].sprite = isChecked;

        reviveCost.text = CalcuateCost().ToString();
    }

    public void AscendClick3()
    {
        ascendSwitch[2] = !ascendSwitch[2];
        if (button[2].sprite.Equals(isChecked)) button[2].sprite = notChecked;
        else button[2].sprite = isChecked;

        reviveCost.text = CalcuateCost().ToString();
    }

    public void AscendClick4()
    {
        ascendSwitch[3] = !ascendSwitch[3];
        if (button[3].sprite.Equals(isChecked)) button[3].sprite = notChecked;
        else button[3].sprite = isChecked;

        reviveCost.text = CalcuateCost().ToString();
    }

    public void AscendClick5()
    {
        ascendSwitch[4] = !ascendSwitch[4];
        if (button[4].sprite.Equals(isChecked)) button[4].sprite = notChecked;
        else button[4].sprite = isChecked;

        reviveCost.text = CalcuateCost().ToString();
    }

    public void AscendClick6()
    {
        ascendSwitch[5] = !ascendSwitch[4];
        if (button[5].sprite.Equals(isChecked)) button[5].sprite = notChecked;
        else button[5].sprite = isChecked;

        reviveCost.text = CalcuateCost().ToString();
    }

    public int CalcuateCost()
    {
        int cost = 1000 + 50 * nowSoul.revive;
        for(int i = 0; i <6; i++)
        {
            if (ascendSwitch[i]) cost += 1000;
        }

        return cost;
    }

    // 리바이브
    public void Revive()
    {
        int cost = CalcuateCost();
        if(UserManager.instance.userData.gold >= cost)
        {
            UserManager.instance.userData.gold -= cost;
            ReviveSoul();
            activeSoul.ReRender();
            gardenManager.UpdateInspirit();
        }

        gameObject.SetActive(false);
    }

    // 만들기
    public void ReviveSoul()
    {
        if (!ascendSwitch[0]) nowSoul.customizes[0] = UnityEngine.Random.Range(1, 7); // 1부터 6까지 몸통
        if (!ascendSwitch[1]) nowSoul.customizes[1] = UnityEngine.Random.Range(1, 3); // 1부터 6까지 몸통
        if (!ascendSwitch[2])
        {
            int chance = UnityEngine.Random.Range(0, 100);

            if (chance > 70)
            {
                nowSoul.customizes[2] = UnityEngine.Random.Range(1, 11);

            }
            chance = UnityEngine.Random.Range(0, 100);

            if (chance > 50)
            {
                nowSoul.customizes[3] = UnityEngine.Random.Range(1, 11);

            }
        }

        // 스킬뽑기
        for (int i = 0; i < 3; i++)
        {
            if (ascendSwitch[i + 3]) continue;
            int chance = UnityEngine.Random.Range(0, 100);
            if (chance == 0)
            {
                nowSoul.parameters[i] = 200; // 신화 스킬
            }
            else if (chance < 10)
            {
                nowSoul.parameters[i] = 100 + UnityEngine.Random.Range(0, 3); // 전설 스킬
            }
            else
            {
                nowSoul.parameters[i] = UnityEngine.Random.Range(0, 6); // 일반 스킬
            }
        }

        nowSoul.parameters[3] = UnityEngine.Random.Range(3, 10);
        // 감정은 5부터 20
        for (int i = 0; i < 6; i++)
        {
            nowSoul.emotions[i] += UnityEngine.Random.Range(5, 20);
        }

        nowSoul.revive += 1;

        UserManager.instance.SaveData();
    }
}
