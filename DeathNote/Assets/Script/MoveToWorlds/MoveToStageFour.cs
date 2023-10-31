using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToStageFour : MonoBehaviour
{
    public void MoveToStageFourScene()
    {
        SceneManager.LoadScene("StageFourScene");
    }
}
