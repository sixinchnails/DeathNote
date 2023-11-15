using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class La : MonoBehaviour
{
    public TalkManager talkManager;
    public OpeningManager manager;

    public GameObject book;
    public GameObject rBook;


    int talkIdx = -1;

    void Start()
    {
        TalkManager.instance.SetBackground("nightsky");
    }

    public void BoxAppear()
    {
        TalkManager.instance.BoxAppear(true);
        Action();
    }
    public void click()
    {
        TalkManager.instance.click();
        Action();
    }
    public void Action()
    {
        Debug.Log("g ");
        talkIdx++;
        Talk();
    }

    void Talk()
    {
        bool isFinish = TalkManager.instance.ChangeUI(0, talkIdx);
        if (isFinish)
        {
            TalkManager.instance.BoxAppear(false);
            //대화 끝났으면 튜토리얼으로 넘어간다.
            SceneManager.LoadScene("Tutorial");
            //LoadingController.LoadScene("Tutorial");
        }
        else
        {
            switch (talkIdx)
            {
                case 2: book.SetActive(false); rBook.SetActive(true); break;
            }
        }

    }
}

