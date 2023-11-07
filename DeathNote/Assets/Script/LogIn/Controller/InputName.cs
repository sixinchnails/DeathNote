using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InputName : MonoBehaviour
{
    public Text nickname;
    private string name = null;

    void Awake()
    {
      
    }

    void Update()
    {
        if (name.Length > 20)
        {

            //InputNickName();
        }
        else
        {
        name = nickname.text;

        }
    }

    public void InputNickName()
    {
        name = nickname.text;
    }
}
