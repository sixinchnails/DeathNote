using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public static int speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        speed = 8;
        Debug.Log(transform.position.x);
        MusicSelectBtn.limit = 0;
    }

    private void Update()
    {
        if(MusicSelectBtn.limit < transform.position.x - 960) {
            transform.position = transform.position + new Vector3(-1, 0, 0) * speed;
        }
        else if(MusicSelectBtn.limit > transform.position.x - 960)
        {
            transform.position = transform.position + new Vector3(1, 0, 0) * speed;
        }
    }
}
