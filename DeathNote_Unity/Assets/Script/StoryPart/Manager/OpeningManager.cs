using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class OpeningManager : MonoBehaviour
{
    public GameObject rBook;

    void Awake()
    {
        rBook.SetActive(false);
    }
}
