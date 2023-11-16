using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



// 노래 선택 페이지
public class MusicSelect : MonoBehaviour
{
    //x 리미트를 둔다. 0, -1920, -3840, -5760
    public static int[] stage = { 0, -1, -2, -3 };
    public static int idx = 0; // 페이지
    public static int limit = 0;
    private int w;
    private int h;
    // 내UIS
    [SerializeField] GameObject stageUI;
    [SerializeField] GameObject[] backgrounds;
    [SerializeField] Text songTitle; // 노래 제목
    [SerializeField] Image songThumb; // 노래 사진
    [SerializeField] Sprite[] songThumbnailData; // 스프라이트
    [SerializeField] Text highScore; // 내 최고 점수
    [SerializeField] Text highGrade; // 내 최고 등급

    public GameObject panel;
    // 랭킹UI
    [SerializeField] GameObject rankPanel; // 랭킹을 가리는 패널
    [SerializeField] GameObject[] rankUI; // 랭크 UI
    Text[] nicknames; // 랭커 닉네임
    Text[] scores; // 랭커 점수 
    Animator[] repres; // 랭커 대표 정령
    
    // 게임준비 UI
    [SerializeField] GameObject startUI; // UI 준비
    [SerializeField] Animator[] sessions; // 내 세션
    [SerializeField] Text customOffsetTxt; // 내 오프셋

    Coroutine recordCoroutine; // 랭킹 코루틴
    public string[] songTitlesData; // 노래제목
    public int[] songProgressData; // 노래코드
    float customOffset; // 현재 오프셋
    public int world;

    public void Awake()
    {
        idx = 0; // 첫 페이지
        // newChild를 stageUI의 자식으로 설정
        world = MusicManager.instance.nowWorld;
        backgrounds[world].SetActive(true);
        nicknames = new Text[20];
        scores = new Text[20];
        repres = new Animator[20];
        for(int i = 0; i < 20; i++)
        {
            int plus = 0;
            if (i < 3) plus = 1; // 1~3위는 한칸씩 밀린다.

            nicknames[i] = rankUI[i].transform.GetChild(1 + plus).GetComponent<Text>(); // 닉네임 설정
            scores[i] = rankUI[i].transform.GetChild(2 + plus).GetComponent<Text>(); // 점수 설정
            repres[i] = rankUI[i].transform.GetChild(3 + plus).GetChild(0).GetComponent<Animator>(); // 정령 설정
        }

        LoadRanking(); // UI 호출
        if (UserManager.instance.userData.progress + 1 < songProgressData[world * 4 + idx]) panel.SetActive(true);
        else panel.SetActive(false);
        MusicManager.instance.SetMusic(songProgressData[world * 4 + idx], customOffset);



        limit = 0;
        w = Screen.width;
        h = Screen.height;
        
        if(w > 1920)
        {
            w = (int)Mathf.Round(16 * (h / 9));
        }
    }

    // UI에 정보를 채우고, 랭킹 정보를 호출
    public void LoadRanking()
    {
        if(recordCoroutine != null) StopCoroutine(recordCoroutine); // 비동기 통신이 진행중이면 멈춘다.

        songTitle.text = songTitlesData[world*4 + idx]; // 노래 제목 설정
      
        songThumb.sprite = songThumbnailData[world * 4 + idx]; // 노래 사진 설정
        int score = 0;
        float grade = 0.0f;
        foreach (RecordData record in UserManager.instance.userData.record)
        {
            if (record.code == songProgressData[world * 4 + idx])
            {
                score = record.score; // 내 기록에서 최고 점수를 꺼내온다.
                grade = record.grade; // 내 기록에서 최고 등급을 꺼내온다.
            }
        }
        highScore.text = score.ToString(); // 점수 설정
        highGrade.text = grade.ToString("F2")+"%";  // 등급 설정
        float savedOffset = PlayerPrefs.GetFloat(songProgressData[world * 4 + idx].ToString(), 0.0f); // 해당 노래의 저장된 오프셋을 꺼내온다.
        customOffset = savedOffset; // 오프셋 설정
        customOffsetTxt.text = customOffset.ToString(); // 오프셋 텍스트 설정
        
        RecordManager.instance.GetGlobalRanking(songProgressData[world * 4 + idx]); // 비동기 통신으로 노래의 전체 랭킹을 불러온다.

        recordCoroutine = StartCoroutine(GetRecord());
    }

    // 비돟기 통신은 코루틴
    IEnumerator GetRecord()
    {
        List<RecordData> rank = RecordManager.instance.globalRecords; // 전체 기록을 가져온다.
        while( rank == null )
        {
            rank = RecordManager.instance.globalRecords; // 전체 기록을 가져온다.
            yield return null; // 기록이 없을 때는 코루틴 반복
        }
        for (int i = 0; i < 20; i++)
        {
            if (rank.Count > i && rank[i] != null)
            {
                nicknames[i].text = rank[i].nickname; // 랭커 닉네임을 설정한다.
                scores[i].text = rank[i].score.ToString();  // 랭커 점수를 설정한다.
                Soul soul = JsonUtility.FromJson<Soul>(rank[i].data); // 랭커 대표 정령 사진을 설정한다.
                repres[i].SetInteger("body", soul.customizes[0]);
                repres[i].SetInteger("eyes", soul.customizes[1]);
                repres[i].SetInteger("bcol", soul.customizes[2]);
                repres[i].SetInteger("ecol", soul.customizes[3]);
            }
            else
            {
                nicknames[i].text = "?????"; // 랭커 닉네임을 설정한다.
                scores[i].text = "??????";  // 랭커 점수를 설정한다.
                repres[i].SetInteger("body", 0);
                repres[i].SetInteger("eyes", -1);
                repres[i].SetInteger("bcol", -1);
                repres[i].SetInteger("ecol", 0);
            }

        }

    }

