using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class KooksUserManager : MonoBehaviour
{
    void Awake()
    {
        string url = "https://thatsnote.site/members/2"; // 원하는 URL로 바꿉니다.
        StartCoroutine(SendGetRequest(url));
    }
    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public string email;
        public string role;
        public string provider;
        public string nickName;
        public int level;
        public int experienceValue;
        public int progress;
    }

    public UserData userData;

    public UserData GetUserData()
    {
        return userData;
    }

    public void SetUserData(UserData newData)
    {
        userData = newData;
    }

    IEnumerator SendGetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            UserData userData = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
            Debug.Log("Response: " + request.downloadHandler.text);
            if (userData != null)
            {
                SetUserData(userData);
            }
        }
    }
}
