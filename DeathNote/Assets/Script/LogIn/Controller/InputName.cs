using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InputName : MonoBehaviour
{
    public LoginManager loginManager;
    public NickNameBtn nickNameBtn;

    public InputField inputField;
    public Text nicknameTxt;
    private string nickname;

    public void InputNickName()
    {
        nicknameTxt.text = inputField.text;
        nickname = nicknameTxt.text;
    }

    public void SubmitNickName()
    {
        Debug.Log(nickname+" 으로 등록");
        StartCoroutine(UpdateNickName(nickname));
    }

    public void clearNickName()
    {
        inputField.text = null;
        nicknameTxt.text = inputField.text;
        nickname = nicknameTxt.text;
        Debug.Log("다 지운다 : " + nickname);
    }

    private class NickNameReqeust
    {
        public string nickname;
    }

    IEnumerator UpdateNickName(string newnickname)
    {
        NickNameReqeust data = new NickNameReqeust
        {
            nickname = newnickname
        };
        var json = JsonUtility.ToJson(data);
        byte[] jdata = System.Text.Encoding.UTF8.GetBytes(json);

        string url = "https://thatsnote.site/members/";
        //string url = "http://localhost:8080/members/";
        string member_email = "1";

        UnityWebRequest wwt = new UnityWebRequest(url + member_email, "PUT");
        wwt.uploadHandler = new UploadHandlerRaw(jdata);
        wwt.uploadHandler.contentType = "application/json";
        wwt.downloadHandler = new DownloadHandlerBuffer();
        wwt.SetRequestHeader("Content-Type", "application/json");

        yield return wwt.SendWebRequest();

        if (wwt.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wwt.error);
            //if (아이디가 중복이면)
            //{
            loginManager.DuplicateName();
            nickNameBtn.getResult(false);
            //}
        }
        else
        {
            //닉네임 등록이 성공했다면
            Debug.Log(wwt.downloadHandler.text);
            loginManager.SuccessName();
            nickNameBtn.getResult(true);
        }

    }
}
