using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    AudioSource audioSource; // ����� ����
    MusicManager musicManager; // �뷡 ������ ��� �ִ� ��ü

    public int beatNumber = 0; // �������� ������ ��Ʈ ��ȣ
    public double timePerBeat; // ��Ʈ �� �ɸ��� �ð�
    public Queue<NoteData> noteQueue; // ��Ʈ ������ ���� ť
    public float speed;
    public double currentTime; // ���� ���� �ð�
    public double gameStart; // ���� ���� �ð�
    public double songStart; // �뷡 ���� �ð�
    public int soulSeq = 0; // �ҿ�

    public Color black = Color.black; // ��ο� ��
    public Color white = Color.white; // ��ο� ��
    public Color semiparent; // ���� ���� (��������)
    public Color transparent; // ���� ���� (������)
    private bool running = false; // ���� ������ ����


    [SerializeField] ScoreManager scoreManager;
    [SerializeField] Image thumbnail;
    [SerializeField] GameObject readyUI;
    [SerializeField] TextMeshProUGUI title; // �� ����
    [SerializeField] ResultManager resultManager;
    [SerializeField] NotePool[] notePools;
    [SerializeField] Effect[] effectControllers;

    List<Soul> mySoulList;

    void Awake()
    {
        transparent = new Color(white.r, white.g, white.b, 0); // ��������
        semiparent = new Color(white.r, white.g, white.b, 150f / 255f); // �ִ����
    }

    void Start()
    {
        readyUI.SetActive(true);
        noteQueue = new Queue<NoteData>(); // ��Ʈ ť ����

        // MusicManager �̱����� �ҷ�����, �뷡 ����
        musicManager = MusicManager.instance;
        musicManager.gameStart = true;
        audioSource = musicManager.audioSource;
        audioSource.Stop();
        // bpm�� 60���� ���� �ʴ� ��Ʈ���� ������ ��Ʈ�� ��
        timePerBeat = (60d / musicManager.bpm);
        // song�� 2����( musicManger.songBeat�� �ι� )���� ����
        songStart = timePerBeat * (musicManager.songBeat * 2);
        speed = musicManager.bpm / 120;
        // ��Ʈ�� �� ����Ʈ�� ���������ϴ�.

        // ť�� �� ��Ʈ�� �����͸� �ִ´�.
        for (int i = 0; i < musicManager.totalNote; i++)
        {
            noteQueue.Enqueue(musicManager.getNoteData(i));
        }

        // ���� ������ �ҿ� ����� �����ɴϴ�.
        mySoulList = SkillManager.instance.equip;

        StartCoroutine(ReadyFinish());
        StartCoroutine(StartMusic(2.0f));
    }

    IEnumerator ReadyFinish()
    {
        int idx = 0;
        string[] text = { "<color=#000000>R</color>EADY", "R<color=#000000>E</color>ADY" ,
            "RE<color=#000000>A</color>DY", "REA<color=#000000>D</color>Y", "READ<color=#000000>Y</color>",
            "REA<color=#000000>D</color>Y", "RE<color=#000000>A</color>DY", "R<color=#000000>E</color>ADY", "<color=#000000>R</color>EADY" };
        TextMeshProUGUI textMesh = readyUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        while (idx < 9)
        {
            textMesh.text = text[idx++];
            yield return new WaitForSeconds(0.2f); // 0.2���� �ð��� ��ٸ�
        }

        readyUI.GetComponent<Animator>().SetTrigger("finish");
        yield return new WaitForSeconds(0.3f);
        readyUI.SetActive(false); // �ٽ� �Ⱥ��̰��� 
    }

    IEnumerator StartMusic(float delay)
    {
        yield return new WaitForSeconds(delay);

        // �뷡�� ������̶�� ����
        running = true;

        // ���� ���۽ð��� ����
        gameStart = AudioSettings.dspTime;
        // ����� ���
        delay = Mathf.Max((float)timePerBeat * 4 + musicManager.offset + musicManager.customOffset + 1.0f, 0);
        audioSource.PlayDelayed(delay);

        StartCoroutine(UpdateNote());
    }

    IEnumerator UpdateNote()
    {

        while (running)
        {
            // ������� �������� ��ħ 
            currentTime = AudioSettings.dspTime; // ����ð�
            int now = (int)((currentTime - gameStart) / timePerBeat);


            if (beatNumber != now)
            {
                beatNumber = now;
                // ��Ʈ�δ� ����
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
        // fadeDuration ���� ���� ������ ���δ�.
        while (audioSource.volume > 0)
        {
            float lerpValue = 1f - audioSource.volume / startVolume; // ������ �پ��� ������ ���� lerpValue�� ����մϴ�.

            audioSource.volume -= startVolume * Time.deltaTime / delay;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume; // ���� �������� �ٽ� ���� (��� �غ�)

        float grade = (float)scoreManager.totalPercent / musicManager.totalNote;

        int gold = (int)(scoreManager.totalInspirit + scoreManager.totalPercent/50);
        UserManager.instance.userData.gold += gold;

        if (UserManager.instance.userData.progress == 0)
        {
            UserManager.instance.userData.progress = musicManager.code;
            UserManager.instance.SaveData();
            RecordManager.instance.SetMyRank(MusicManager.instance.code, grade, scoreManager.currentScore, SkillManager.instance.equip[0]);
            SceneManager.LoadScene("NewOpeningStory");
        }


        else if (musicManager.code >= UserManager.instance.userData.progress)
        {
            UserManager.instance.userData.progress = musicManager.code;
            if (musicManager.code == 404) UserManager.instance.userData.progress = 500;
            if (musicManager.code == 504) UserManager.instance.userData.progress = 600;
            RecordManager.instance.SetMyRank(MusicManager.instance.code, grade, scoreManager.currentScore, SkillManager.instance.equip[0]);
            UserManager.instance.SaveData();
            if (musicManager.code == 604)
            {
                SceneManager.LoadScene("EndingStory");
            }
            else
            {
                resultManager.ShowResult(musicManager.musicTitle, grade, scoreManager.score.text, gold);
            }
        }

        else
        {
            resultManager.ShowResult(musicManager.musicTitle, grade, scoreManager.score.text, gold);
            RecordManager.instance.SetMyRank(MusicManager.instance.code, grade, scoreManager.currentScore, SkillManager.instance.equip[0]);
            UserManager.instance.SaveData();
        }


        


    }
    IEnumerator EnableNote(ClickNote note, Soul turnSoul, float delay)
    {
        yield return new WaitForSeconds(delay);

        note.SetActive(true);
        if (turnSoul != null)
        {
            note.animator.SetTrigger("Jump");
            note.animator.SetInteger("body", turnSoul.customizes[0]);
            note.animator.SetInteger("eyes", turnSoul.customizes[1]);
            note.animator.SetInteger("bcol", turnSoul.customizes[2]);
            note.animator.SetInteger("ecol", turnSoul.customizes[3]);
        }
        else
        {
            note.animator.SetTrigger("Jump");
            note.animator.SetInteger("body", 1);
            note.animator.SetInteger("eyes", 1);
            note.animator.SetInteger("bcol", -1);
            note.animator.SetInteger("ecol", 0);
        }

    }

    public void SceneChange()
    {
        audioSource.Stop();
        musicManager.gameStart = false;
        SceneManager.LoadScene("StageScene");
    }

    public void SceneRestart()
    {
        // ���� Ȱ�� ���� �̸��� ����ϴ�.
        string sceneName = SceneManager.GetActiveScene().name;

        // ���� Ȱ�� ���� �ٽ� �ε��մϴ�.
        SceneManager.LoadScene(sceneName);
    }

    // �� ��Ʈ���� �ߵ��ϴ� �޼����, ��Ʈ�� ����Ʈ�� �����մϴ�.
    private void Metronome(int beatNumber)
    {
        bool isEarly = false;
        // �� ������ ���� ť�� �����ִ� �����Ͱ� �� ���� �� �����Ƿ�, �ݺ������� ����
        while (!isEarly)
        {
            // noteQueue�� ����ִ��� Ȯ��
            if (noteQueue.TryPeek(out NoteData noteData))
            {
                // �����Ͱ� ������, ��Ʈ�� ���� Ȥ�� ���� ��Ʈ�� �ش�
                if (noteData.beat / 10 <= beatNumber)
                {
                    // noteQueue�� �ִ� �����͸� ����
                    noteQueue.Dequeue();

                    // ������ ����ϱ� ����, ��Ʈ�� ��Ÿ���� ��Ȯ�� �ð��� ��� 
                    float exactTime = CheckTime((noteData.beat) / 10, noteData.beat % 10);
                    // ��Ʈ Ǯ���� ��Ʈ�� �����͸� ����
                    ClickNote note = notePools[noteData.pos].clickQueue.Dequeue().GetComponent<ClickNote>();
                    Effect effect = notePools[noteData.pos].effectQueue.Peek().GetComponent<Effect>();
                    // ��Ʈ�� �����͸� �Է� (������, ��ƮǮ, �����ð�, �����ð�)
                    note.SetNoteInfo(effect, notePools[noteData.pos], exactTime + 1.0f, timePerBeat);
                    note.transform.SetAsFirstSibling();
                    // �̹��� ����� ������ �ҷ���
                    Debug.Log("�̹�����:" + soulSeq);
                    Soul turnSoul = mySoulList[soulSeq++];
                    // �ٽ� ���ư��� ���ؼ�, soulSeq�� ����(0~5, �� 6����)
                    soulSeq = soulSeq % mySoulList.Count;
                    if (turnSoul != null || turnSoul.emotions.Length != 0)
                    {
                        // �̹��� �ߵ��� ��ų��ȣ�� ����
                        Debug.Log(turnSoul.emotions.Length);
                        Debug.Log(turnSoul.parameters.Length);
                        int skillNumber = SkillManager.instance.GetSkill(turnSoul, note);
                        // effect�� �ִϸ������� ��Ƽ���� ����

                        effect.hitAnimator.SetInteger("Num", skillNumber);
                    }


                    // note�� ��Ȱ��ȭ�� ���·� �����ϹǷ�, ó���� Awake �޼��尡 �������� �ʱ� ������ animator�� ����������� ��
                    if (note.animator == null) note.animator = note.GetComponent<Animator>();

                    // �� ������ �����ɸ��� ������, �ٽ� �ð� ����
                    double time = AudioSettings.dspTime;
                    // ���� ��Ʈ���� ������ ����
                    float nextTime = CheckTime((noteData.beat + 1) / 10, noteData.beat % 10);
                    StartCoroutine(EnableNote(note, turnSoul, (float)(nextTime - time)));


                }
                //else if (noteData.length >= 1)
                //{
                //    // �ճ�Ʈ�� ���
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
                //    // �ճ�Ʈ�� ��ũ��Ʈ�� X�� ��ġ, Y�� ��ġ, ���� �ð�, �ð� ����, ����/������ ���θ� ����
                //    script.SetNoteInfo(startPos, areasY[noteData.posY], checkTime(noteData.beat + 3, noteData.posX), checkTime(noteData.beat + 3 + noteData.length / 4, noteData.length % 4), timePerBeat);
                //    script.gauges = gauges;
                //    longNote.SetActive(true);


                //}
                //else
                //{
                //    // �����̽� ��Ʈ�� ���
                //}
                else
                {
                    // ���� ��Ʈ�� �� ũ�ٸ� ���� �����ʿ� ������ �ݺ��� ����
                    isEarly = true;
                }
            }
            else
            {
                // ���̻� ������ ������ �ݺ��� ����
                isEarly = true;
            }
        }
    }


    private float CheckTime(int beatNumber, int tempo)
    {
        return (float)(gameStart + beatNumber * timePerBeat + tempo * timePerBeat / 4);
    }


    /**
     * ���� �ý���
     */
    //AudioSource audioSource; // ����� ����
    //MusicManager musicManager; // �뷡 ������ ����ִ� ��ü

    //public float areaX = 0; // ��Ʈ�� ��Ÿ���� ������ ���α���
    //public float[] areasY = new float[11]; // ��Ʈ�� ��Ÿ���� ���ο����� �� �κ� ����

    //public int beatNumber = 0; // �������� ������ ��Ʈ ��ȣ
    //public double timePerBeat; // ��Ʈ �� �ɸ��� �ð�
    //public Queue<NoteData> noteQueue; // ��Ʈ ������ ���� ť

    //public double currentTime; // ���� ���� �ð�
    //public double gameStart; // ���� ���� �ð�
    //public double songStart; // �뷡 ���� �ð�

    //private bool running = false; // ���� ������ ����
    //private bool hasPlayed = false; // ���� ���� ����
    //private List<CenterNote> gauges; // �߾� ��Ʈ

    //[SerializeField] GameObject judgeLine; // ������
    //[SerializeField] TextMeshProUGUI title; // �� ����
    //Image[] noteDownImage;
    //ResultManager resultManager;


    //void Start()
    //{
    //    noteQueue = new Queue<NoteData>();
    //    // ���� ���� �ð�


    //    // MusicManager �̱����� �ҷ�����, �뷡 ����
    //    musicManager = MusicManager.instance;
    //    musicManager.setSunset();
    //    title.text = musicManager.musicTitle;
    //    resultManager = FindObjectOfType<ResultManager>();
    //    audioSource = musicManager.audioSource;
    //    // bpm�� 60���� ���� �ʴ� ��Ʈ���� ������ ��Ʈ�� ��
    //    timePerBeat = (60d / musicManager.bpm);
    //    // song�� 2����( musicManger.songBeat�� �ι� )���� ����
    //    songStart = timePerBeat * (musicManager.songBeat * 2);

    //    // �뷡�� ������̶�� ����
    //    running = true;
    //    // �뷡�� ����� �̷��� �ִٰ� ����
    //    hasPlayed = true;


    //    // ��Ʈ�� ������ ������ ũ������
    //    areaX = GetComponent<RectTransform>().rect.width;
    //    float areaY = GetComponent<RectTransform>().rect.height;
    //    noteDownImage = new Image[11];
    //    for (int i = 0; i < 11; i++)
    //    {
    //        areasY[i] = (areaY * i / 10) - areaY/2; // Y�� ��Ŀ�� �߾ӿ� ���������Ƿ�, areaY/2��ŭ ����� ��Ȯ�ϰ� ����

    //        // Center ǥ���� ���� ��ü
    //        Transform effect = transform.GetChild(2).GetChild(1).GetChild(i);
    //        Vector3 newPos = effect.localPosition;
    //        newPos.y = areasY[i];
    //        effect.localPosition = newPos;
    //        noteDownImage[i] = effect.GetComponent<Image>();
    //    }

    //    // ť�� �� ��Ʈ�� �����͸� �ִ´�.
    //    for (int i = 0; i < musicManager.totalNote; i++)
    //    {
    //        noteQueue.Enqueue(musicManager.getNoteData(i));
    //    }

    //    gameStart = AudioSettings.dspTime;
    //    // ����� �ҽ��� ����
    //    audioSource.PlayDelayed((float)(songStart + musicManager.offset + musicManager.customOffset));


    //}


    //void Update()
    //{
    //    currentTime = AudioSettings.dspTime - gameStart; // ����ð�
    //    Debug.Log(gameStart);

    //    double timeDiffer = currentTime - (timePerBeat * beatNumber); // ���� ���ڰ� ���� �ð������� ����
    //    beatNumber = (int)(currentTime / timePerBeat);
    //    // ���� ��Ʈ �����ں��� ������ ���, ��Ʈ ���� �ø�

    //    // ��Ʈ�δ� ����
    //    Metronome(beatNumber, currentTime - (timePerBeat * beatNumber));


    //    if (!audioSource.isPlaying && running)
    //    {
    //        running = false;
    //       // StartCoroutine(ExecuteAfterDelay(2.0f));
    //    }
    //}

    //private void Metronome(int beatNumber, double timeDiff)
    //{
    //    // ������ ��ġ ����
    //    judgeLine.transform.localPosition = new Vector3(checkPositionX(beatNumber, 0, timeDiff), judgeLine.transform.localPosition.y);

    //    // isEarly��, ���� ������ ��Ʈ�ѹ��� ��Ʈť�� ��Ʈ ���ʺ��� ū���� �ǹ��Ѵ�.
    //    // ���ÿ� �����ų�, �������� ������ ��Ʈ ������ �ݺ����� ������.
    //    bool isEarly = false;

    //    while (!isEarly)
    //    {
    //        // noteQueue�� �����ִ� �����Ͱ� �ִ��� Ȯ��
    //        if (noteQueue.TryPeek(out NoteData noteData))
    //        {
    //            // �����Ͱ� �ִٸ� ����(���� ������ �Ȳ��� ����� ������(��������) ���� ����)
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
    //                    // �ճ�Ʈ�� ���
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
    //                    // �ճ�Ʈ�� ��ũ��Ʈ�� X�� ��ġ, Y�� ��ġ, ���� �ð�, �ð� ����, ����/������ ���θ� ����
    //                    script.SetNoteInfo(startPos, areasY[noteData.posY], checkTime(noteData.beat + 3, noteData.posX), checkTime(noteData.beat + 3 + noteData.length / 4, noteData.length % 4), timePerBeat);
    //                    script.gauges = gauges;
    //                    longNote.SetActive(true);


    //                    }
    //                    //else
    //                    //{
    //                    //    // �����̽� ��Ʈ�� ���
    //                    //}

    //                }
    //            else
    //            {
    //                // ���� ��Ʈ�� �� ũ�ٸ� ���� �����ʿ� ������ �ݺ��� ����
    //                isEarly = true;
    //            }
    //        }
    //        else
    //        {   
    //            // ���̻� ������ ������ �ݺ��� ����
    //            isEarly = true;
    //        }
    //    }
    //}

    //IEnumerator ExecuteAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    resultManager.ShowResult();

    //}

    //// ��Ʈ�� ����� �ڷ�ƾ�̴�.

    //// ��Ʈ�δ�


    //public void noteDown(int i, bool stat)
    //{
    //    noteDownImage[i].enabled = stat;
    //}

    //private float checkPositionX(int beatNumber, int tempo, double timeDiff)
    //{
    //    // ��Ȯ�� ��Ʈ���� �������� ��ġ�� ��Ÿ����. 
    //    double beatPosStd = beatNumber % 8;

    //    // ���������� ���� ��츦 ��Ÿ���� ���� 2�γ��� ������ ���� �̿��Ѵ�.
    //    if ((beatNumber / 8) % 2 == 0)
    //        beatPosStd = 8 - beatPosStd;


    //    // �������� ����Ʈ�� ��ġ
    //    double linePosition = areaX * beatPosStd / 8;
    //    // �������� ���� ���� ����
    //    double deciPosition = areaX * tempo / 32;

    //    // �������� �������� ���� ��ġ  
    //    double error = (areaX / 8) * (timeDiff / timePerBeat);

    //    // �������� ��ġ
    //    Vector3 newPos = judgeLine.transform.localPosition;

    //    // ������ X��ǥ ��ġ ����
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