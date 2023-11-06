using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class StageManager : MonoBehaviour
{
    AudioSource audioSource; // 오디오 정보
    MusicManager musicManager; // 노래 정보를 담고있는 객체

    public float areaX = 0; // 노트가 나타나는 영역의 가로길이
    public float[] areasY = new float[11]; // 노트가 나타나는 세로영역의 각 부분 높이

    public int beatNumber = 0; // 진행중인 현재의 비트 번호
    public double timePerBeat; // 비트 당 걸리는 시간
    public Queue<NoteData> noteQueue; // 노트 정보를 담을 큐

    public double currentTime; // 현재 게임 시간
    public double gameStart; // 게임 시작 시간
    public double songStart; // 노래 시작 시간

    private bool running = false; // 음악 실행중 여부
    private bool hasPlayed = false; // 음악 실행 여부
    
    [SerializeField] GameObject judgeLine; // 판정선
    [SerializeField] TextMeshProUGUI title; // 곡 제목
    Image[] noteDownImage;
    ResultManager resultManager;


    void Start()
    {
        noteQueue = new Queue<NoteData>();
        // 게임 시작 시간
        

        // MusicManager 싱글턴을 불러오고, 노래 설정
        musicManager = MusicManager.instance;
        musicManager.setSunset();
        title.text = musicManager.musicTitle;
        resultManager = FindObjectOfType<ResultManager>();
        audioSource = musicManager.audioSource;
        // bpm을 60으로 나눈 초당 비트수의 역수는 비트당 초
        timePerBeat = (60d / musicManager.bpm);
        // song은 2마디( musicManger.songBeat의 두배 )에서 시작
        songStart = timePerBeat * (musicManager.songBeat * 2);

        // 노래가 재생중이라고 변경
        running = true;
        // 노래가 재생된 이력이 있다고 변경
        hasPlayed = true;


        // 노트를 누르는 영역의 크기지정
        areaX = GetComponent<RectTransform>().rect.width;
        float areaY = GetComponent<RectTransform>().rect.height;
        noteDownImage = new Image[11];
        for (int i = 0; i < 11; i++)
        {
            areasY[i] = (areaY * i / 10) - areaY/2; // Y축 앵커가 중앙에 잡혀있으므로, areaY/2만큼 빼줘야 정확하게 측정
            
            // Center 표현을 위한 객체
            Transform effect = transform.GetChild(2).GetChild(1).GetChild(i);
            Vector3 newPos = effect.localPosition;
            newPos.y = areasY[i];
            effect.localPosition = newPos;
            noteDownImage[i] = effect.GetComponent<Image>();
        }
        
        // 큐에 각 노트의 데이터를 넣는다.
        for (int i = 0; i < musicManager.totalNote; i++)
        {
            noteQueue.Enqueue(musicManager.getNoteData(i));
        }

        gameStart = AudioSettings.dspTime;
        // 오디오 소스를 저장
        audioSource.PlayDelayed((float)(songStart + musicManager.offset + musicManager.customOffset));


    }


    void Update()
    {
        currentTime = AudioSettings.dspTime - gameStart; // 현재시간
        Debug.Log(gameStart);

        double timeDiffer = currentTime - (timePerBeat * beatNumber); // 실제 정박과 현재 시간사이의 간격
        beatNumber = (int)(currentTime / timePerBeat);
        // 현재 비트 정박자보다 이후인 경우, 비트 수를 올림
        
        // 메트로눔 실행
        Metronome(beatNumber, currentTime - (timePerBeat * beatNumber));


        if (!audioSource.isPlaying && running)
        {
            running = false;
           // StartCoroutine(ExecuteAfterDelay(2.0f));
        }
    }

    private void Metronome(int beatNumber, double timeDiff)
    {
        // 판정선 위치 변경
        judgeLine.transform.localPosition = new Vector3(checkPositionX(beatNumber, 0, timeDiff), judgeLine.transform.localPosition.y);

        // isEarly는, 지금 현재의 비트넘버가 노트큐의 비트 차례보다 큰지를 의미한다.
        // 동시에 나오거나, 렉등으로 못나온 노트 때문에 반복문을 돌린다.
        bool isEarly = false;

        while (!isEarly)
        {
            // noteQueue에 남아있는 데이터가 있는지 확인
            if (noteQueue.TryPeek(out NoteData noteData))
            {
                // 데이터가 있다면 꺼냄(만약 이전에 안꺼낸 기록이 있으면(렉등으로) 같이 꺼냄)
                if (noteData.beat <= beatNumber)
                {
                    noteQueue.Dequeue();

                    if (noteData.length == 0)
                    {
                        GameObject note = NotePool.instance.normalQueue.Dequeue();
                        Note script = note.GetComponent<Note>();
                        script.SetNoteInfo(checkPositionX(noteData.beat + 3, noteData.posX, timeDiff), areasY[noteData.posY], checkTime(noteData.beat+3, noteData.posX), timePerBeat);
                        note.transform.SetAsFirstSibling();
                        note.SetActive(true);
                    }
                    else if (noteData.length >= 1)
                    {
                        // 롱노트인 경우
                        float startPos = checkPositionX(beatNumber + 3, noteData.posX, timeDiff);
                        float endPos = checkPositionX(beatNumber + 3 + noteData.length, noteData.demi, timeDiff);
                        GameObject endNote = NotePool.instance.endQueue.Dequeue();
                        EndNote script2 = endNote.GetComponent<EndNote>();

                        script2.SetNoteInfo(endPos, areasY[noteData.posY], checkTime(noteData.beat + 3 + noteData.length, noteData.demi), timePerBeat);

                        GameObject longNote = NotePool.instance.longQueue.Dequeue();
                        LongNote script = longNote.GetComponent<LongNote>();
                        // 롱노트의 스크립트로 X축 위치, Y축 위치, 판정 시간, 시간 단위, 왼쪽/오른쪽 여부를 설정
                        script.SetNoteInfo(noteData.posY, startPos, areasY[noteData.posY], checkTime(noteData.beat + 3, noteData.posX), checkTime(noteData.beat + 3 + noteData.length, noteData.demi), timePerBeat, checkLeft(beatNumber + 3), script2);

                        Debug.Log("헬로우");
                        endNote.SetActive(true);
                        longNote.SetActive(true);


                        }
                        //else
                        //{
                        //    // 슬라이스 노트인 경우
                        //}

                    }
                else
                {
                    // 현재 비트가 더 크다면 굳이 꺼낼필요 없으니 반복문 종료
                    isEarly = true;
                }
            }
            else
            {   
                // 더이상 꺼낼게 없으면 반복문 종료
                isEarly = true;
            }
        }
    }

    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultManager.ShowResult();

    }

    // 노트를 만드는 코루틴이다.

    // 메트로눔


    public void noteDown(int i, bool stat)
    {
        noteDownImage[i].enabled = stat;
    }

    private float checkPositionX(int beatNumber, int tempo, double timeDiff)
    {
        // 정확한 비트에서 판정선의 위치를 나타낸다. 
        double beatPosStd = beatNumber % 8;

        // 오른쪽으로 오는 경우를 나타내기 위해 2로나눈 나머지 값을 이용한다.
        if ((beatNumber / 8) % 2 == 0)
            beatPosStd = 8 - beatPosStd;


        // 판정선의 정비트의 위치
        double linePosition = areaX * beatPosStd / 8;
        // 판정선의 세부 박자 단위
        double deciPosition = areaX * tempo / 32;

        // 판정선의 엇박자의 보정 위치  
        double error = (areaX / 8) * (timeDiff / timePerBeat);

        // 판정선의 위치
        Vector3 newPos = judgeLine.transform.localPosition;

        // 판정선 X좌표 위치 측정
        if ((beatNumber / 8) % 2 == 1)
            return (float)(linePosition + deciPosition + error);
        else
            return (float)(linePosition - deciPosition - error);

    }

    private float checkTime(int beatNumber, int tempo)
    {
        return (float)(gameStart + beatNumber * timePerBeat + tempo * timePerBeat / 4);


    }


    private bool checkLeft(int beatNumber)
    {
        return ((beatNumber / 8) % 2 == 1);

    }

    

}
