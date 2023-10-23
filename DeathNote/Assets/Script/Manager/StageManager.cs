using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    AudioSource audioSource; // 오디오 정보
    public int bpm = 0; // beats per minute : 1분당 비트 수 
    public int songBeat = 2; // 현재 노래의 비트 단위 
    public int stdBeat = 4; // 기준이 되는 비트의 길이 단위
    public float offset = 0; // 오프셋 : 게임이 시작된 지점
    public float customOffset = 0; // 사람이 바꾼 오프셋

    public float areaX = 0;
    public float[] areasY = new float[8];


    public int beatNumber = 0;

    public int songSpeed = 1; // 노래 재생 속도로, 1은 2초에 노트가 등장
    public double timePerBeat; // 1비트당 시간

    public double currentTime; // 현재 시간
    public double gameStart; // 게임 시작 시간
    public double songStart; // 노래 시작 시간

    private bool running = false; // 음악 실행중 여부
    private bool hasPlayed = false; // 음악 실행 여부
    //                                     /              /                                                              딴, 딴, 딴           /           /           /          /            /           /          /            /           /                                                                                    
    int[] notes = new int[] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 23, 25, 26, 27, 29, 31, 33, 34, 35, 37, 39, 41, 43, 45, 49, 50, 51, 53, 54, 55, 57, 58, 59, 61, 62, 63, 65, 66, 67, 69, 70, 71, 73, 74, 75, 77, 78, 79, 81, 82, 83, 85, 86, 87, 89, 90, 91, 93, 94, 95, 97, 98, 99, 101, 102, 103, 104, 105, 106, 107, 109, 109, 114, 115, 116, 117, 122, 123, 124, 125, 130, 131, 132, 133, 138, 139, 140, 141, 141, 142, 143, 143, 144, 146, 147, 148, 149, 154, 155, 156, 157, 159, 161, 163, 165, 167, 169, 169, 173, 173, 178, 179, 180, 183, 184, 186, 187, 188, 191, 192, 194, 195, 196, 198, 200, 201, 203, 205, 207, 209, 209, 214, 215, 216, 217, 220, 221, 222, 223, 224, 226, 227, 228, 229, 229, 231, 231, 233, 233, 238, 239, 240, 242, 243, 244, 245, 245, 247, 247, 249, 249, 254, 255, 256, 258, 259, 260, 261, 261, 263, 263, 265, 266, 267, 269, 271, 273, 274, 275, 277, 279, 281, 281, 282, 282, 283, 283, 285, 287, 289, 291, 293, 293, 297, 297 };
    int[] notesPosY = new int[] { 3, 4, 5, 6, 2, 3, 4, 5, 5, 5, 5, 5, 2, 2, 2, 2, 2, 5, 5, 5, 5, 5, 2, 1, 4, 7, 4, 4, 4, 3, 3, 3, 4, 4, 4, 3, 3, 3, 4, 4, 4, 3, 3, 3, 4, 4, 4, 3, 3, 3, 2, 3, 4, 4, 5, 6, 6, 5, 4, 4, 3, 2, 2, 1, 0, 0, 1, 2, 3, 3, 3, 4, 0, 7, 2, 6, 3, 7, 1, 5, 2, 6, 0, 4, 1, 5, 4, 3, 4, 2, 5, 3, 2, 5, 2, 2, 5, 3, 6, 1, 4, 2, 5, 4, 1, 3, 2, 4, 5, 2, 4, 4, 3, 4, 4, 3, 4, 6, 2, 2, 1, 2, 2, 1, 2, 4, 3, 3, 4, 2, 2, 3, 4, 2, 1, 4, 5, 6, 2, 4, 7, 1, 3, 0, 4, 1, 2, 3, 4, 5, 2, 1, 3, 4, 1, 2, 4, 1, 3, 6, 2, 4, 1, 5, 2, 4, 6, 2, 1, 5, 4, 7, 3, 2, 1, 0, 5, 4, 3, 2, 1, 5, 2, 6, 3, 7, 0, 1, 3, 5, 2, 4, 6, 7, 4, 1, 4 };
    [SerializeField] GameObject judgeLine = null;


    public Queue<int[]> noteQueue = new Queue<int[]>();

    ResultManager resultManager;


    void Start()
    {

        string s = "{";
        for (int i = 0; i < notes.Length; i++)
        {
            s += 4 + ",";
        }
        s += "}";

        Debug.Log(notesPosY.Length);
        Debug.Log(notes.Length);
        // 요소의 너비와 높이를 구한다.
        areaX = GetComponent<RectTransform>().rect.width;
        float areaY = GetComponent<RectTransform>().rect.height;

        for(int i = 0; i < 8; i++)
        {
            areasY[i] = (areaY * i / 7) - areaY/2;
        
        }
        // 8비트 뒤에 노래가 시작

        for(int i = 0; i < notes.Length; i++)
        {
            noteQueue.Enqueue(new int[] { notes[i], notesPosY[i] });
        }

        songStart = (timePerBeat * 7);
        resultManager = FindObjectOfType<ResultManager>();

        audioSource = GetComponent<AudioSource>();
        gameStart = AudioSettings.dspTime;
        // bpm을 60으로 나눈 초당 비트수의 역수는 비트당 초이며, 이를 비트단위와 셈한다.
        timePerBeat = (60d / bpm) * ((double)songBeat / stdBeat);
        songStart = timePerBeat * 7;
        //string s = "";
        //for(int i = 4; i <= 1000; i++)
        //{
        //    s = s + i + ",";
        //}

        //Debug.Log(s);

    }

    void Update()
    {

        currentTime = AudioSettings.dspTime - gameStart; // 현재시간
        double timeDiffer = currentTime - (timePerBeat * beatNumber); // 실제 정박과 현재 시간사이의 간격

        // 현재 비트의 정박자보다 이후인 경우, 비트 수를 올림
        if (timeDiffer >= 0)
        {
            // 다음 박자까지 비트 순번을 올림
            while (beatNumber * timePerBeat <= currentTime)
            {
                beatNumber++;
            }
        }

        // 메트로눔 실행
        Metronome(beatNumber-1, currentTime - (timePerBeat * (beatNumber-1)));


        if (!audioSource.isPlaying && running)
        {
            running = false;
            StartCoroutine(ExecuteAfterDelay(2.0f));
        }
    }
        
    IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultManager.ShowResult();

    }

    // 노트를 만드는 코루틴이다.

    // 메트로눔

    private void Metronome(int beatNumber, double timeDiff)
    {
        // 노래는 무조건 8비트에 시작하므로 7비트에 시작한다. 
        if (!running && !hasPlayed && (currentTime >= (songStart - offset)))
        {
            // 노래가 재생중이라고 변경
            running = true;
            // 노래가 재생된 이력이 있다고 변경
            hasPlayed = true;

            // 다음 박자(8비트)에 노래를 실행시키되, 현재의 오차만큼을 빼준다.
            audioSource.PlayDelayed((float)(timePerBeat - (currentTime - (songStart-offset))));
        }


        // 비트는 1부터 들어오기 시작한다.
        // 정확한 비트에서 판정선의 위치를 나타낸다. 
        double beatPosStd = beatNumber % 8;
        // 4비트 후의 판정선에서의 위치를 나타낸다.
        double notePosStd = (beatNumber + 3) % 8;

        // 오른쪽으로 오는 경우를 나타내기 위해 2로나눈 나머지 값을 이용한다.
        if ((beatNumber / 8) % 2 == 1)
            beatPosStd = 8 - beatPosStd;
        if (((beatNumber + 3) / 8) % 2 == 1)
            notePosStd = 8 - notePosStd;


        // 판정선의 정비트의 위치
        double linePosition = areaX * beatPosStd/8;
        // 노트의 정비트의 위치
        double notePosition = areaX * notePosStd / 8;

        // 판정선의 엇박자의 보정 위치  
        double error = (areaX / 8) * (timeDiff / timePerBeat);

        // 판정선의 위치
        Vector3 newPos = judgeLine.transform.localPosition;

        // 판정선 X좌표 위치 측정
        if ((beatNumber / 8) % 2 == 0)
            newPos.x = (float)(linePosition + error);
        else
            newPos.x = (float)(linePosition - error);

        // 판정선 위치 변경
        judgeLine.transform.localPosition = newPos;

        // beatNumber를 확인하고, 큐에서 꺼낸다.

        // isEarly는, 지금 현재의 비트넘버가 노트큐의 비트 차례보다 큰지를 의미한다.
        // 동시에 나오거나, 렉등으로 못나온 노트 때문에 반복문을 돌린다.
        bool isEarly = false;

        while (!isEarly)
        {
            if (noteQueue.TryPeek(out int[] noteInfo))
            {

                if (noteInfo[0] <= beatNumber)
                {
                    noteQueue.Dequeue();
                    GameObject note = NotePool.instance.noteQueue.Dequeue();
                    WideNote script = note.GetComponent<WideNote>();
                    script.SetNoteInfo((float)notePosition, areasY[noteInfo[1]], gameStart + (beatNumber + 3) * timePerBeat, timePerBeat);
                    note.SetActive(true);
                }
                else
                {
                    isEarly = true;
                }
            }
            else
            {
                isEarly = true;
            }
        }
    }

}
