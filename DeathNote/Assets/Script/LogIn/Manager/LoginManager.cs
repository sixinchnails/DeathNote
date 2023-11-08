using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public GameObject alertBox;
    public InputField inputField;
    void Awake()
    {
        alertBox.SetActive(false);
    }

    private void Start()
    {
        inputField.ActivateInputField();  
    }

    IEnumerator login()
    {
        string url = "https://thatsnote.site/oauth2/authorization/google?redirect_uri=http://thatsnote.site/oauth/redirect";
        yield return null;
    }
}