    // 노래를 준비하는 메서드
    public void ReadyToStart()
    {
        if (startUI.activeSelf) startUI.SetActive(false); // 켜져있으면 끈다
        else
        {
            startUI.SetActive(true);
            // 애니메이터를 설정하기 위해 내 장비된 정령을 불러온다.
            List<Soul> mySession = SkillManager.instance.equip;

            startUI.SetActive(true);
            for (int i = 0; i < 6; i++)
            { 
                if (mySession[i] != null)
                {
                    sessions[i].SetInteger("body", mySession[i].customizes[0]);
                    sessions[i].SetInteger("eyes", mySession[i].customizes[1]);
                    sessions[i].SetInteger("bcol", mySession[i].customizes[2]);
                    sessions[i].SetInteger("ecol", mySession[i].customizes[3]);
                }
                else
                {
                    sessions[i].SetInteger("body", 0);
                    sessions[i].SetInteger("eyes", -1);
                    sessions[i].SetInteger("bcol", -1);
                    sessions[i].SetInteger("ecol", 0);
                }
            }
            customOffset = PlayerPrefs.GetFloat(songProgressData[idx].ToString(), 0.0f); // 가져오기
            customOffsetTxt.text = customOffset.ToString("F2");
        }

    }
    // 오프셋 업
   public void OffsetUp()
    {
        customOffset += 0.01f;
        customOffset = Mathf.Min(customOffset, 2); // customOffset이 2를 넘지 않도록 함
        customOffset = Mathf.Round(customOffset * 100f) / 100f; // 소수점 둘째 자리에서 반올림
        customOffsetTxt.text = customOffset.ToString("F2");
    }
    // 오프셋 다운
    public void OffsetDown()
    {
        customOffset -= 0.01f;
        customOffset = Mathf.Max(customOffset, -22); // customOffset이 -2를 넘지 않도록 함
        customOffset = Mathf.Round(customOffset * 100f) / 100f; // 소수점 둘째 자리에서 반올림
        customOffsetTxt.text = customOffset.ToString("F2");
    }

    // 게임 시작
    public void StartGame()
    {
        Debug.Log(songProgressData[world * 4 + idx]);
        PlayerPrefs.SetFloat(songProgressData[idx].ToString(), customOffset); // 커스텀 오프셋 저장
        MusicManager.instance.SetMusic(songProgressData[world * 4 + idx], customOffset);
        LoadingController.LoadScene("RhythmGameScene");
    }



    public void MoveNext()
    {
        if (idx < 3) 
        {
            idx++;
            limit = stage[idx] * w;
            if(Mathf.Abs(limit % 8) == 1)
            {
                limit += 1;
            }else if (Mathf.Abs(limit % 8) == 2)
            {
                limit += 2;
            }
            else if (Mathf.Abs(limit % 8) == 3)
            {
                limit += 3;
            }
            else if (Mathf.Abs(limit % 8) == 4)
            {
                limit += 4;
            }
            else if (Mathf.Abs(limit % 8) == 5)
            {
                limit -= 3;
            }
            else if (Mathf.Abs(limit % 8) == 6)
            {
                limit -= 2;
            }
            else if (Mathf.Abs(limit % 8) == 7)
            {
                limit -= 1;
            }
            else
            {
            }
        }
        LoadRanking();
        if (UserManager.instance.userData.progress + 1 < songProgressData[world * 4 + idx]) panel.SetActive(true);
        else panel.SetActive(false);
        MusicManager.instance.SetMusic(songProgressData[world * 4 + idx], customOffset);

    }

    public void MoveBack()
    {
        if( idx > 0 )
        {
            idx--;
            limit = stage[idx] * w;
            if (Mathf.Abs(limit % 8) == 1)
            {
                limit -= 1;
            }
            else if (Mathf.Abs(limit % 8) == 2)
            {
                limit -= 2;
            }
            else if (Mathf.Abs(limit % 8) == 3)
            {
                limit -= 3;
            }
            else if (Mathf.Abs(limit % 8) == 4)
            {
                limit -= 4;
            }
            else if (Mathf.Abs(limit % 8) == 5)
            {
                limit += 3;
            }
            else if (Mathf.Abs(limit % 8) == 6)
            {
                limit += 2;
            }
            else if (Mathf.Abs(limit % 8) == 7)
            {
                limit += 1;
            }
        }
        LoadRanking();
        if (UserManager.instance.userData.progress + 1 < songProgressData[world * 4 + idx]) panel.SetActive(true);
        else panel.SetActive(false);
        MusicManager.instance.SetMusic(songProgressData[world * 4 + idx], customOffset);
    }
}
