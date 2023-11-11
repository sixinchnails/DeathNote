//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class BoxSelector : MonoBehaviour, IPointerClickHandler
//{
//    public CharacterInfo characterInfo;

//    public Sprite defaultSprite; // 기본 박스 스프라이트
//    public Sprite selectedSprite; // 선택된 박스 스프라이트

//    private Image imageComponent; // 박스의 이미지 컴포넌트
//    private BoxManager boxManager; // 박스 매니저 참조

//    public Image characterDisplayArea; // 왼쪽 페이지의 캐릭터를 표시할 영역
//    public Image innerCharacterImage; // 박스 내부의 캐릭터 이미지 참조
//    public Text nicknameText; // 캐릭터 닉네임을 표시할 텍스트 컴포넌트
//    public Text skillDescriptionText; // 캐릭터 스킬 설명을 표시할 텍스트 컴포넌트

//    private Animator animator; // 애니메이터 컴포넌트 참조

//    private void Awake()
//    {
//        imageComponent = GetComponent<Image>(); // 이 오브젝트의 이미지 컴포넌트 참조 가져오기
//        boxManager = FindObjectOfType<BoxManager>(); // 씬에서 BoxManager 찾기
//        animator = characterDisplayArea.GetComponent<Animator>(); // 캐릭터 표시 영역의 애니메이터 컴포넌트 가져오기
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        Select();
//        boxManager.RegisterSelection(this);

//        // UI 업데이트
//        characterDisplayArea.sprite = characterInfo.characterSprite;
//        nicknameText.text = characterInfo.nickname;
//        skillDescriptionText.text = characterInfo.skillDescription;

//        // 애니메이션 재생
//        animator.Play("BounceAnimation");
//    }

//    public void Deselect()
//    {
//        // 박스의 스프라이트를 기본 스프라이트로 변경
//        imageComponent.sprite = defaultSprite;
//    }

//    private void Select()
//    {
//        // 박스의 스프라이트를 선택된 스프라이트로 변경
//        imageComponent.sprite = selectedSprite;
//    }
//}
