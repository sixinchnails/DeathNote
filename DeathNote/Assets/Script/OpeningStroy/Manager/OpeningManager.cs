using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningManager : MonoBehaviour
{
    public TalkManager talkManager;
    //public GameObject scriptBox;
    public Animator scriptBox;
    public Text nickname; //나중에 유저 닉네임 받아와서 넣을거임
    public Text content;

    int talkIdx=-1;

    void Awake()
    {
        //scriptBox.SetActive(false);
    }

    public void BoxAppear()
    {
        //대사 박스 나타난다.
        scriptBox.SetBool("isShow", true);
        Invoke("Action", 1);
        //return;
    }

    //다음 대사로 넘어가기 위해 talkIdx++ 해준다.
    public void Action()
    {
        talkIdx++;
        Talk();
    }

    IEnumerator Typing(string str)
    {
        content.text = null;
        for(int i=0; i<str.Length; i++)
        {
            content.text += str[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
    void Talk()
    {
        int storyId = talkManager.getStoryId();
        TalkData data = talkManager.getTalk(storyId, talkIdx);
        if (data == null) 
        {
            scriptBox.SetBool("isShow", false);
            //대화 끝났으면 다음 화면으로 넘어간다.
            return;
        }
        if(data.id == 0)
        {
            nickname.text = "";
        }
        else if(data.id == 1)
        {
            nickname.text = "사용자 닉네임 들어갈거임";
        }
        else if(data.id == 2)
        {
            nickname.text = "사신";
        }
        StartCoroutine(Typing(data.content));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
