using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScript : MonoBehaviour
{
    public TalkManager talkManager;
    public OpeningManager manager;

    public Animator scriptBox;
    public GameObject dark;
    public Text nickname; //나중에 유저 닉네임 받아와서 넣을거임
    public Text content;
    public Text nickname2; //사신
    public Text content2;
    public GameObject book;
    public GameObject rBook;
    public GoBackR goBackR;
    public GoBackM goBackM;
    public GameObject me;
    public Button button;
    public GameObject backgroundN;

    AudioSource audioSource;

    int talkIdx = -1;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void BoxAppear()
    {
        dark.SetActive(true);
        //대사 박스 나타난다.
        scriptBox.SetBool("isShow", true);
        Action();
    }

    public void click()
    {
        //대사 시작하면 버튼 비활성화
        button.interactable = false;
        audioSource.Play();
        Action();
    }

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
        //대사 끝나면 다시 버튼 활성화
        button.interactable = true;
    }
    void Talk()
    {
        backgroundN.SetActive(false);
        content.text = null;
        nickname.text = null;
        content2.text = null;
        nickname2.text = null;
        int storyId = talkManager.getStoryId();
        TalkData data = talkManager.getTalk(storyId, talkIdx);
        if (talkIdx == 1)
        {

        }
        if (talkIdx == 2)
        {
            book.SetActive(false);
            rBook.SetActive(true);
        }

        if (data == null)
        {
            scriptBox.SetBool("isShow", false);
            //대화 끝났으면 튜토리얼으로 넘어간다.
            SceneManager.LoadScene("Tutorial");
        }
        else if (data.id == 1)
        {
            goBackR.back();
            goBackM.forward();
            nickname.text = "사용자 닉네임 들어갈거임";
        }
        else if (data.id == 2)
        {
            backgroundN.SetActive(true);
            goBackR.forward();
            goBackM.back();
            nickname2.text = "악마";
        }
        StartCoroutine(Typing(data.content, data.id));
    }
}
