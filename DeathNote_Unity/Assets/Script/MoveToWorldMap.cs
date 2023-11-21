using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToWorldMap : MonoBehaviour
{
    public void MoveToWorldMapScene()
    {
        SceneManager.LoadScene("WorldMapScene");
    }
}
