using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Opening2Manager : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject dark;
    public Animator scriptBox;
    public Text nickname; //나중에 유저 닉네임 받아와서 넣을거임
    public Text content;
    public Text nickname2; //사신
    public Text content2;
    public GoBackR goBackR;
    public GoBackM goBackM;

    int talkIdx = -1;

    private void Awake()
    {
        dark.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        BoxAppear();
    }

    public void BoxAppear()
    {
        dark.SetActive(true);
        //대사 박스 나타난다.
        scriptBox.SetBool("isShow", true);
        Invoke("Action", 1);
    }

    //다음 대사로 넘어가기 위해 talkIdx++ 해준다.
    public void Action()
    {
        talkIdx++;
        Talk();
    }

    IEnumerator Typing(string str, int data)
    {
        content.text = null;
        content2.text = null;
        for (int i = 0; i < str.Length; i++)
        {
            if (data == 1)
            {
                content.text += str[i];
            }
            else if (data == 2)
            {
                content2.text += str[i];
            }
            yield return new WaitForSeconds(0.06f);
        }
    }
    void Talk()
    {
        content.text = null;
        nickname.text = null;
        content2.text = null;
        nickname2.text = null;
        int storyId = talkManager.getStoryId();
        TalkData data = talkManager.getTalk(storyId, talkIdx);
        if (data == null)
        {
            scriptBox.SetBool("isShow", false);
            //대화 끝났으면 다음 화면으로 넘어간다.
            //SceneManager.LoadScene("");
            return;
        }
        if (data.id == 0)
        {
            nickname.text = "";
        }
        else if (data.id == 1)
        {
            goBackR.back();
            goBackM.forward();
            nickname.text = "사용자 닉네임 들어갈거임";
        }
        else if (data.id == 2)
        {
            goBackR.forward();
            goBackM.back();
            nickname2.text = "사신";
        }
        StartCoroutine(Typing(data.content, data.id));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
