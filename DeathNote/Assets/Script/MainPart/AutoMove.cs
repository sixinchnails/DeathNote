using UnityEngine;

public class RandomJumpingMovement : MonoBehaviour
{
    public float speed = 5.0f; // 움직임의 속도
    public float jumpForce = 5.0f; // 점프 힘
    public float jumpInterval = 1.5f; // 점프 간격 (초)

    private float direction; // 움직임의 방향 (1: 오른쪽, -1: 왼쪽)
    private Rigidbody2D rb;
    private float nextJumpTime = 0; // 다음 점프 시간

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 시작 시 랜덤한 방향으로 설정
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void Update()
    {
        // 현재 방향으로 움직임
        transform.Translate(direction * speed * Time.deltaTime, 0, 0);

        // 점프
        if (Time.time > nextJumpTime)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            nextJumpTime = Time.time + jumpInterval;
        }
    }
}
