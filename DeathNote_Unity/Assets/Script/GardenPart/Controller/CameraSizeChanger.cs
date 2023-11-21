using UnityEngine;

public class CameraSizeChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Camera mainCamera;
    private float originalCameraSize; // 원래 카메라 크기를 저장할 변수

    void Start()
    {
        // 이 스크립트가 첨부된 게임 오브젝트에서 Sprite Renderer 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 메인 카메라를 찾습니다.
        mainCamera = Camera.main;

        // 원래 카메라 크기를 저장합니다.
        originalCameraSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        // Sprite의 이름을 확인합니다.
        string spriteName = spriteRenderer.sprite.name;

        // 이름이 "1"이나 "2"일 경우, 카메라의 Size를 148로 설정합니다.
        if (spriteName == "1" || spriteName == "2")
        {
            mainCamera.orthographicSize = 148;
        }
        else // 그 외의 경우에는 카메라 크기를 원래대로 돌립니다.
        {
            mainCamera.orthographicSize = originalCameraSize;
        }
    }
}
