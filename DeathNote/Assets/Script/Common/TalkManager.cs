using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, List<TalkData>> scriptList;
    //오프닝신은 0번 엔딩 신은 7번 1~6번은 월드
    public int storyId;

    void Awake()
    {
        //초기화
        scriptList = new Dictionary<int, List<TalkData>>();
        GenerateData();
    }

    void GenerateData()
    {
        List<TalkData> openingData =  new List<TalkData> {
            new TalkData(1, "『어, 이게 뭐지』"),
            new TalkData(1, "『다 찢어져있네』"),
            new TalkData(1, "『오랜만에 연주나 해볼까』")
        };

        scriptList.Add(0, openingData);
    }

    public int getStoryId()
    {
        return storyId;
    }

    public TalkData getTalk(int id, int idx)
    {
        if (idx == scriptList[id].Count) 
            return null; //대화 끝나면 null
        List<TalkData> data = scriptList[id];
        return data[idx];
    }
}
