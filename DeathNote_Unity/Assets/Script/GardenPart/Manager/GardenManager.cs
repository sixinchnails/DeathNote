using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// Garden 배경을 담고 있는 Garden 객체
public class GardenManager : MonoBehaviour
{
    public List<GardenSoul> moveSouls; // 현재 움직이는 소울
    public int location; // 현재의 가든 번호
    public int capacity; // 현재 정령 갯수
    
    public SpriteRenderer background; // 현재의 가든 배경
    public ParticleSystem particles; // 파티클...

    Coroutine coroutine;

    // 모든 UI
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject soulUI;
    [SerializeField] GameObject wikiUI;
    [SerializeField] GameObject gardenPurchaseUI;
    [SerializeField] GameObject soulPurchaseUI;

    private float speed = 80000.0f;
    private bool open;

    [SerializeField] TextMeshProUGUI inspirit;
    [SerializeField] TextMeshProUGUI maxCapacity;
    [SerializeField] GameObject soulPrefab;
    [SerializeField] Transform area; // Garden의 범위
    [SerializeField] GameObject composeButton; // 작곡 버튼(비활성용)

    void Awake()
    {
        location = -1;
    }

    // 초기화 메서드
    void Start()
    {
        InitSoul(); // 정령을 초기화
    }

    // 정령을 초기화 하는 메서드
    public void InitSoul()
    {
       
        moveSouls = new List<GardenSoul>(); // 움직이는 정령들의 스크립트 리스트
        List<Soul> souls = UserManager.instance.userData.souls; // 유저의 모든 정령
        capacity = souls.Count; // 유저의 총 정령 수
        Debug.Log("유저 총 정령 수 :" + capacity);
        for (int i = 0; i < souls.Count; i++)
        {
            ActiveSoul(souls[i]); // 정령 활성화
        }
        UpdateInspirit(); // UI 초기화
        ChangeGarden(0); // 배경 변경
    }

    // 정령 객체를 활성화 시켜서 맵에 돌아다니게 한다.
    public void ActiveSoul(Soul soul) 
    {
        int random = UnityEngine.Random.Range(-50, 50); // 첫 위치 랜덤화
        Vector3 position = new Vector3(transform.position.x + random, transform.position.y + random);

        GameObject gardenSoul = Instantiate(soulPrefab, position, Quaternion.identity); // 돌아다니는 오브젝트 형성
        gardenSoul.SetActive(false);

        // 생성된 요소의 부모요소 설정
        gardenSoul.transform.SetParent(area);

        // GardenSoul 스크립트를 설정
        GardenSoul script = gardenSoul.GetComponent<GardenSoul>();
        script.boundaryTransform = area;
        // Soul 객체와 연결
        script.soul = soul;
        script.soulDetail = soulUI.GetComponent<SoulDetail>();
        soul.gardenSoul = script;
        // 한번에 관리하기 위해, gardenSoul 스크립트를 관리
        moveSouls.Add(script);

        // 활성화
        gardenSoul.SetActive(true);
    }

    // 정령을 얻으면 자신의 정령 리스트에 넣음
    public void NewSoul(Soul soul)
    {
        SoulManager.instance.AddSoul(soul);
        ActiveSoul(soul);
    }


    IEnumerator MoveToPosition(Transform transform, Vector3 position)
    {
        while (Vector3.Distance(transform.localPosition, position) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, position, speed * Time.deltaTime);
            yield return null;
        }

        transform.localPosition = position; // 최종 위치로 정확하게 설정
    }




    public void ChangeGarden(int location)
    {
        // 현재 위치 변경
        if (this.location == location) return;
        this.location = location;
        this.background.sprite = Resources.Load<Sprite>("Image/Garden/Background/" + location);

    }

    public void Cheat()
    {
        UserManager.instance.userData.gold += 100000;
        UpdateInspirit();
    }

    // 현재 본인의 영감을 표시하고, capacity를 수정합니다.
    public void UpdateInspirit()
    {
        capacity = UserManager.instance.userData.souls.Count; // UI 초기화
        maxCapacity.text = UserManager.instance.userData.souls.Count.ToString() + "/16";
        inspirit.text = UserManager.instance.userData.gold.ToString();
    }


    // soul과 wiki, 구매 UI를 모두 닫습니다.
    public void CloseAllUi()
    {
        soulUI.SetActive(false);
        wikiUI.SetActive(false);
        gardenPurchaseUI.SetActive(false);
        soulPurchaseUI.SetActive(false);
    }

    public void GoMain()
    {
        SceneManager.LoadScene("MainScene");
    }
    
}
