using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Opening2Manager : MonoBehaviour
{
    public TalkManager talkManager;
    public NextScript2 next;

    public GameObject dark;

    private void Awake()
    {
        dark.SetActive(false);
    }

    void Start()
    { 
        next.BoxAppear();
    }
}
