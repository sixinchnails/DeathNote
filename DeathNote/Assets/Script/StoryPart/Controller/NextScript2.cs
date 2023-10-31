using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScript2 : MonoBehaviour
{
    public Opening2Manager manager;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void click2()
    {
        audioSource.Play();
        manager.Action();
    }
}
