using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public Text countdownText;
    public Text scoreText;
    public Animator backgroundAnimator; // 배경 애니메이터
    public ColliderMove colliderMoveScript; // ColliderMove 스크립트
    public Rigidbody2D controlledRigidbody; // 제어할 Rigidbody 2D

    private void Start()
    {
        scoreText.gameObject.SetActive(false); // 스코어 텍스트를 숨깁니다.
        backgroundAnimator.enabled = false; // 배경 애니메이션을 비활성화합니다.
        colliderMoveScript.StopMoving(); // 움직임을 정지합니다.
        controlledRigidbody.simulated = false; // 물리 시뮬레이션을 정지합니다.

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.text = "게임 시작!";
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false); // 카운트다운 텍스트를 숨깁니다.
        scoreText.gameObject.SetActive(true); // 스코어 텍스트를 활성화합니다.
        backgroundAnimator.enabled = true; // 배경 애니메이션을 활성화합니다.
        colliderMoveScript.StartMoving(); // 움직임을 재개합니다.
        controlledRigidbody.simulated = true; // 물리 시뮬레이션을 재개합니다.
    }
}
