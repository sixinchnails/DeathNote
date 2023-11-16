using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoulGet : MonoBehaviour
{
    [SerializeField] GameObject soulGetUI; // 정령을 얻는 UI
    [SerializeField] GameObject animationUI; // 정령 얻고난 뒤 애니메이션 UI
    [SerializeField] GameObject confirmUI; // 정령 얻기 전의 UI
    [SerializeField] TextMeshProUGUI currentCapacity; // 현재 Garden의 Capacity
    [SerializeField] TextMeshProUGUI currentCost; // 현재 정령을 뽑는데 필요한 영감
    [SerializeField] TextMeshProUGUI purchaseButton; // 구매가 가능한지 아닌지
    [SerializeField] Animator getAnimator;
    [SerializeField] GardenManager gardenManager;
    [SerializeField] Transform soulImage;

    private bool isPurchase = false;
    // [SerializeField] TextMeshProUGUI alert;

    // 스킬 : 노말 6개


    // 제일 먼저, UI를 띄워야 한다. 버튼을 이용한 UI다.
    public void EnableUI()
    {


         animationUI.SetActive(false);

        if (!soulGetUI.activeSelf)
        {
            gardenManager.CloseAllUi(); // 모든 UI 창을 닫음
            soulGetUI.SetActive(true);
        }
        else
        {
            soulGetUI.SetActive(false);
        }

        confirmUI.SetActive(true); // 닫힐때나 열릴때나 기본은 무조건 true여야함
        InitUI();
    }

    // UI를 띄웠을 때, 들어가야 하는 값들을 초기화하는 메서드
    public void InitUI()
    {
        int cost = gardenManager.capacity * 1000;
        currentCost.text = cost.ToString(); // 현재 정령수의 1000을 곱한 가격
        if (gardenManager.capacity == 16) // 16마리라면
        {
            purchaseButton.text = "더 이상 소환할 수 없습니다.";
        }
        else if (UserManager.instance.userData.gold < cost) // 돈이 없다면
        {
            purchaseButton.text = "영감이 부족합니다.";
        }
        else
        {
            purchaseButton.text = "소환";
        }
    }
    

    // 정령을 소환하는 메서드
    public void PurchaseSoul()
    {
        int cost = gardenManager.capacity * 1000; // 정령뽑는데 드는 비용
        if(UserManager.instance.userData.gold >= cost && gardenManager.capacity < 16){ // 조건문
            UserManager.instance.userData.gold -= cost; // 영감 감소

            Soul soul = MakeSoul(); // 정령 만들기

            confirmUI.SetActive(false); // 확인창은 없애자


            gardenManager.NewSoul(soul); // 새로운 정령 등록
            gardenManager.UpdateInspirit(); // UI 초기화

            StartCoroutine(StartAnimation(soul));
        }
    }

    // 정령 획득 애니메이션을 틀고, 2초뒤에 닫는다.
    IEnumerator StartAnimation(Soul soul)
    {
        animationUI.SetActive(true);
        getAnimator.SetInteger("body", soul.customizes[0]);
        getAnimator.SetInteger("eyes", soul.customizes[1]);
        getAnimator.SetInteger("bcol", soul.customizes[2]);
        getAnimator.SetInteger("ecol", soul.customizes[3]);
        yield return new WaitForSeconds(2.0f);

        animationUI.SetActive(false);
    }



    // 정령을 새롭게 만드는 메서드
    public Soul MakeSoul()
    {
        int[] customizes = new int[4]; // 꾸미는거 4개
        int[] parameters = new int[4]; // 스킬 3개 + 친밀도
        int[] emotions = new int[6]; // 감성 6개

        customizes[0] = UnityEngine.Random.Range(1, 7); // 1부터 6까지 몸통
        customizes[1] = UnityEngine.Random.Range(1, 3); // 1부터 2까지 눈


        // 난수에 따라 색깔을 바꿔줌
        int chance = UnityEngine.Random.Range(0, 100);

        if (chance > 70)
        {
            customizes[2] = UnityEngine.Random.Range(1, 11);

        }
        chance = UnityEngine.Random.Range(0, 100);

        if (chance > 50)
        {
            customizes[3] = UnityEngine.Random.Range(1, 11);

        }



        // 스킬뽑기
        for(int i = 0; i < 3; i++)
        {
            chance = UnityEngine.Random.Range(0, 100);
            if (chance == 0)
            {
                parameters[i] = 200; // 신화 스킬
            }
            else if (chance < 10)
            {
                parameters[i] = 100 + UnityEngine.Random.Range(0, 3); // 전설 스킬
            }
            else
            {
                parameters[i] = UnityEngine.Random.Range(0, 6); // 일반 스킬
            }
        }

        parameters[3] = UnityEngine.Random.Range(3, 10);

        // 감정은 5부터 20
        for (int i = 0; i < 6; i++)
        {
            emotions[i] = UnityEngine.Random.Range(5, 20);
        }

        int id = UserManager.instance.userData.souls.Count;
        //return new Soul(nameInputField.text, -1, parameters, customizes, emotions, 0, gardenManager.location);
        return new Soul(id, "새 정령", -1, parameters, customizes, emotions, 0, gardenManager.location);
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
