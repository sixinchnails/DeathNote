using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * 노트의 빈번한 생성과 삭제를 막기 위해 큐로 구성한 ObjectPool입니다.
 * 
 ***/

[System.Serializable]
public class NoteInfo
{
    public GameObject goPrefab; // 이동하는 노트 정보(prefab)
    public int count; // 큐에 들어갈 갯수
    public Transform tfPoolParent; // 부착할 위치
}

public class NotePool : MonoBehaviour
{
    // ObjectPool에 미리 넣어놓을 노트 리스트
    [SerializeField] NoteInfo[] noteInfo = null;

    // 스스로를 호출하는 변수
    public static NotePool instance;

    // Object를 담아둘 Queue
    public Queue<GameObject> noteQueue = new Queue<GameObject>();

    void Start()
    {
        // 모든 정보를 초기화
        instance = this;
        noteQueue = InsertQueue(noteInfo);

    }

    Queue<GameObject> InsertQueue(NoteInfo[] objectInfos)
    {
        // Queue 선언
        Queue<GameObject> queue = new Queue<GameObject>();

            foreach (NoteInfo objectInfo in objectInfos)
            {
                for (int i = 0; i < objectInfo.count; i++)
                {
                    // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
                    GameObject clone = Instantiate(objectInfo.goPrefab, transform.position, Quaternion.identity);
                    // clone.GetComponent<Note>().isLeft = left;
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

        
        return queue;
    }
}
