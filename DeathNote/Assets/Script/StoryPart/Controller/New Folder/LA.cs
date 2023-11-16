using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class La : MonoBehaviour
{
    public static La instance;
    public TalkManager talkManager;
    //public OpeningManager manager;

    public GameObject book;
    public GameObject rBook;
    public GameObject soul;

    int talkIdx = -1;

    private void Awake()
    {
        soul.SetActive(false);
    }

    void Start()
    {
        TalkManager.instance.SetBackground("landscape");
    }

    public void BoxAppear()
    {
        Invoke("box", 1.0f);
    }

    void box()
    {
        book.SetActive(false);
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

    async Task Talk()
    {
        if(talkIdx == 6)
        {
            await Soul();
        }
        bool isFinish = TalkManager.instance.ChangeUI(0, talkIdx);
        print("대사가 시작했다");
        if (isFinish)
        {
            TalkManager.instance.BoxAppear(false);
            //대화 끝났으면 튜토리얼으로 넘어간다.
            MusicManager.instance.SetTutorial();
            MusicManager.instance.tutorial = true;
            SceneManager.LoadScene("RhythmGameScene");
            //LoadingController.LoadScene("Tutorial");
        }
        else
        {
            switch (talkIdx)
            {
                case 2: book.SetActive(false); rBook.SetActive(true); break;
                case 5: rBook.SetActive(false); break;
                case 6: soul.SetActive(false); break;   
            }
        }

    }

    async Task Soul()
    {
        soul.SetActive(true);
        await Task.Delay(1900);
    }
}

