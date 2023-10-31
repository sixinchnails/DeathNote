using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToStageFive : MonoBehaviour
{
    public void MoveToStageFiveScene()
    {
        SceneManager.LoadScene("StageFiveScene");
    }
}
