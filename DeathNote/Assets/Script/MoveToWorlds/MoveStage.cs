using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStage : MonoBehaviour
{
    public void MoveToStage1()
    {
        MusicManager.instance.nowWorld = 0;
        SceneManager.LoadScene("StageScene");
    }
    public void MoveToStage2()
    {
        MusicManager.instance.nowWorld = 1;
        SceneManager.LoadScene("StageScene");
    }
    public void MoveToStage3()
    {
        MusicManager.instance.nowWorld = 2;
        SceneManager.LoadScene("StageScene");
    }
}
