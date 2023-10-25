using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScript : MonoBehaviour
{
    public OpeningManager manager;

    public void click()
    {
        manager.Action();
    }
}
