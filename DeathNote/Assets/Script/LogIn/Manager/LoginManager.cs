using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 초반에 로드하는 플레이어 정보


// 토큰정보(JSON)를 받기위해서 선언하는 DTO 형식
[Serializable]
public class UserDataDTO
{
    public int id;
    public string nickname;
    public int progress;
    public int gold;
    public string token;
    public List<Soul> souls = null;
    public List<Garden> gardens = null;
}

public class LoginManager : MonoBehaviour
{
    [SerializeField] GameObject signButton;
    [SerializeField] GameObject logoutButton;
    [SerializeField] Image policyButton;
    [SerializeField] TextMeshProUGUI myName;
    [SerializeField] GameObject buttons;
    [SerializeField] TMP_InputField signInputField;
    [SerializeField] TMP_InputField logInputField;
    [SerializeField] Sprite unCheckSprite;
    [SerializeField] Sprite checkSprite;
    public bool isSignable;
    public bool check;

    public int load; // 0은 로딩 전, 1은 회원가입 혹은 로그인 필요, 2는 서버 통신 불가, 3은 로그인 완료

    void Start()
    {
        StartCoroutine(GetUserData(null));
    }

    // 인풋 필드가 변경될 때 마다 호출하여 isSignable을 초기화
    public void ValueChangedCheck()
    {
        signButton.SetActive(false);
    }
     
    // 약관에 동의하는 메서드
    public void CheckPolicy()
    {
        check = !check;
        if (check) policyButton.sprite = checkSprite;
        else policyButton.sprite = unCheckSprite;
    }

    // 중복을 체크하는 메서드
    public void CheckDuplicate()
    {
        string nickname = signInputField.text;
        if (nickname.Length > 5) return;
        StartCoroutine(CheckDuple(nickname));
    }

    // 회원가입 메서드
    public void SignUp()
    {
        string nickname = signInputField.text;
        if (nickname.Length > 5) return;
        StartCoroutine(SignUp(nickname));
    }

    public void LogIn()
    {
        string nickname = logInputField.text;
        StartCoroutine(GetUserData(nickname));
    }
    // 로그아웃
    public void LogOut()
    {
        PlayerPrefs.DeleteAll();
        string sceneName = SceneManager.GetActiveScene().name;

        // 현재 활성 씬을 다시 로드합니다.
        SceneManager.LoadScene(sceneName);

    }

    public void MoveMain()
    {
        if (load != 3) return;
        if(UserManager.instance.userData.progress > 0)
        {
             LoadingController.LoadScene("MainScene");
        }
        else
        {
            // SceneManager.LoadScene("OpeningScene 1");
            LoadingController.LoadScene("MainScene");
        }
    }

    // 유저 데이터를 가져오는 코루틴
    IEnumerator GetUserData(string nickname)
    {
        // PlayerPrefs를 확인하여 userData 형식으로 반환
        UserData saveData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString("UserData", null));

        // 유저데이터가 없거나, 전달받은 아이디가 경우 1번 상태로 변경
        if (saveData == null && nickname == null)
        {
            load = 1;
            buttons.SetActive(true);
            yield return null;
            // SceneManager.LoadSceneAsync("WorldFourScene");
        }
        else
        {   
            if(nickname == null)
            nickname = saveData.nickname;
            // string token = PlayerPrefs.GetString("Token", null);

            // URL 설정
            string url = "https://thatsnote.site/members/login";

            // JSON 데이터 생성
            UserData data = new UserData
            {
                nickname = nickname
            };
            string jsonData = JsonUtility.ToJson(data);
            Debug.Log(jsonData);
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
                    load = 2;
                    //PlayerPrefs.DeleteAll();
                    UserManager.instance.userData = saveData;
                }
                else
                {
                    logoutButton.SetActive(true);
                    UserDataDTO dto = JsonUtility.FromJson<UserDataDTO>(www.downloadHandler.text);
                    UserManager.instance.userData = JsonUtility.FromJson<UserData>(dto.token);
                    UserManager.instance.SaveData();

                    foreach(Soul soul in UserManager.instance.userData.souls)
                    {
                        if(soul.equip != -1)
                        {
                            Debug.Log("equip의 길이:" + SkillManager.instance.equip.Count);
                            SkillManager.instance.equip[soul.equip] = soul;
                        }
                    }
                    myName.text = nickname;
                    load = 3;
                }
            }
        }

    }
    
    // 중복 확인 코루틴
    IEnumerator CheckDuple(string nickname)
    {
        // URL 설정
        string url = "https://thatsnote.site/members/login";

        // JSON 데이터 생성
        UserData data = new UserData
        {
            nickname = nickname
        };

        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);
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
                // 만약 로그인에 실패한다는 것은, 이 닉네임이 등록이 안되었다는 것 
                signButton.SetActive(true);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    // 회원가입 코루틴
    IEnumerator SignUp(string nickname)
    {
        // URL 설정
        string url = "https://thatsnote.site/members/signup";

        // JSON 데이터 생성
        UserData data = new()
        {
            nickname = nickname
        };
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log(jsonData);
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
                // 유저데이터를 입력합니다.
                UserData userData = JsonUtility.FromJson<UserData>(www.downloadHandler.text);
                userData.gardens = new List<Garden>
                {
                    new Garden(0)
                };
                userData.souls = new List<Soul>
                {
                    new Soul("이름", 0, new int[]{1, 0, 0, 5}, new int[]{1, 1, 0, 0}, new int[]{10, 10, 10, 10, 10, 10 }, 0, 0)
                };
                userData.record = new List<RecordData>();
                string tokenData = JsonUtility.ToJson(userData);
                UserManager.instance.userData = userData;
                UserManager.instance.SaveData();
                StartCoroutine(SendToken(nickname, tokenData));
            }
        }
    }

    //JSON 정보를 보내는 코루틴
    IEnumerator SendToken(string nickname, string token)
    {
        Debug.Log("nickname:" + nickname);
        Debug.Log("token:" + token);
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
            else
            {
                Debug.Log("오예오예오오옹예");
                string sceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    // 유저의 닉네임 변경
    IEnumerator PatchUserName(string newName)
    {
        // PlayerPrefs를 확인하여, Id가 있으면 아이디를, 없으면 0을 반환
        int userId = PlayerPrefs.GetInt("Id", 0);
        string token = PlayerPrefs.GetString("Token", "12");

        // 아이디가 없는 경우 변수를 0으로 변경
        if (userId == 0 || token == null)
        {
            load = 1;
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
                    load = 2; // 서버 통신 불가로 변경

                    Debug.Log(www.error);
                }
                else
                {
                    load = 3; // 로그인 완료로 변경 
                    Debug.Log(www.downloadHandler.text);

                }
            }
        }

    }


}
