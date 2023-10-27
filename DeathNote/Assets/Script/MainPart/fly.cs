using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스

public class fly : MonoBehaviour
{
    public float floatStrength = 0.5f;
    public float amplitude = 0.1f;
    private Vector2 originalPosition;

    private RectTransform rectTransform; // RectTransform 변수 추가

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform 컴포넌트를 가져옵니다.
        originalPosition = rectTransform.anchoredPosition; // 초기 위치를 anchoredPosition으로 저장합니다.
    }

    void Update()
    {
        rectTransform.anchoredPosition = originalPosition + new Vector2(0.0f, Mathf.Abs(Mathf.Sin(Time.time) * amplitude) * floatStrength);
        // anchoredPosition을 사용하여 위치를 변경합니다.
    }
}
