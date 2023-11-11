using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMove : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left*speed*Time.deltaTime;
        //transform: 이건 바로 들고올 수 있다.
        //Vector3.left : (-1, 0, 0)
        //Time.deltaTime : 지난 프레임이 완료되는데까지 걸린 시간(FPS 보정용으로 사용)
    }
}
