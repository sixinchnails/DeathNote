using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchStart : MonoBehaviour
{
    public void SceneChange()
    {   
        SceneManager.LoadScene("OpeningStory");
    }
    // Start is called before the first frame update
}
