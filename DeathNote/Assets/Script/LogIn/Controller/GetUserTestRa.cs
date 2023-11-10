using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class GetUserTestRa : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getMember());
    }

    IEnumerator getMember()
    {
        string url = "https://thatsnote.site/members";
        //string url = "http://localhost:8080/members/2";
        string user_id = "2";

        using (UnityWebRequest www = UnityWebRequest.Get(url+user_id))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

}
