using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScriptE : MonoBehaviour
{
    public EndingManager EndingManager;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void click()
    {
        audioSource.Play();
        EndingManager.Action();
    }
}
