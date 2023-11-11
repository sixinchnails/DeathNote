using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StageManager : MonoBehaviour
{
    AudioSource audioSource; // 오디오 정보
    MusicManager musicManager; // 노래 정보를 담고 있는 객체
    ScoreManager scoreManager; // 점수 정보를 담고 있는 객체
   
    public int beatNumber = 0; // 진행중인 현재의 비트 번호
    public double timePerBeat; // 비트 당 걸리는 시간
    public Queue<NoteData> noteQueue; // 노트 정보를 담을 큐
    public float speed ;
    public double currentTime; // 현재 게임 시간
    public double gameStart; // 게임 시작 시간
    public double songStart; // 노래 시작 시간

    public Color black = Color.black; // 어두운 색
    public Color white = Color.white; // 어두운 색
    public Color semiparent; // 최종 색상 (반투명색)
    public Color transparent; // 원래 색상 (투명색)
    private bool running = false; // 음악 실행중 여부
    private bool hasPlayed = false; // 음악 실행 여부
    private Animator bgAnimator;

    [SerializeField] ClickNote[] clicknotes;
    [SerializeField] EffectController[] effectControllers;
    [SerializeField] GameObject background;
    [SerializeField] Image thumbnail;
    [SerializeField] GameObject readyUI;
    [SerializeField] TextMeshProUGUI title; // 곡 제목
    [SerializeField] ResultManager resultManager;


    void Awake()
    {
        transparent = new Color(white.r, white.g, white.b, 0); // 투명색상
        semiparent = new Color(white.r, white.g, white.b, 150f / 255f); // 최대색상

    }

    void Start()
    {
        readyUI.SetActive(true);
        noteQueue = new Queue<NoteData>(); // 노트 큐 선언
        bgAnimator = background.GetComponent<Animator>(); // 배경 애니메이션 설정
        
        // MusicManager 싱글턴을 불러오고, 노래 설정
        musicManager = MusicManager.instance;
        musicManager.SetSomeDay();
        //musicManager.SetSecondRun();
        title.text = musicManager.musicTitle;
        scoreManager = ScoreManager.instance;
        audioSource = musicManager.audioSource;
        Debug.Log("길이:"+musicManager.beat.Length);
        // bpm을 60으로 나눈 초당 비트수의 역수는 비트당 초
        timePerBeat = (60d / musicManager.bpm);
        // song은 2마디( musicManger.songBeat의 두배 )에서 시작
        songStart = timePerBeat * (musicManager.songBeat * 2);
        speed = musicManager.bpm / 120;
        // 노트와 그 이펙트를 연결짓습니다.
        for (int i = 0; i < 16; i++)
        {
            clicknotes[i].effectController = effectControllers[i];
            clicknotes[i].speed = 1.0f;

        }


        // 큐에 각 노트의 데이터를 넣는다.
        for (int i = 0; i < musicManager.totalNote; i++)
        {
            noteQueue.Enqueue(musicManager.getNoteData(i));
        }

        StartCoroutine(ReadyFinish());
        StartCoroutine(StartMusic(4.0f));
    }

    IEnumerator ReadyFinish()
    {
        yield return new WaitForSeconds(2.0f); // 2초의 시간을 기다림
        readyUI.SetActive(false); // 다시 안보이게함

        
    }

    IEnumerator StartMusic(float delay)
    {
        yield return new WaitForSeconds(delay);

        bgAnimator.speed = (float)musicManager.bpm / 240; // 애니메이션 설정
        // 노래가 재생중이라고 변경
        running = true;
        // 노래가 재생된 이력이 있다고 변경
        hasPlayed = true;
        // 게임 시작시간을 저장
        gameStart = AudioSettings.dspTime;
        // 오디오 재생
        audioSource.PlayDelayed((float)timePerBeat * 4 + musicManager.offset + musicManager.customOffset + speed);
        StartCoroutine(UpdateNote());
        bgAnimator.SetTrigger("start");
    }

    IEnumerator UpdateNote()
    {

        while (running)
        {
            // 썸네일의 투명도를 고침
            float lerpValue = Mathf.Clamp01(((float)scoreManager.totalPercent / (musicManager.totalNote * 100))); // 보간(Clamp는 0~1로 제한)
            Debug.Log(lerpValue);
            thumbnail.color = Color.Lerp(transparent, semiparent, lerpValue);
            currentTime = AudioSettings.dspTime; // 현재시간
            int now = (int)((currentTime - gameStart) / timePerBeat);

            if(beatNumber != now)
            {
                beatNumber = now;
                // 메트로눔 실행
                Metronome(beatNumber);
            }
            

            if (noteQueue.Count == 0)
            {
                running = false;
                StartCoroutine(ExecuteAfterDelay(4.0f));
            }

            yield return null;
        }
        
    }

    IEnumerator ExecuteAfterDelay(float delay)
    {
        float startVolume = audioSource.volume;
        Color color = thumbnail.color;
        // fadeDuration 동안 점차 볼륨을 줄인다.
        while (audioSource.volume > 0)
        {
            float lerpValue = 1f - audioSource.volume / startVolume; // 볼륨이 줄어드는 비율에 따라 lerpValue를 계산합니다.
            thumbnail.color = Color.Lerp(color, white, lerpValue); // 그 비율에 따라 색상을 변경합니다.

            audioSource.volume -= startVolume * Time.deltaTime / delay;
            yield return null;
        }
        thumbnail.color = white;
        audioSource.Stop();
        audioSource.volume = startVolume; // 원본 볼륨으로 다시 설정 (재생 준비)

        resultManager.ShowResult(musicManager.musicTitle, (float)scoreManager.totalPercent / (musicManager.totalNote * 100), scoreManager.score.text);

    }
    IEnumerator EnableNote(float info, ClickNote note, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (note.gameObject.activeSelf)
        {
            note.SetActive(false);
            // 필요하다면 여기서 잠깐 대기할 수도 있습니다.
            // yield return null; // 다음 프레임까지 대기
        }
        note.SetNoteInfo(info, timePerBeat);
        note.SetActive(true);

    }

    public void SceneChange()
    {

    }

    public void SceneRestart()
    {
        // 현재 활성 씬의 이름을 얻습니다.
        string sceneName = SceneManager.GetActiveScene().name;

        // 현재 활성 씬을 다시 로드합니다.
        SceneManager.LoadScene(sceneName);
    }

    private void Metronome(int beatNumber)
    {
        //String str = "{";
        //for(int i = 1; i <= 300; i++)
        //{
        //    str += i * 10;
        //    str += ", ";
        //}
        //str += "}";
        //Debug.Log(str);
        // isEarly는, 지금 현재의 비트넘버가 노트큐의 비트 차례보다 큰지를 의미한다.
        // 동시에 나오거나, 렉등으로 못나온 노트 때문에 반복문을 돌린다.
        bool isEarly = false;

        while (!isEarly)
        {
            // noteQueue에 남아있는 데이터가 있는지 확인
            if (noteQueue.TryPeek(out NoteData noteData))
            {
                // 데이터가 있다면 꺼냄(만약 이전에 안꺼낸 기록이 있으면(렉등으로) 같이 꺼냄)
                if (noteData.beat/10 <= beatNumber)
                {
                    noteQueue.Dequeue();

                    if (noteData.length == 0)
                    {
                        float nextTime = CheckTime((noteData.beat + 1) / 10, noteData.beat % 10);
                        ClickNote note = clicknotes[noteData.pos];
                        

                        StartCoroutine(EnableNote(nextTime+1.0f, note, (float)(nextTime - currentTime)));
                        
                    }
                    //else if (noteData.length >= 1)
                    //{
                    //    // 롱노트인 경우
                    //    float startPos = checkPositionX(beatNumber + 3, noteData.posX, timeDiff);
                    //    gauges = new List<CenterNote>();

                    //    int start = noteData.beat + 3;

                    //    for (int i = 2; i <= noteData.length; i++)
                    //    {
                    //        GameObject centerNote = NotePool.instance.centerQueue.Dequeue();
                    //        CenterNote scripts = centerNote.GetComponent<CenterNote>();
                    //        scripts.SetNoteInfo(checkPositionX(start + i / 4, i % 4, timeDiff), areasY[noteData.posY], checkTime(start + i / 4, i % 4));
                    //        gauges.Add(scripts);
                    //        centerNote.SetActive(true);
                    //    }

                    //    GameObject longNote = NotePool.instance.longQueue.Dequeue();
                    //    LongNote script = longNote.GetComponent<LongNote>();
                    //    // 롱노트의 스크립트로 X축 위치, Y축 위치, 판정 시간, 시간 단위, 왼쪽/오른쪽 여부를 설정
                    //    script.SetNoteInfo(startPos, areasY[noteData.posY], checkTime(noteData.beat + 3, noteData.posX), checkTime(noteData.beat + 3 + noteData.length / 4, noteData.length % 4), timePerBeat);
                    //    script.gauges = gauges;
                    //    longNote.SetActive(true);


                    //}
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


    private float CheckTime(int beatNumber, int tempo)
    {
        return (float)(gameStart + beatNumber * timePerBeat + tempo * timePerBeat / 4);
    }


    /**
     * 기존 시스템
     */
    //AudioSource audioSource; // 오디오 정보
    //MusicManager musicManager; // 노래 정보를 담고있는 객체

    //public float areaX = 0; // 노트가 나타나는 영역의 가로길이
    //public float[] areasY = new float[11]; // 노트가 나타나는 세로영역의 각 부분 높이

    //public int beatNumber = 0; // 진행중인 현재의 비트 번호
    //public double timePerBeat; // 비트 당 걸리는 시간
    //public Queue<NoteData> noteQueue; // 노트 정보를 담을 큐

    //public double currentTime; // 현재 게임 시간
    //public double gameStart; // 게임 시작 시간
    //public double songStart; // 노래 시작 시간

    //private bool running = false; // 음악 실행중 여부
    //private bool hasPlayed = false; // 음악 실행 여부
    //private List<CenterNote> gauges; // 중앙 노트

    //[SerializeField] GameObject judgeLine; // 판정선
    //[SerializeField] TextMeshProUGUI title; // 곡 제목
    //Image[] noteDownImage;
    //ResultManager resultManager;


    //void Start()
    //{
    //    noteQueue = new Queue<NoteData>();
    //    // 게임 시작 시간


    //    // MusicManager 싱글턴을 불러오고, 노래 설정
    //    musicManager = MusicManager.instance;
    //    musicManager.setSunset();
    //    title.text = musicManager.musicTitle;
    //    resultManager = FindObjectOfType<ResultManager>();
    //    audioSource = musicManager.audioSource;
    //    // bpm을 60으로 나눈 초당 비트수의 역수는 비트당 초
    //    timePerBeat = (60d / musicManager.bpm);
    //    // song은 2마디( musicManger.songBeat의 두배 )에서 시작
    //    songStart = timePerBeat * (musicManager.songBeat * 2);

    //    // 노래가 재생중이라고 변경
    //    running = true;
    //    // 노래가 재생된 이력이 있다고 변경
    //    hasPlayed = true;


    //    // 노트를 누르는 영역의 크기지정
    //    areaX = GetComponent<RectTransform>().rect.width;
    //    float areaY = GetComponent<RectTransform>().rect.height;
    //    noteDownImage = new Image[11];
    //    for (int i = 0; i < 11; i++)
    //    {
    //        areasY[i] = (areaY * i / 10) - areaY/2; // Y축 앵커가 중앙에 잡혀있으므로, areaY/2만큼 빼줘야 정확하게 측정

    //        // Center 표현을 위한 객체
    //        Transform effect = transform.GetChild(2).GetChild(1).GetChild(i);
    //        Vector3 newPos = effect.localPosition;
    //        newPos.y = areasY[i];
    //        effect.localPosition = newPos;
    //        noteDownImage[i] = effect.GetComponent<Image>();
    //    }

    //    // 큐에 각 노트의 데이터를 넣는다.
    //    for (int i = 0; i < musicManager.totalNote; i++)
    //    {
    //        noteQueue.Enqueue(musicManager.getNoteData(i));
    //    }

    //    gameStart = AudioSettings.dspTime;
    //    // 오디오 소스를 저장
    //    audioSource.PlayDelayed((float)(songStart + musicManager.offset + musicManager.customOffset));


    //}


    //void Update()
    //{
    //    currentTime = AudioSettings.dspTime - gameStart; // 현재시간
    //    Debug.Log(gameStart);

    //    double timeDiffer = currentTime - (timePerBeat * beatNumber); // 실제 정박과 현재 시간사이의 간격
    //    beatNumber = (int)(currentTime / timePerBeat);
    //    // 현재 비트 정박자보다 이후인 경우, 비트 수를 올림

    //    // 메트로눔 실행
    //    Metronome(beatNumber, currentTime - (timePerBeat * beatNumber));


    //    if (!audioSource.isPlaying && running)
    //    {
    //        running = false;
    //       // StartCoroutine(ExecuteAfterDelay(2.0f));
    //    }
    //}

    //private void Metronome(int beatNumber, double timeDiff)
    //{
    //    // 판정선 위치 변경
    //    judgeLine.transform.localPosition = new Vector3(checkPositionX(beatNumber, 0, timeDiff), judgeLine.transform.localPosition.y);

    //    // isEarly는, 지금 현재의 비트넘버가 노트큐의 비트 차례보다 큰지를 의미한다.
    //    // 동시에 나오거나, 렉등으로 못나온 노트 때문에 반복문을 돌린다.
    //    bool isEarly = false;

    //    while (!isEarly)
    //    {
    //        // noteQueue에 남아있는 데이터가 있는지 확인
    //        if (noteQueue.TryPeek(out NoteData noteData))
    //        {
    //            // 데이터가 있다면 꺼냄(만약 이전에 안꺼낸 기록이 있으면(렉등으로) 같이 꺼냄)
    //            if (noteData.beat <= beatNumber)
    //            {
    //                noteQueue.Dequeue();

    //                if (noteData.length == 0)
    //                {
    //                    GameObject note = NotePool.instance.normalQueue.Dequeue();
    //                    Note script = note.GetComponent<Note>();
    //                    script.SetNoteInfo(checkPositionX(noteData.beat + 3, noteData.posX, timeDiff), areasY[noteData.posY], checkTime(noteData.beat+3, noteData.posX), timePerBeat);
    //                    note.transform.SetAsFirstSibling();
    //                    note.SetActive(true);
    //                }
    //                else if (noteData.length >= 1)
    //                {
    //                    // 롱노트인 경우
    //                    float startPos = checkPositionX(beatNumber + 3, noteData.posX, timeDiff);
    //                    gauges = new List<CenterNote>();

    //                    int start = noteData.beat + 3;

    //                    for (int i = 2; i <= noteData.length; i++)
    //                    {
    //                        GameObject centerNote = NotePool.instance.centerQueue.Dequeue();
    //                        CenterNote scripts = centerNote.GetComponent<CenterNote>();
    //                        scripts.SetNoteInfo(checkPositionX(start + i / 4, i % 4, timeDiff), areasY[noteData.posY], checkTime(start + i / 4, i % 4));
    //                        gauges.Add(scripts);
    //                        centerNote.SetActive(true);
    //                    }

    //                    GameObject longNote = NotePool.instance.longQueue.Dequeue();
    //                    LongNote script = longNote.GetComponent<LongNote>();
    //                    // 롱노트의 스크립트로 X축 위치, Y축 위치, 판정 시간, 시간 단위, 왼쪽/오른쪽 여부를 설정
    //                    script.SetNoteInfo(startPos, areasY[noteData.posY], checkTime(noteData.beat + 3, noteData.posX), checkTime(noteData.beat + 3 + noteData.length / 4, noteData.length % 4), timePerBeat);
    //                    script.gauges = gauges;
    //                    longNote.SetActive(true);


    //                    }
    //                    //else
    //                    //{
    //                    //    // 슬라이스 노트인 경우
    //                    //}

    //                }
    //            else
    //            {
    //                // 현재 비트가 더 크다면 굳이 꺼낼필요 없으니 반복문 종료
    //                isEarly = true;
    //            }
    //        }
    //        else
    //        {   
    //            // 더이상 꺼낼게 없으면 반복문 종료
    //            isEarly = true;
    //        }
    //    }
    //}

    //IEnumerator ExecuteAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    resultManager.ShowResult();

    //}

    //// 노트를 만드는 코루틴이다.

    //// 메트로눔


    //public void noteDown(int i, bool stat)
    //{
    //    noteDownImage[i].enabled = stat;
    //}

    //private float checkPositionX(int beatNumber, int tempo, double timeDiff)
    //{
    //    // 정확한 비트에서 판정선의 위치를 나타낸다. 
    //    double beatPosStd = beatNumber % 8;

    //    // 오른쪽으로 오는 경우를 나타내기 위해 2로나눈 나머지 값을 이용한다.
    //    if ((beatNumber / 8) % 2 == 0)
    //        beatPosStd = 8 - beatPosStd;


    //    // 판정선의 정비트의 위치
    //    double linePosition = areaX * beatPosStd / 8;
    //    // 판정선의 세부 박자 단위
    //    double deciPosition = areaX * tempo / 32;

    //    // 판정선의 엇박자의 보정 위치  
    //    double error = (areaX / 8) * (timeDiff / timePerBeat);

    //    // 판정선의 위치
    //    Vector3 newPos = judgeLine.transform.localPosition;

    //    // 판정선 X좌표 위치 측정
    //    if ((beatNumber / 8) % 2 == 1)
    //        return (float)(linePosition + deciPosition + error);
    //    else
    //        return (float)(linePosition - deciPosition - error);

    //}

    //private float checkTime(int beatNumber, int tempo)
    //{
    //    return (float)(gameStart + beatNumber * timePerBeat + tempo * timePerBeat / 4);


    //}


    //private bool checkLeft(int beatNumber)
    //{
    //    return ((beatNumber / 8) % 2 == 1);

    //}



}
