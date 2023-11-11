using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public NextScriptE next;

    public GameObject book;
    public GameObject past1;
    public GameObject past2;


    private void Awake()
    {
        book.SetActive(false);
        past1.SetActive(false);
        past2.SetActive(false);
    }

    private void Start()
    {
        next.BoxAppear();
    }
}
