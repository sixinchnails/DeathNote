using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SteamUp : MonoBehaviour
{
    public Image[] smokes; // 연기들의 이미지 배열. Unity 에디터에서 드래그 앤 드롭으로 연결해주세요.

    void Start()
    {
        // 모든 연기를 초기에 숨깁니다.
        foreach (Image smoke in smokes)
        {
            smoke.gameObject.SetActive(false);
        }

        StartCoroutine(ShowSmokes());
    }


    IEnumerator ShowSmokes()
    {
        while (true)
        {
            foreach (Image smoke in smokes)
            {
                smoke.gameObject.SetActive(true); // 연기를 보여줌
                yield return new WaitForSeconds(1f); // 1초 동안 기다림
                smoke.gameObject.SetActive(false); // 연기를 숨김
            }
            //yield return new WaitForSeconds(3f); // 3초 동안 기다림
        }
    }
}
