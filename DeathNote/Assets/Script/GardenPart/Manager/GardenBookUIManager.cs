using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GardenBookUIManager : MonoBehaviour
{

    public string[] custom;
    [SerializeField] GameObject BookUI;
    [SerializeField] GameObject[] BookPage;
    [SerializeField] Transform soulImage;
    [SerializeField] Transform soulImage2;
    [SerializeField] Text soulname;
    [SerializeField] GameObject tier;
    [SerializeField] Text skillname;
    [SerializeField] Text skillDescription;
    [SerializeField] Text[] emotions;
    [SerializeField] Text[] parameters;
    [SerializeField] Text revive;

    // 책 UI의 활성화 상태를 변경
    public void OpenBook(Soul soul)
    {
        BookUI.SetActive(!BookUI.activeSelf);
        if (BookUI.activeSelf)
        {
            custom = new string[] { "Body", "Eyes", "Accessary", "Skin" };
            soulname.text = soul.name;
            for(int i = 0; i < 6; i++)
            {
                emotions[i].text = soul.emotions[i].ToString();
                if(i<4)
                {
                    Image image = soulImage.GetChild(i).GetComponent<Image>();
                    Image image2 = soulImage2.GetChild(i).GetComponent<Image>();
                    string str = "Image/Character/Souls/" + custom[i] + "/" + soul.customizes[i];
                    image.sprite = Resources.Load<Sprite>(str);
                    image2.sprite = Resources.Load<Sprite>(str);
                    parameters[i].text = soul.parameters[i].ToString();
                }
                    
            }


            

            revive.text = "총 " + soul.revive.ToString() + " 번째";

            Skill skill = SkillManager.instance.GetSkill(soul.customizes[3]);
            skillname.text = skill.name;
            skillDescription.text = skill.description;
            tier.transform.GetChild(0).GetComponent<Text>().text = skill.tier;


        }
        ChangeBookPage1();
    }

    // 정보 페이지로 변경
    public void ChangeBookPage1()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 0) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }
    }

    public void ChangeBookPage2()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 1) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }
    }

    public void ChangeBookPage3()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 2) BookPage[i].SetActive(true);
            else BookPage[i].SetActive(false);
        }
    }
}
