using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulDetail : MonoBehaviour
{

    [SerializeField] GameObject BookUI; // 전체 UI를 의미
    [SerializeField] GameObject[] BookPage; // 북페이지를 의미 : 정령 / 스킬 / 환생
    [SerializeField] Animator[] soulImages; // 정령의 생김새를 나타내는 부분
    [SerializeField] TMP_InputField nameInputField; // 정령의 이름 입력받고 나타냄
    [SerializeField] TextMeshProUGUI[] emotions; // 정령의 각 수치를 알려줌

    [SerializeField] TextMeshProUGUI tier; // 스킬 티어
    [SerializeField] TextMeshProUGUI[] skillname; // 스킬 이름
    [SerializeField] TextMeshProUGUI[] skillDescription; // 스킬에 대한 설명
   
    [SerializeField] TextMeshProUGUI revive; // 환생 횟수
    [SerializeField] Image[] button; // 환생 버튼
    [SerializeField] Sprite notChecked; // 체크 안된 스프라이트
    [SerializeField] Sprite isChecked; // 체크 된 스프라이트

    // 책 UI의 활성화 상태를 변경
    public void OpenBook(Soul soul)
    {
        BookUI.SetActive(!BookUI.activeSelf); // 상태 변경 후, 활성화 되었다면 ?
        if (BookUI.activeSelf)
        {
            InitBook(soul); // 초기화
            ChangeBookPage1(); // 책 1페이지로
        } 
    }

    // 책 초기화 메서드
    public void InitBook(Soul soul)
    {
        nameInputField.text = soul.name; // 이름을 표시
        for (int i = 0; i < 6; i++)
        {
            emotions[i].text = soul.emotions[i].ToString(); // 감정을 표시
        }

        // 애니메이션 설정
        for (int i = 0; i < 2; i++)
        {
            soulImages[0].SetInteger("body", soul.customizes[0]);
            soulImages[0].SetInteger("eyes", soul.customizes[1]);
            soulImages[0].SetInteger("bcolor", soul.customizes[2]);
            soulImages[0].SetInteger("ecolor", soul.customizes[3]);
        }

        // 환생 횟수 설정
        revive.text = "총 " + soul.revive.ToString() + " 번째";
        // 버튼 초기화
        for(int i  = 0; i < 6; i++)
        {
            // 환생 옵션 꼭 없앨것
            button[i].sprite = notChecked;
        }

        // 스킬 텍스트 설정
        for (int i = 0; i < 3; i++)
        {
            Skill skill = SkillManager.instance.GetSkillInfo(soul.parameters[i]);
            skillname[i].text = skill.name;
            skillDescription[i].text = skill.description;
            tier.text = skill.tier;
            if (tier.text.Equals("전설")) tier.color = Color.magenta;
            if (tier.text.Equals("신화")) tier.color = Color.red;
        }
    }

    // 정령 페이지로 변경
    public void ChangeBookPage1()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 0) BookPage[i].SetActive(true); // 이 페이지를 활성화하고 
            else BookPage[i].SetActive(false); // 다른 페이지를 활성화하지 않음
        }
    }

    // 스킬 페이지로 변경
    public void ChangeBookPage2()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 1) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }
    }

    // 환생 페이지로 변경
    public void ChangeBookPage3()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 2) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }
    }
}
