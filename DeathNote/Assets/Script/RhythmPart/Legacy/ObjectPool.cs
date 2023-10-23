using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * 노트의 빈번한 생성과 삭제를 막기 위해 큐로 구성한 ObjectPool입니다.
 * 
 ***/

[System.Serializable] public class ObjectInfo
{
    public GameObject goPrefab; // 이동하는 노트 정보(prefab)
    public int count; // 큐에 들어갈 갯수
    public Transform tfPoolParent;
}

public class ObjectPool : MonoBehaviour
{    
    // ObjectPool에 미리 넣어놓을 노트 리스트
    [SerializeField] ObjectInfo[] leftObjectInfo = null;
    [SerializeField] ObjectInfo[] rightObjectInfo = null;
    [SerializeField] ObjectInfo[] leftMetronomeInfo = null;
    [SerializeField] ObjectInfo[] rightMetronomeInfo = null;

    // 스스로를 호출하는 변수
    public static ObjectPool instance;

    // Object를 담아둘 Queue
    public Queue<GameObject> leftMetronome = new Queue<GameObject>();
    public Queue<GameObject> rightMetronome = new Queue<GameObject>();
    public Queue<GameObject> leftQueue = new Queue<GameObject>();
    public Queue<GameObject> rightQueue = new Queue<GameObject>();

    void Start()
    {
        // 모든 정보를 초기화
        instance = this;
        leftMetronome = InsertQueue(leftMetronomeInfo, true);
        rightMetronome = InsertQueue(rightMetronomeInfo, false);
        leftQueue = InsertQueue(leftObjectInfo, true);
        rightQueue = InsertQueue(rightObjectInfo, false);

    }

    Queue<GameObject> InsertQueue(ObjectInfo[] objectInfos, bool left)
    {
        // Queue 선언
        Queue<GameObject> queue = new Queue<GameObject>();
        for(int x = 0; x < 50; x++)
        {
            foreach (ObjectInfo objectInfo in objectInfos)
            {
                for (int i = 0; i < objectInfo.count; i++)
                {
                    // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
                    GameObject clone = Instantiate(objectInfo.goPrefab, transform.position, Quaternion.identity);
                    clone.GetComponent<Note>().isLeft = left;
                    clone.SetActive(false);
                    if (objectInfo.tfPoolParent != null)
                    {
                        clone.transform.SetParent(objectInfo.tfPoolParent);
                    }
                    else
                        clone.transform.SetParent(transform);

                    queue.Enqueue(clone);
                }
            }
        
        }
        return queue;
    }
}
