using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScript : MonoBehaviour
{
    public OpeningManager manager;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void click()
    {
        audioSource.Play();
        manager.Action();
    }
}
