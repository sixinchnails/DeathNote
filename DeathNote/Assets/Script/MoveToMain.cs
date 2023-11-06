using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToMain : MonoBehaviour
{
    public void MoveToMainScene()
    {
        SceneManager.LoadScene("RaMain");
    }
}
