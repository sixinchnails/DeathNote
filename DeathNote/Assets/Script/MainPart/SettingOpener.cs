using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingOpener : MonoBehaviour
{
    public GameObject modalImage1; // 에디터에서 설정할 모달 이미지1 게임 오브젝트

    public void OnBackgroundClicked()
    {
        modalImage1.SetActive(true); // 모달 이미지1 활성화
    }

    public void CloseModals()
    {
        modalImage1.SetActive(false); // 모달 이미지1 비활성화
    }
}
