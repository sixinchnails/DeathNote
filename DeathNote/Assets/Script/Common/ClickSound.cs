using UnityEngine;
using UnityEngine.EventSystems; // EventTrigger 사용을 위한 네임스페이스

public class ClickSound : MonoBehaviour, IPointerDownHandler // IPointerDownHandler 인터페이스를 구현
{
    public AudioSource audioSource; // Inspector에서 할당할 AudioSource 컴포넌트

    // 인터페이스의 OnPointerDown 메서드 구현
    public void OnPointerDown(PointerEventData eventData)
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // 오디오 소스 재생
        }
    }
}
