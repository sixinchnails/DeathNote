using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSoulScript : MonoBehaviour
{
    public TalkManager talkManager;

    Text content;

    private void Awake()
    {
        content = GetComponent<Text>();
    }

    void Start()
    {
        int storyId = talkManager.getStoryId();
        int talkIdx = Random.Range(0, 4);
        print(talkIdx);
        TalkData data = talkManager.getTalk(storyId, talkIdx);
        StartCoroutine(Typing(data.content, 0));
    }

    IEnumerator Typing(string str, int data)
    {
        content.text = null;
        for (int i = 0; i < str.Length; i++)
        {
            content.text += str[i];
            yield return new WaitForSeconds(0.05f);
        }
    }

}
