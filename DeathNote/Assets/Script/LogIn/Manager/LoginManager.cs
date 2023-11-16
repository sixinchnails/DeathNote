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

    public string stringer()
    {
        return id + "/" + nickname + "/" + token;
    }
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
    [SerializeField] GameObject clickable;
    public bool isSignable;
    public bool check;

    public int load; // 0은 로딩 전, 1은 회원가입 혹은 로그인 필요, 2는 서버 통신 불가, 3은 로그인 완료

    public Button CloseButton;

    public float LeftMargin = 400;
    public float TopMargin = 50;
    public float RightMargin = 400;
    public float BottomMargin = 50;
    public bool IsRelativeMargin = true;

    [HideInInspector]
    public string URL;

    private WebViewObject webViewObject;


    void Start()
    {
        AlignCloseButton();
        UserData saveData = JsonUtility.FromJson<UserData>(PlayerPrefs.GetString("UserData", null));
        if(saveData != null)
        {
            StartCoroutine(GetUserData(null));
        }
        else
        {
            buttons.SetActive(true);
        }
        
    }

    // 회원가입 메서드
    public void SignUp()
    {
        Show("https://thatsnote.site/oauth2/authorization/kakao");
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

        // 유저데이터가 없고, 전달받은 아이디가 경우 1번 상태로 변경
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
            nickname = saveData.nickname; // 전달된 닉네임이 없으면, 세이브데이터로
                
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

                    

                    UserDataDTO dto = JsonUtility.FromJson<UserDataDTO>(www.downloadHandler.text);

                    if (dto.token.Length == 0)
                    {
                        Debug.Log("여기에요");
                        UserData userData = new UserData();
                        userData.id = dto.id;
                        userData.nickname = dto.nickname;
                        userData.progress = 0;
                        userData.gold = 0;
                        userData.souls = new List<Soul>
                        {
                            new Soul(0, "첫 정령", 0, new int[]{1, 0, 0, 5}, new int[]{1, 1, 0, 0}, new int[]{10, 10, 10, 10, 10, 10 }, 0, 0)
                        };
                        userData.gardens = new List<Garden>
                        {
                            new Garden(0)
                        };
                        userData.record = new List<RecordData>();

                        UserManager.instance.userData = userData;
                        UserManager.instance.SaveData();
                    }
                    else
                    {
                        Debug.Log("저기에여");
                        UserData userData = JsonUtility.FromJson<UserData>(dto.token);
                        UserManager.instance.userData = userData;
                        UserManager.instance.SaveData();
                    }

                    Debug.Log("레코드 데이터 설정 성공" + www.downloadHandler.text);
                    
                    foreach(Soul soul in UserManager.instance.userData.souls)
                    {
                        if(soul.equip != -1)
                        {
                            SkillManager.instance.equip[soul.equip] = soul;
                        }
                    }

                    Debug.Log("스킬 데이터 설정 성공" + www.downloadHandler.text);
                    myName.text = nickname;
                    load = 3;
                    DestroyWebView();
                    logoutButton.SetActive(true);
                    clickable.SetActive(true);
                    buttons.SetActive(false);
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

    void Update()
    {
        //if (Application.platform == RuntimePlatform.Android) {
        if (Input.GetKey(KeyCode.Escape))
        {
            // 뒤 로 가 기, esc 버 튼 
            if (webViewObject)
            {
                if (webViewObject.gameObject.activeInHierarchy)
                {
                    // webViewObject.GoBack();
                }
            }
            Hide();
            return;
        }
        //}
    }

    private void OnDestroy()
    {
        DestroyWebView();
    }

    void DestroyWebView()
    {
        if (webViewObject)
        {
            GameObject.Destroy(webViewObject.gameObject);
            webViewObject = null;
        }
    }

    void AlignCloseButton()
    {
        if (CloseButton == null)
        {
            return;
        }

        float defaultScreenHeight = 1080;
        float top = CloseButton.GetComponent<RectTransform>().rect.height * Screen.height / defaultScreenHeight;

        TopMargin = top;
    }

    public void Show(string url)
    {
        gameObject.SetActive(true);

        URL = url;

        StartWebView();
    }

    public void Hide()
    {
        // 뒤 로 가 기, esc 버 튼 
        URL = string.Empty;

        if (webViewObject != null)
        {
            webViewObject.SetVisibility(false);

            //webViewObject.ClearCache(true);
            //webViewObject.ClearCookies();                
        }

        DestroyWebView();

        gameObject.SetActive(false);
    }

    public void StartWebView()
    {
        string strUrl = URL;  //"https://www.naver.com/";            

        try
        {
            if (webViewObject == null)
            {
                webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

                webViewObject.Init(
                    cb: OnResultWebView,
                    err: (msg) => { Debug.Log($"WebView Error : {msg}"); },
                    httpErr: (msg) => { Debug.Log($"WebView HttpError : {msg}"); },
                    started: (msg) => { Debug.Log($"WebView Started : {msg}"); },
                    hooked: (msg) => { Debug.Log($"WebView Hooked : {msg}"); },

                    ld: (msg) =>
                    {
                        Debug.Log($"WebView Loaded : {msg}");
                        //webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");                    
                    }
                    , androidForceDarkMode: 1  // 0: follow system setting, 1: force dark off, 2: force dark on

#if UNITY_EDITOR
                    , separated: true
#endif

                );
            }

            webViewObject.LoadURL(strUrl);
            webViewObject.SetVisibility(true);
            webViewObject.SetMargins((int)LeftMargin, (int)TopMargin, (int)RightMargin, (int)BottomMargin, IsRelativeMargin);
        }
        catch (System.Exception e)
        {
            print($"WebView Error : {e}");
        }

    }

    void OnResultWebView(string resultData)
    {
        StartCoroutine(GetUserData(resultData));
        /*
         {
            result : string,
            data : string or json string
        }
        */

        //try
        //{
        //    JsonData json = JsonMapper.ToObject(resultData);

        //    if ((string)json["result"] == "success")
        //    {
        //        JsonData data = json["data"]["response"];
        //        long birthdayTick = (long)(data["birth"].IsLong ? (long)data["birth"] : (int)data["birth"]);
        //        string birthday = (string)data["birthday"];
        //        string unique_key = (string)data["unique_key"];

        //        // success
        //    }
        //    else if ((string)json["result"] == "failed")
        //    {
        //        Hide();

        //        // failed
        //    }
        //}
        //catch (Exception e)
        //{
        //    print("웹 리턴 값에 문제가 있습니다.");
        //}
    }
}

