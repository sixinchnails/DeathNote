using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MoveSetting : MonoBehaviour
{
    public RectTransform uiElement; // 이동시킬 UI 요소
    public Vector3 offScreenPosition; // 화면 밖의 위치
    public Vector3 onScreenPosition; // 화면 안의 위치
    public float moveSpeed = 1.0f; // 이동 속도

    // 화면 안으로 이동
    public void MoveIn()
    {
        StartCoroutine(Move(uiElement, onScreenPosition, moveSpeed));
    }

    // 화면 밖으로 이동
    public void MoveOut()
    {
        StartCoroutine(Move(uiElement, offScreenPosition, moveSpeed));
    }

    // 이동 코루틴
    IEnumerator Move(RectTransform element, Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(element.localPosition, targetPosition) > 0.01f)
        {
            element.localPosition = Vector3.MoveTowards(element.localPosition, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
