using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public FlashBack flashback;
    public Credit credit;

    public GameObject book;
    public GameObject past1;
    public GameObject past2;
    public GameObject obj1;
    public Image img1;
    public GameObject obj2;
    public Image img2;
    public GameObject cryReaper;

    int talkIdx = -1;

    private void Awake()
    {
        book.SetActive(false);
        past1.SetActive(false);
        past2.SetActive(false);
    }

    private void Start()
    {
        TalkManager.instance.BoxAppear(true);
        Action();
    }

    public void click()
    {
        print("클릭");
        Action();
    }

    public void Action()
    {
        talkIdx++;
        Talk();
    }

    async void Talk()
    {
        if(talkIdx == 3)
        {
            await flashback.show(obj1, img1);
            await flashback.hide(obj1, img1);
            await flashback.show(obj2, img2);
            await flashback.hide(obj2, img2);

        }
        bool isFinish = TalkManager.instance.ChangeUI(7, talkIdx);
        print("대사가 시작했다");
        if (isFinish)
        {
            cryReaper.SetActive(true);
            //credit.up();
            Invoke("creditUp", 2.5f);
            //진행도 800으로 업데이트
            UserManager.instance.userData.progress = 800;
            UserManager.instance.SaveData();
            TalkManager.instance.BoxAppear(false);
            //대화 끝났으면 튜토리얼으로 넘어간다.
            Invoke("loadScene", 14.5f);
        }
        else
        {
            switch (talkIdx)
            {
                case 1: bookShow(); break;
            }
        }
    }

    void bookShow()
    {
        book.SetActive (true);
    }

    void creditUp()
    {
        cryReaper.SetActive(false);
        credit.up();
    }

    void loadScene()
    {
        LoadingController.LoadScene("MainScene");
    }
}
