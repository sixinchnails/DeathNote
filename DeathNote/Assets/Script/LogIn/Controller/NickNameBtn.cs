using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NickNameBtn : MonoBehaviour
{
    private bool isSuccess;

    [SerializeField] GameObject alert;
    
    public void getResult(bool result)
    {
        isSuccess = result;
    }

    public void click()
    {
        if(isSuccess)
        {
            SceneManager.LoadScene("StartGame");
        }
        else
        {
            alert.SetActive(false);
        }
    }
}
