using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public Text countdownText;
    public Text scoreText;
    public Animator backgroundAnimator;
    public ColliderMove colliderMoveScript;
    public Rigidbody2D controlledRigidbody;

    public static bool shouldStartCountdown = true; // 처음에 true로 설정

    private void Start()
    {
        if (shouldStartCountdown)
        {
            InitGameStart();
            StartCoroutine(StartCountdown());
        }
        else
        {
            InitGameResume(); // 카운트다운 없이 게임 재개
        }
    }

    private void InitGameStart()
    {
        // 카운트다운 시작에 필요한 초기화
        scoreText.gameObject.SetActive(false);
        backgroundAnimator.enabled = false;
        colliderMoveScript.StopMoving();
        controlledRigidbody.simulated = false;
    }

    private void InitGameResume()
    {
        // 카운트다운 없이 게임 재개에 필요한 초기화
        countdownText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        backgroundAnimator.enabled = true;
        colliderMoveScript.StartMoving();
        controlledRigidbody.simulated = true;
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

    public static void ResetCountdown()
    {
        shouldStartCountdown = false; // 카운트다운을 비활성화
    }
}
