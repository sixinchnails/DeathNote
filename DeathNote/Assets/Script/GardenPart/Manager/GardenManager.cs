using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
// Garden 배경을 담고 있는 Garden 객체
public class GardenManager : MonoBehaviour
{
    public List<GardenSoul> moveSouls; // 현재 움직이는 소울
    public int location; // 현재의 가든 번호
    public int capacity; // 현재 정령 갯수
    public SpriteRenderer background; // 현재의 가든 배경
    public ParticleSystem particles; // 파티클...
    
    
    [SerializeField] TextMeshProUGUI inspirit;
    [SerializeField] TextMeshProUGUI maxCapacity;
    [SerializeField] GameObject soulPrefab;
    [SerializeField] Transform area; // Garden의 범위
    [SerializeField] GameObject BookUI;
    [SerializeField] GameObject[] BookPage;

    void Awake()
    {
        location = -1;

    }

    void Start()
    {
        InitSouls();
        UpdateInspirit(); // UI 초기화
        ChangeGarden(0);
    }

    public void InitSouls()
    {
        moveSouls = new List<GardenSoul>();

        for (int i = 0; i < 15; i++)
        {
            int random = UnityEngine.Random.Range(-50, 50);
            Vector3 position = new Vector3(transform.position.x + random, transform.position.y + random);

            // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
            GameObject gardenSoul = Instantiate(soulPrefab, position, Quaternion.identity);
            gardenSoul.SetActive(false);
            // 생성된 요소의 부모요소 설정
            gardenSoul.transform.SetParent(area);
            // GardenSoul 스크립트를 설정
            GardenSoul script = gardenSoul.GetComponent<GardenSoul>();
            script.boundaryTransform = area; 

            moveSouls.Add(script);
        }
    }

    public void ChangeGarden(int location)
    {
        // 현재 위치 변경
        if (this.location == location) return;
        this.location = location;
        this.background.sprite = Resources.Load<Sprite>("Image/Garden/Background/" + location);
        List<Soul> souls = UserManager.instance.userData.souls;
        // 지금 표시할 정령
        
        for(int i = 0; i < 15; i++)
        {
            moveSouls[i].gameObject.SetActive(false);
        }

        int idx = 0;

        // souls에서 확인
        foreach (Soul soul in souls)
        {
            if (soul.garden == location)
            {
                // 정령의 위치를 랜덤으로 설정
                int random = UnityEngine.Random.Range(-50, 50);
                Vector3 position = new Vector3(transform.position.x + random, transform.position.y + random);

                // moveSouls의 GameObject
                GameObject gardenSoul = moveSouls[idx++].gameObject;
                GardenSoul script = gardenSoul.GetComponent<GardenSoul>();
                script.soul = soul;
                gardenSoul.SetActive(true);
            }

        }
    }

    public void Cheat()
    {
        UserManager.instance.userData.gold += 100000;
        UpdateInspirit();
    }

    // 현재 본인의 영감을 표시합니다.
    public void UpdateInspirit()
    {
        inspirit.text = UserManager.instance.userData.gold.ToString();
    }

    // 정령을 얻으면 자신의 정령 리스트에 넣음
    public void NewSoul(Soul soul)
    {
        SoulManager.instance.AddSoul(soul);

        int random = UnityEngine.Random.Range(-50, 50);
        Vector3 position = new Vector3(transform.position.x + random, transform.position.y + random);

        // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
        GameObject gardenSoul = Instantiate(soulPrefab, position, Quaternion.identity);
        gardenSoul.SetActive(false);
        // 생성된 요소의 부모요소 설정
        gardenSoul.transform.SetParent(area);
        // GardenSoul 스크립트를 설정
        GardenSoul script = gardenSoul.GetComponent<GardenSoul>();
        script.boundaryTransform = area;
        script.soul = soul;

        gardenSoul.SetActive(true);
        moveSouls.Add(script);
    }
    // Update is called once per frame
    void Update()
    {

    }

    
}
