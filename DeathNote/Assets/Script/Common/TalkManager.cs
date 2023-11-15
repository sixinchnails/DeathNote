using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class TalkData
{
    //id 0이면 설명문, 1이면 유저 대사, 2이면 사신 대사, 3이면 사신인데 가려짐, 4이면 정령 대사
    public int id;
    public string content;
    public string background;
    public int opp;
    public float delay;

    public TalkData(int id, string content)
    {
        this.id = id;
        this.content = content;
        this.background = null;
    }
    public TalkData(int id, string content, string background)
    {
        this.id = id;
        this.content = content;
        this.background = background;
    }
    public TalkData(int id, string content, string background, float delay)
    {
        this.id = id;
        this.content = content;
        this.background = background;
        this.delay = delay;
    }
}

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;
    [SerializeField] public RectTransform[] rects;
    [SerializeField] public Image[] images;
    [SerializeField] public Sprite[] sprites;
    [SerializeField] Text sayname;
    [SerializeField] Text content;
    [SerializeField] Animator[] textAnimation;
    [SerializeField] Button button;
    [SerializeField] Animator scriptBox;
    [SerializeField] Image background;
    AudioSource audioSource;

    Dictionary<int, List<TalkData>> scriptList;
    //오프닝신은 0번 엔딩 신은 7번 1~6번은 월드
    public int storyId;


    void Awake()
    {
        //초기화
        instance = this;
        //int가 스토리씬 번호
        scriptList = new Dictionary<int, List<TalkData>>();
        GenerateData();
        audioSource = GetComponent<AudioSource>();  
    }

    void GenerateData()
    {
        // 오프닝 데이터
        List<TalkData> openingData = new List<TalkData> {
            new TalkData(1, "『··· 이게 뭐지』", "landscape"),
            new TalkData(2, "『앗! 떨어트렸다』", "nightsky"),
            new TalkData(1, "『노트가 다 찢어져있네』","landscape"),
            new TalkData(1, "『악보를 멀리한 지도 오래됐구나··· 』"),
            new TalkData(1, "『오랜만에 연주나 해볼까···?』"),
            new TalkData(1, "『아냐, 됐어.. 이제 악기라면 지긋지긋···"),
            new TalkData(4, "[[ ♩♬ - ! ]]"),
            new TalkData(1, "『뭐, 뭐야 이건...?』", "landscape"),
            new TalkData(1, "『생긴 건 조팡맹이처럼 생긴게 진짜 시끄럽네···』"),
            new TalkData(1, "『이 악보를 연주하라는 건가···?"),
            new TalkData(1, "『별 수 없네, 그럼 한번 연주해볼까···"),
        };
        scriptList.Add(0, openingData);

        List<TalkData> opening2Data = new List<TalkData> {
            new TalkData(1, "『···오랜만이니까 손이 많이 굳었네.", "landscape"),
            new TalkData(1, "『자, 이제 됐으니까 사라져 줄래 조팡맹이야?"),
            new TalkData(2, "『너···! 어떻게 정령을 불러낸 거지?』"),
            new TalkData(1, "『어어···!  너··· 넌 뭐야!』"),
            new TalkData(2, "『정령은 뛰어난 음악가의 냄새를 맡아야만 풀려나는데···』"),
            new TalkData(2, "『난 악보를 보관하는 악(樂)마야』"),
            new TalkData(2, "『악보엔 정령들이 봉인되어 있어』"),
            new TalkData(2, "『하지만, 악보가 다 찢어져서 정령들이 뿔뿔이 흩어지고 말았어···』"),
            new TalkData(2, "『너라면 악보를 찾아 정령을 풀어줄 수 있을 것 같아!』"),
            new TalkData(2, "『월드 버튼을 눌러봐. 나랑 악보를 찾아 여행을 떠나자!』")
        };
        scriptList.Add(-1, opening2Data);

        List<TalkData> soulGetData = new List<TalkData> {
            new TalkData(0, "『0번대사 정령을 얻었다!』", "landscape"),
            new TalkData(0, "『1번대사』"),
            new TalkData(0, "『2번대사』"),
            new TalkData(0, "『3번대사』"),
            new TalkData(0, "『4번대사』"),
        };
        scriptList.Add(2, soulGetData);
        //이렇게 각 스토리 씬마다 대사 넣고 storyId에 씬번호 넣어서 출력하면 됨.
        //월드1개마다 스토리 4개 들어가야되니까 대사 여러개 넣어놓고 랜덤으로 출력하게할까?
        //물어보는거임

        List<TalkData> endingData = new List<TalkData>
        {
            new TalkData(1, "『···  ···  !』", "landscape"),
            new TalkData(1, "『이 노래··· 어딘가 익숙한데?』"),
            new TalkData(1, "『내가 어릴 때 작곡한 노래잖아?!』"),
            new TalkData(2, "『정령을 찾아줘서 고마워 그럼 이만···』"),
            new TalkData(1, "『안녕···』"),
        };
        scriptList.Add(7, endingData);
    }


    public void SetBackground(string bg)
    {
        background.sprite = Resources.Load<Sprite>("Image/Background/" + bg);
    }

    public int getStoryId()
    {
        return storyId;
    }


    public void BoxAppear(bool param)
    {
        //대사 박스 나타난다.
        scriptBox.SetBool("isShow", param);

    }

    public void click()
    {
        //대사 시작하면 버튼 비활성화
        button.interactable = false;
        audioSource.Play();
    }

    public TalkData getTalk(int id, int idx)
    {
        if (idx == scriptList[id].Count) 
            return null; //대화 끝나면 null
        List<TalkData> data = scriptList[id];
        return data[idx];
    }

    public bool ChangeUI(int id, int idx)
    {
        TalkData data = getTalk(id, idx);
        if (data == null) return true;

        if(data.id == 1)
        {
            images[0].color = new Color(255, 255, 255, 1);
            rects[0].SetAsLastSibling();
            images[1].color = new Color(255, 255, 255, 0.5f);
            rects[1].SetAsFirstSibling();
            //sayname.text = UserManager.instance.userData.nickname;
            sayname.text = "닉네임 들어갈거임";
            sayname.alignment = TextAnchor.MiddleLeft;
            content.alignment = TextAnchor.MiddleLeft;

            StartCoroutine(Speaking(textAnimation[0]));

        }

        else
        {
            images[1].sprite = sprites[data.id - 1];
            images[1].color = new Color(255, 255, 255, 1);
            rects[1].SetAsLastSibling();
            for (int i = 1; i <= 3; i++)
            {
                if (i == data.id - 1)
                {
                    if (i == 1) sayname.text = "악마";
                    else if (i == 2) sayname.text = "???";
                    else if (i == 3) sayname.text = "뽀짝이";
                }
            }
            images[0].color = new Color(255, 255, 255, 0.5f);
            rects[0].SetAsFirstSibling();

            
            sayname.alignment = TextAnchor.MiddleRight;
            content.alignment = TextAnchor.MiddleRight;

            StartCoroutine(Speaking(textAnimation[1]));
            

        }
        if(data.background != null)
        {
            background.sprite = Resources.Load<Sprite>("Image/Background/" + data.background);
        }

        StartCoroutine(Typing(data.content,data.delay));
        
        return false;
    }

    IEnumerator Speaking(Animator animator)
    {
        ;
        yield return new WaitForSeconds(1);
        animator.SetBool("turn", false);
    }

    IEnumerator Typing(string str, float delay)
    {
        yield return new WaitForSeconds(delay);
        content.text = null;
        for (int i = 0; i < str.Length; i++)
        {
            content.text += str[i];
            yield return new WaitForSeconds(0.06f);
        }
        //대사 끝나면 다시 버튼 활성화
        button.interactable = true;
    }

}
