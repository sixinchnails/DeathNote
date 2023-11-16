using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Opening2Manager : MonoBehaviour
{
    public TalkManager talkManager;
    //public NextScript2 next;

    int talkIdx = -1;

    void Start()
    { 
        BoxAppear();
    }

    public void BoxAppear()
    {
        talkManager.BoxAppear(true);
        //scriptBox.SetBool("isShow", true);
        Action();
    }
    public void click()
    {
        print("클릭");
        TalkManager.instance.click();
        //button.interactable = false;
        //audioSource.Play();
        Action();
    }

    //다음 대사로 넘어가기 위해 talkIdx++ 해준다.
    public void Action()
    {
        talkIdx++;
        Talk();
    }

    void Talk()
    {
        bool isFinish = TalkManager.instance.ChangeUI(-1, talkIdx);
        print("대사가 시작했다");
        if (isFinish)
        {
            //진행도 100으로 업데이트
            //UserManager.instance.SaveData();
            TalkManager.instance.BoxAppear(false);
            //대화 끝났으면 튜토리얼으로 넘어간다.
            //SceneManager.LoadScene("Tutorial");
            LoadingController.LoadScene("MainScene");
        }
        else
        {
            switch (talkIdx)
            {

            }
        }
    }
}
