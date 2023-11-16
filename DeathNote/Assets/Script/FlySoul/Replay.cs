using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Replay : MonoBehaviour
{
    public void ReplayGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void MoveToGarden()
    {
        SceneManager.LoadScene("GardenScene");
    }
}
