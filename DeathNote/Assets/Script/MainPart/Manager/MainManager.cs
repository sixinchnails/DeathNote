using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public Animator[] story;
    public GameObject newStory;
    public TextMeshProUGUI nickname;
    
    public void Awake()
    {

        string myNick = UserManager.instance.userData.nickname;
        Debug.Log(myNick);
        int myProgress = UserManager.instance.userData.progress;

        nickname.text = myNick;
        if (myProgress % 100 == 0)
        {
            story[0].SetTrigger("updown");
            newStory.SetActive(true);

        }           

    }

}
