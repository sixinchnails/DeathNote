using UnityEngine;

public class RandomJumpingMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;
    private float direction;
    private float nextActionTime = 0;
    private float actionInterval = 2.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Random.Range(0, 2) == 0 ? -1 : 1;
    }

    private void Update()
    {
        // 랜덤한 시간 간격으로 랜덤한 행동을 선택
        if (Time.time > nextActionTime)
        {
            int action = Random.Range(0, 3); // 0: 방향 변경, 1: 점프, 2: 그대로

            switch (action)
            {
                case 0: // 방향 변경
                    direction *= -1;
                    break;
                case 1: // 점프
                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    break;
                case 2: // 그대로
                    // 아무것도 하지 않음
                    break;
            }

            nextActionTime = Time.time + actionInterval;
        }

        // 현재 방향으로 움직임
        transform.Translate(direction * speed * Time.deltaTime, 0, 0);
    }
}
