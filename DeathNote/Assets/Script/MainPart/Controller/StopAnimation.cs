using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public void Stop()
    {
        gameObject.SetActive(false);
    }
}
