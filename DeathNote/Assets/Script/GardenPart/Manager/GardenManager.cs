using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Garden 배경을 담고 있는 Garden 객체
public class GardenManager : MonoBehaviour
{
    public List<Soul> souls;
    public List<GardenSoul> moveSouls;
    public Sprite Background;
    public ParticleSystem particles;
    public int location;
  
    [SerializeField] GameObject soulPrefab;
    [SerializeField] Transform area;
    [SerializeField] GameObject BookUI;
    [SerializeField] GameObject[] BookPage;

    void Awake()
    {
        location = 1;

    }


    void Start()
    {

        List<Soul> souls = SoulManager.instance.Souls;
        moveSouls = new List<GardenSoul>();

        foreach (Soul soul in souls)
        {
            if(soul.garden == location)
            {
                
                int random = UnityEngine.Random.Range(-50, 50);
                Vector3 position = new Vector3(transform.position.x + random, transform.position.y + random);
                // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
                GameObject gardenSoul = Instantiate(soulPrefab, position, Quaternion.identity);
                gardenSoul.SetActive(false);
                // 생성된 요소의 부모요소 설정
                gardenSoul.transform.SetParent(area);

                GardenSoul script = gardenSoul.GetComponent<GardenSoul>();
                script.soul = soul;
                script.boundaryTransform = area;
                gardenSoul.SetActive(true);

                moveSouls.Add(script);
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    
}
