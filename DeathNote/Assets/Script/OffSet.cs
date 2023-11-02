using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffSet : MonoBehaviour
{
    public static double offset = 0;
    public static int num = 0;

    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        offset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = offset.ToString();
    }
}
