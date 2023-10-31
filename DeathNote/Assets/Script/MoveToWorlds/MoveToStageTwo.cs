using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToStageTwo : MonoBehaviour
{
    public void MoveToStageTwoScene()
    {
        SceneManager.LoadScene("StageTwoScene");
    }
}
