using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public NextScriptE next;

    public GameObject dark;
    public GameObject book;


    private void Awake()
    {
        book.SetActive(false);
        dark.SetActive(false);
    }

    private void Start()
    {
        next.BoxAppear();
    }
}
