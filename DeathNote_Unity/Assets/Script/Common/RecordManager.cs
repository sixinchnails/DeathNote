using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class RecordDataArray
{
    public List<RecordData> records;
}

[Serializable]
public class RecordData
{
    public string nickname; // 닉네임
    public int code; // progress
    public int score; // 점수
    public float grade; // 등급
    public string data; // 토큰
    public string[] soulNames;

    public RecordData(string nickname, int code, int score, float grade, string data)
    {
        this.nickname = nickname;
        this.code = code;
        this.score = score;
        this.grade = grade;
        this.data = data;
    }
}

public class RecordManager : MonoBehaviour
{
    public static RecordManager instance;
    public List<RecordData> globalRecords; // 곡에 대한 전체 점수
    Coroutine getCoroutine;
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환시 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스를 파괴
        }
    }


    public void MakeRecord(int code)
    {
        RecordData newData = new RecordData(UserManager.instance.userData.nickname, code, 0, 0.0f, "");
        UserManager.instance.userData.record.Add(newData);
    }

    public void GetGlobalRanking(int code)
    {
        if(getCoroutine != null) StopCoroutine(getCoroutine);
        globalRecords = null;
        getCoroutine = StartCoroutine(GetRanking(code));
    }

    IEnumerator GetRanking(int code)
    {
        // URL 설정
        string url = "https://thatsnote.site/rankings/"+code;

        // JSON 데이터 생성
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // 요청 전송 및 응답 대기
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.downloadHandler.text);
                globalRecords = null;
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                RecordDataArray recordDataArray = JsonUtility.FromJson<RecordDataArray>(www.downloadHandler.text);
                globalRecords = recordDataArray.records;
            }
        }
    }

    public void SetMyRank(int code, float grade, int score, Soul souls)
    {

        bool findData = false;
        string data = JsonUtility.ToJson(souls);
        foreach (RecordData record in UserManager.instance.userData.record)
        {
            if (code == record.code)
            {
                findData = true;
                bool isChange = false;
                record.grade = Mathf.Max(grade, record.grade);
                if (score > record.score)
                {
                    record.score = score;
                    isChange = true;
                }
                if (!isChange)
                {
                    data = record.data;
                }
                else
                {
                    record.data = data;
                }
                break;
            }
        }
        
        RecordData newRecord = new RecordData(UserManager.instance.userData.nickname, code, score, grade, data);
        if (!findData) UserManager.instance.userData.record.Add(newRecord);
        UserManager.instance.SaveData();
        UserManager.instance.StartCoroutine(PostRecord(newRecord));

    }

    IEnumerator PostRecord(RecordData data)
    {
        // URL 설정
        string url = "https://thatsnote.site/logs";

        // JSON 데이터 생성
        string jsonData = JsonUtility.ToJson(data);

        // UnityWebRequest 생성 및 설정
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            // GET이 아닌 경우엔 upload와 download 핸들러를 수동으로 지정해줘야함.
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // 요청 전송 및 응답 대기
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

          
            }
        }
    }
}
