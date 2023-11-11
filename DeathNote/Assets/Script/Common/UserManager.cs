using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

// 게임 초반에 로드하는 플레이어 정보

public class UserData
{
    public int id;
    public string email;
    public string name;
    public int progress;
    public int gold;
    public List<Soul> souls = null;

    public UserData(int id, string email, string name, int progress, int gold)
    {
        this.id = id;
        this.email = email;
        this.name = name;
        this.progress = progress;  
        this.gold = gold;

    }
}
[System.Serializable]
public class GardenData
{
    public int id;
    public int type;

    public GardenData(int id, int type)
    {
        this.id = id;
        this.type = type;
    }
}

public class Garden
{
    public int type;
    public string name;
}


public class UserManager : MonoBehaviour
{
    public UserData userData;
    public List<GardenData> gardenData;
    public static UserManager instance;  
    public int Load 
    {
        get; private set;
    }

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

        userData = new UserData(1, "싸피요", "새후니", 600, 130000);
        gardenData = new List<GardenData>
        {
            new GardenData(0, 1),
            new GardenData(1, 2),
            new GardenData(2, 3)
    };

        Load = 0; // 
    }

    void Start()
    {

        // StartCoroutine(GetUserData());
    }

    IEnumerator GetUserData()
    {
        // PlayerPrefs를 확인하여, Id가 있으면 아이디를, 없으면 0을 반환
        int userId = PlayerPrefs.GetInt("Id", 0);
        string token = PlayerPrefs.GetString("Token", "12");

        // 아이디가 없는 경우 로그인 씬으로 이동
        if (userId == 0 || token == null)
        {
            // SceneManager.LoadSceneAsync("WorldFourScene");
        }
        else
        {
            // 유저의 정보를 모두 가져오는 HTTP통신
            string url = "https://thatsnote.site/members/" + userId;

            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                www.SetRequestHeader("Authorization", "Bearer " + token);

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    Load = 1; // 에러가 발생함
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    // userData 객체에 JSON을 parsing
                    List<Soul> souls = JsonUtility.FromJson<List<Soul>>(www.downloadHandler.text);
                    // SoulManager.instance.Souls = souls;

                    Load = 2; // 로드에 성공함
                    SceneManager.LoadSceneAsync("RaMain");
                }
            }
        }

    }

    // 유저의 닉네임 변경
    IEnumerator PatchUserName(string newName)
    {
        // PlayerPrefs를 확인하여, Id가 있으면 아이디를, 없으면 0을 반환
        int userId = PlayerPrefs.GetInt("Id", 0);
        string token = PlayerPrefs.GetString("Token", "12");

        // 아이디가 없는 경우 로그인 씬으로 이동
        if (userId == 0 || token == null)
        {
            // SceneManager.LoadSceneAsync("WorldFourScene");
        }
        else
        {
            // URL 설정
            string url = "https://thatsnote.site/members/" + userId;

            // JSON 데이터 생성
            var nickname = new
            {
                nickname = newName
            };

            string jsonData = JsonUtility.ToJson(nickname);

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
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("JSON upload complete!");
                }
            }
        }

    }

}
