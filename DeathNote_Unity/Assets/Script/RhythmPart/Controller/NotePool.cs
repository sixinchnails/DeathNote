using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * 노트의 빈번한 생성과 삭제를 막기 위해 큐로 구성한 ObjectPool입니다.
 * ClickNote 에서는, 각 위치에 ObjectPool이 존재합니다.
 ***/

public class NotePool : MonoBehaviour
 {
    [SerializeField] GameObject note = null; // 노트
    [SerializeField] GameObject effect = null; // 이펙트

    public Queue<GameObject> clickQueue = null;
    public Queue<GameObject> effectQueue = null;
    // public Queue<GameObject> longQueue = new Queue<GameObject>();

    void Start()
    {
        // 자기 자신을 초기화한 뒤, 큐들을 채워넣음
        clickQueue = InsertQueue(note, 4, false);
        effectQueue = InsertQueue(effect, 4, true);

    }

    Queue<GameObject> InsertQueue(GameObject target, int idx, bool active)
    {
        // Queue 선언
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < idx; i++)
        {
            // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
            GameObject clone = Instantiate(target, transform.position, Quaternion.identity);
            // 비활성화
            clone.SetActive(active);
            // 생성된 요소의 부모요소 설정
            clone.transform.SetParent(transform);
            queue.Enqueue(clone);
        }


        return queue;
    }

    /**
     * 기존
     */
    //// 노트 풀에 미리 넣어놓을 노트 리스트
    //[SerializeField] NoteInfo[] normalNoteInfo = null;
    //[SerializeField] NoteInfo[] longNoteInfo = null;
    //[SerializeField] NoteInfo[] centerNoteInfo = null;
    //[SerializeField] NoteInfo[] endNoteInfo = null;

    //// 스스로를 호출하는 변수
    //public static NotePool instance;

    //// Object를 담아둘 Queue
    //public Queue<GameObject> normalQueue = new Queue<GameObject>();
    //public Queue<GameObject> longQueue = new Queue<GameObject>();
    //public Queue<GameObject> centerQueue = new Queue<GameObject>();
    //public Queue<GameObject> endQueue = new Queue<GameObject>();

    //void Start()
    //{
    //    // 자기 자신을 초기화한 뒤, 큐들을 채워넣음
    //    instance = this;
    //    normalQueue = InsertQueue(normalNoteInfo);
    //    longQueue = InsertQueue(longNoteInfo);
    //    centerQueue = InsertQueue(centerNoteInfo);
    //    endQueue = InsertQueue(endNoteInfo);
    //}

    //Queue<GameObject> InsertQueue(NoteInfo[] objectInfos)
    //{
    //    // Queue 선언
    //    Queue<GameObject> queue = new Queue<GameObject>();

    //        foreach (NoteInfo objectInfo in objectInfos)
    //        {
    //            for (int i = 0; i < objectInfo.count; i++)
    //            {
    //                // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
    //                GameObject clone = Instantiate(objectInfo.goPrefab, transform.position, Quaternion.identity);
    //            // 비활성화
    //            clone.SetActive(false);
    //            // 생성된 요소의 부모요소 설정
    //            if (objectInfo.tfPoolParent != null)
    //            {
    //                clone.transform.SetParent(objectInfo.tfPoolParent);
    //            }
    //            else
    //                clone.transform.SetParent(transform);
    //            queue.Enqueue(clone);
    //            }
    //        }


    //    return queue;
    //}
}
