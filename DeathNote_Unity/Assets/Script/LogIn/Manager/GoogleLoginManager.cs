using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleLoginManager : MonoBehaviour
{
    // public ShowResult showResult;

    public InputField inputField;
    public GameObject alertBox;
    public GameObject dark;
    
    void Awake()
    {
        alertBox.SetActive(false);
        dark.SetActive(false);
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

    public void DuplicateName()
    {
        dark.SetActive(true);
        alertBox.SetActive(true);
        // showResult.duplicateName();
    }

    public void SuccessName()
    {
        dark.SetActive(true);
        alertBox.SetActive(true);
        // showResult.successName();
    }
}
