using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingOpener : MonoBehaviour
{
    public GameObject modalImage; // 에디터에서 설정할 모달 이미지 게임 오브젝트

    public void OnBackgroundClicked()
    {
        modalImage.SetActive(true); // 모달 이미지 활성화
    }

    public void CloseModal()
    {
        modalImage.SetActive(false); // 모달 이미지 비활성화
    }
}
