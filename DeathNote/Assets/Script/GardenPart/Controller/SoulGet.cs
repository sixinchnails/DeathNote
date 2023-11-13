using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoulGet : MonoBehaviour
{
    [SerializeField] GameObject soulGetUI; // 정령을 얻는 UI
    [SerializeField] TextMeshProUGUI currentCapacity; // 현재 Garden의 Capacity
    [SerializeField] GameObject getUI;
    [SerializeField] Animator getAnimator;
    
    [SerializeField] GardenManager gardenManager;
    [SerializeField] Transform soulImage;
    [SerializeField] TMP_InputField nameInputField;
    // [SerializeField] TextMeshProUGUI alert;
    string soulName;
    string[] custom = new string[] { "Body", "Eyes", "Accessary", "Skin" };
    // 스킬 : 노말 6개


    // 제일 먼저, UI를 띄워야 한다. 버튼을 이용한 UI다.
    public void EnableUI(bool active)
    {
        soulGetUI.SetActive(active);
    }

    // UI를 띄웠을 때, 들어가야 하는 값들을 초기화하는 메서드
    public void InitUI()
    {
      
    }



    public void Test()
    {
        getUI.SetActive(false);
        Soul soul = MakeSoul();
        int[] cus = soul.customizes;
        Debug.Log(cus[0]);
        getUI.SetActive(true);
        getAnimator.SetInteger("body", cus[0]);
        getAnimator.SetInteger("eyes", cus[1]);
        getAnimator.SetInteger("bcol", cus[2]);
        getAnimator.SetInteger("ecol", cus[3]);

    }

    public void PurChaseSoul()
    {
        if (nameInputField.text == null || nameInputField.text.Length > 6) return;
        

        Soul soul = MakeSoul();

        for (int i = 0; i < 3; i++)
        {
            Image image = soulImage.GetChild(i).GetComponent<Image>();
            string str = "Image/Character/Souls/" + custom[i] + "/" + soul.customizes[i];
            image.sprite = Resources.Load<Sprite>(str);
        }

        getUI.SetActive(false);
        //confirmUI.SetActive(true);

        gardenManager.NewSoul(soul);

        UserManager.instance.userData.gold -= 1000;
        gardenManager.UpdateInspirit();

    }

    public Soul MakeSoul()
    {
        int[] customizes = new int[4];
        int[] parameters = new int[4];
        int[] emotions = new int[6];

        int chance = UnityEngine.Random.Range(0, 100);
        if( chance == 99) { return null; }

        customizes[0] = UnityEngine.Random.Range(1, 7);
        customizes[1] = UnityEngine.Random.Range(0, 3);

        chance = UnityEngine.Random.Range(0, 100);

        int[] a = { 0, 0 };

        if (chance > 70)
        {
            customizes[2] = UnityEngine.Random.Range(0, 11);

        }

        if (chance > 50)
        {
            customizes[3] = UnityEngine.Random.Range(0, 11);

        }



        // 스킬뽑기
        for(int i = 0; i < 3; i++)
        {
            chance = UnityEngine.Random.Range(0, 100);
            if (chance == 99)
            {

            }
            else if (chance < 10)
            {
                parameters[i] = UnityEngine.Random.Range(0, 3);
            }
            else
            {
                parameters[i] = UnityEngine.Random.Range(0, 6);
            }
        }

        // 무게
        parameters[3] = UnityEngine.Random.Range(10, 100);

        // 감정
        for(int i = 0; i < 6; i++)
        {
            emotions[i] = UnityEngine.Random.Range(5, 20);
        }

        //return new Soul(nameInputField.text, -1, parameters, customizes, emotions, 0, gardenManager.location);
        return new Soul("새 정령", -1, parameters, customizes, emotions, 0, gardenManager.location);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
