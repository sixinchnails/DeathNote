using UnityEngine;

public class XYMoving : MonoBehaviour
{
    public float floatStrength = 0.5f; // 떠오르는 힘. 이 값을 조절하여 떠오르는 정도를 변경할 수 있습니다.
    public float amplitude = 0.1f; // 움직임의 진폭을 조절하는 변수
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position; // 초기 위치 저장
    }

    void Update()
    {
        // Mathf.Abs를 사용하여 Sin 함수의 값이 항상 양수가 되도록 합니다.
        // 추가로 amplitude를 곱하여 움직임의 진폭을 조절합니다.
        transform.position = originalPosition + new Vector3(Mathf.Abs(Mathf.Sin(Time.time) * amplitude) * floatStrength, 0.0f, 0.0f);
    }
}
