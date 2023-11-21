using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPool : MonoBehaviour
{
    [SerializeField] GameObject text = null; // 퍼센트, 스킬

    public Queue<GameObject> textQueue = null;
    // public Queue<GameObject> longQueue = new Queue<GameObject>();

    void Start()
    {
        // 자기 자신을 초기화한 뒤, 큐들을 채워넣음
        textQueue = InsertQueue(text, 50);
    }

    Queue<GameObject> InsertQueue(GameObject target, int idx)
    {
        // Queue 선언
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < idx; i++)
        {
            // ObjectInfo 배열의 모든 요소를 count만큼 생성하고 비활성화 한 뒤 Queue에 넣어둔다.
            GameObject clone = Instantiate(target, transform.position, Quaternion.identity);
            // 비활성화
            clone.SetActive(false);
            // 생성된 요소의 부모요소 설정
            clone.transform.SetParent(transform);
            queue.Enqueue(clone);
        }

        return queue;
    }
}

