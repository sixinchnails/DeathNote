using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private bool running = false;
    private AudioSource parentAudio;
    // Start is called before the first frame update
    void Start()
    {
        parentAudio = transform.parent.GetComponent<AudioSource>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Sound"))
        {
            parentAudio.Play();
            running = true;
        }
    }

    public bool isRunning()
    {
        return running;
    }
}
