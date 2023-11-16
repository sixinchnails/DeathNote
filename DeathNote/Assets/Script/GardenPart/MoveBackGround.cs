using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float speed = 5.0f;   // 배경의 이동 속도
    public Vector3 moveDirection = Vector3.left;  // 기본적으로 왼쪽으로 이동

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
