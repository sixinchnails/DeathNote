using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetSouls : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        string url = "https://thatsnote.site/souls";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // 요청을 보내고 응답을 기다립니다.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                // 에러가 발생했을 경우, 에러 메시지를 로그에 출력합니다.
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // 성공적으로 데이터를 받았을 경우, 데이터를 로그에 출력합니다.
                Debug.Log("Received: " + webRequest.downloadHandler.text);

                // 받아온 JSON 데이터를 C# 객체로 변환합니다.
                SoulsList soulsList = JsonUtility.FromJson<SoulsList>("{\"souls\":" + webRequest.downloadHandler.text + "}");

                // 변환된 데이터를 사용하여 UI를 업데이트하거나 다른 처리를 할 수 있습니다.
                // 예를 들어, 변환된 데이터를 출력해볼 수 있습니다.
                foreach (var soul in soulsList.souls)
                {
                    Debug.Log("Soul Name: " + soul.soulName);
                }
            }
        }
    }
}
