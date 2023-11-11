using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // UI 요소에 대한 네임스페이스 추가

public class BoxSelector : MonoBehaviour, IPointerClickHandler
{
    public CharacterInfo characterInfo; // CharacterInfo 클래스가 존재하고 올바르게 정의되어 있는지 확인

    public Sprite defaultSprite; 
    public Sprite selectedSprite; 

    private Image imageComponent; 
    private BoxManager boxManager; 

    public Image characterDisplayArea; 
    public Image innerCharacterImage;
    public Text nicknameText; 
    public Text skillDescriptionText; 

    private Animator animator;

    private void Awake()
    {
        imageComponent = GetComponent<Image>(); 
        boxManager = FindObjectOfType<BoxManager>(); 
        animator = characterDisplayArea.GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        Select(); // 메서드 이름 대문자 S
        boxManager.RegisterSelection(this); 

        // UI 업데이트
        nicknameText.text = characterInfo.nickname;
        skillDescriptionText.text = characterInfo.skillDescription;

        // 애니메이션 재생
        animator.Play("BounceAnimation");
    }

    public void Deselect()
    {
        // 박스의 스프라이트를 기본 스프라이트로 변경
        imageComponent.sprite = defaultSprite;
    }

    private void Select()
    {
        // 박스의 스프라이트를 선택된 스프라이트로 변경
        imageComponent.sprite = selectedSprite;
    }
}
