using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderMove : MonoBehaviour
{
    public float speed;
    private bool isMoving = true; // 움직임을 제어하기 위한 플래그를 추가합니다.

    // 움직임을 시작하거나 정지하기 위한 메서드를 추가합니다.
    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    void Update()
    {
        if (isMoving) // 움직임 플래그를 체크합니다.
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }
}
