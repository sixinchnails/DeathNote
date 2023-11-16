using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// 게임 초반에 로드하는 플레이어 정보


[Serializable]
public class UserData
{
    public int id;
    public string nickname;
    public int progress;
    public int gold;
    public List<Soul> souls = null;
    public List<Garden> gardens = null;
    public List<RecordData> record = null;
}
[System.Serializable]

public class Garden
{
    public int id;
    public int type;

    public Garden(int type)
    {
        this.type = type;
    }

}

public class UserManager : MonoBehaviour
{
    public UserData userData;
    public static UserManager instance;

    public int load; // 0은 로딩 전, 1은 회원가입 혹은 로그인 필요, 2는 서버 통신 불가, 3은 로그인 완료

    private void Awake()
    {
        // 싱글톤을 위한 초기화 메서드
        if (instance == null)
        {
            instance = this; // 자기 자신을 할당
            DontDestroyOnLoad(gameObject); // 씬 전환시 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스를 파괴
        }
    }

    public void SaveData()
    {
        string nickname = userData.nickname;
        String token = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("UserData", token);

        StartCoroutine(SendToken(nickname, token));
    }

    IEnumerator SendToken(string nickname, string token)
    {
        // URL 설정
        string url = "https://thatsnote.site/members/updatetoken";

        // JSON 데이터 생성
        UserDataDTO data = new()
        {
            nickname = nickname,
            token = token
        };
        string jsonData = JsonUtility.ToJson(data);

        // UnityWebRequest 생성 및 설정
        using (UnityWebRequest www = new UnityWebRequest(url, "PATCH"))
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

        }
    }
}
