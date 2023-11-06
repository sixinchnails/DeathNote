using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, List<TalkData>> scriptList;
    //오프닝신은 0번 엔딩 신은 7번 1~6번은 월드
    public int storyId;

    void Awake()
    {
        //초기화
        //int가 스토리씬 번호
        scriptList = new Dictionary<int, List<TalkData>>();
        GenerateData();
    }

    void GenerateData()
    {
        List<TalkData> openingData =  new List<TalkData> {
            new TalkData(1, "『··· 이게 뭐지』"),
            new TalkData(2, "『앗! 떨어트렸다』"),
            new TalkData(1, "『노트가 다 찢어져있네』"),
            new TalkData(1, "『악보를 멀리한 지도 오래됐구나··· 』"),
            new TalkData(1, "『오랜만에 연주나 해볼까?』")
        };
        scriptList.Add(0, openingData);

        List<TalkData> opening2Data = new List<TalkData> {
            new TalkData(2, "『너···! 어떻게 정령을 불러낸 거지?』"),
            new TalkData(1, "『어어···!  너··· 넌 뭐야!』"),
            new TalkData(2, "『악보를 완벽히 연주해야만 정령이 풀려나는데···』"),
            new TalkData(2, "『난 악(樂)마야』"),
            new TalkData(2, "『그 악보를 보관하고 있었지』"),
            new TalkData(2, "『악보엔 정령들이 봉인되어 있어』"),
            new TalkData(2, "『하지만, 악보가 다 찢어져서 정령들이 뿔뿔이 흩어지고 말았어···』"),
            new TalkData(2, "『너라면 악보를 찾아 정령을 풀어줄 수 있을 것 같아!』"),
            new TalkData(2, "『월드 버튼을 눌러봐. 나랑 악보를 찾아 여행을 떠나자!』")
        };
        scriptList.Add(-1, opening2Data);


        //이렇게 각 스토리 씬마다 대사 넣고 storyId에 씬번호 넣어서 출력하면 됨.
        //월드1개마다 스토리 4개 들어가야되니까 대사 여러개 넣어놓고 랜덤으로 출력하게할까?
        //물어보는거임

        List<TalkData> endingData = new List<TalkData>
        {
            new TalkData(1, "『···  ···  !』"),
            new TalkData(1, "『이 노래··· 어딘가 익숙한데?』"),
            new TalkData(1, "『내가 어릴 때 작곡한 노래잖아?!』"),
            new TalkData(2, "『정령을 찾아줘서 고마워 그럼 이만···』"),
            new TalkData(1, "『안녕···』"),
        };
        scriptList.Add(7, endingData);
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
