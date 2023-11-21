using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToStageThree : MonoBehaviour
{
    public void MoveToStageThreeScene()
    {
        SceneManager.LoadScene("WorldThreeScene");
    }
}
