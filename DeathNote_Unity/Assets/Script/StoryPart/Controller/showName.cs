using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowName : MonoBehaviour
{
    public Text img;

    void Update()
    {
        StartCoroutine(show());
    }

    IEnumerator show()
    {
        float t = 0.005f;
        while (img.color.a < 1)
        {
            img.color = new Color(255, 255, 255, img.color.a + t);
            yield return new WaitForSeconds(2f);
        }
    }

}
