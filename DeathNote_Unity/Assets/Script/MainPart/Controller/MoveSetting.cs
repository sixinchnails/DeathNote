using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveSetting : MonoBehaviour
{
    public static MoveSetting instance;
    public RectTransform uiElement; // 이동시킬 UI 요소
    public Vector3 offScreenPosition; // 화면 밖의 위치
    public Vector3 onScreenPosition; // 화면 안의 위치
    public float moveSpeed = 1.0f; // 이동 속도
    public GameObject regame;
    public GameObject returns;
    Coroutine cor;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환시 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스를 파괴
        }
    }
    // 화면 안으로 이동
    public void MoveIn()
    {
        if(cor!=null)StopCoroutine(cor);
        cor = StartCoroutine(Move(uiElement, onScreenPosition, moveSpeed));
    }

    // 화면 밖으로 이동
    public void MoveOut()
    {
        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(Move(uiElement, offScreenPosition, moveSpeed));
    }

    public void SceneChange()
    {
        Debug.Log("끝내자");
        MusicManager.instance.audioSource.Stop();
        MusicManager.instance.gameStart = false;
        MoveOut();
        SceneManager.LoadScene("StageScene");
    }

    public void SceneRestart()
    {
        Debug.Log("끝내자");
        // 현재 활성 씬의 이름을 얻습니다.
        string sceneName = SceneManager.GetActiveScene().name;
        MoveOut();
        // 현재 활성 씬을 다시 로드합니다.
        SceneManager.LoadScene(sceneName);
    }

    // 이동 코루틴
    IEnumerator Move(RectTransform element, Vector3 targetPosition, float speed)
    {
        if (MusicManager.instance.gameStart)
        {
            regame.SetActive(true);
            returns.SetActive(true);
        }
        else
        {
            regame.SetActive(false);
            returns.SetActive(false);
        }
        while (Vector3.Distance(element.localPosition, targetPosition) > 0.01f)
        {
            element.localPosition = Vector3.MoveTowards(element.localPosition, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

}
