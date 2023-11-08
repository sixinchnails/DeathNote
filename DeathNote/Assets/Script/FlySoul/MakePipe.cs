using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePipe : MonoBehaviour
{
    public GameObject pipe;
    //GameObject를 하나 정해서 거기에 pipe를 넣어주기 위해 GameObject의 pipe라는 이름으로 만들어준다.
    float timer = 0;
    public float timeDiff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (timer>timeDiff)
        {
        GameObject newpipe = Instantiate(pipe);
            newpipe.transform.position=new Vector3 (0.53f,Random.Range(-5f,-2.65f),0);
            timer = 0;
            Destroy(newpipe, 10.0f);
        }
    }
}
