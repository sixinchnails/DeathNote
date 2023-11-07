using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelectBtn : MonoBehaviour
{
    //x 리미트를 둔다. 0, -1920, -3840, -5760
    public static int[] stage = { 0, -1, -2, -3 };
    public static int idx = 0;
    public static int limit = 0;
    private int w;


    public void Start()
    {
        limit = 0;
        idx = 0;
        w = Screen.width;
    }
    public void MoveNext()
    {
        if (idx < 3) 
        {
            idx++;
            limit = stage[idx] * w;
            if(Mathf.Abs(limit % 8) == 1)
            {
                limit -= 1;
                Debug.Log(limit);
            }else if (Mathf.Abs(limit % 8) == 2)
            {
                limit -= 2;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 3)
            {
                limit -= 3;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 4)
            {
                limit -= 4;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 5)
            {
                limit += 3;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 6)
            {
                limit += 2;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 7)
            {
                limit += 1;
                Debug.Log(limit);
            }
            else
            {
                Debug.Log(limit);
            }
        }
    }

    public void MoveBack()
    {
        if( idx > 0 )
        {
            idx--;
            limit = stage[idx] * w;
            if (Mathf.Abs(limit % 8) == 1)
            {
                limit -= 1;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 2)
            {
                limit -= 2;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 3)
            {
                limit -= 3;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 4)
            {
                limit -= 4;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 5)
            {
                limit += 3;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 6)
            {
                limit += 2;
                Debug.Log(limit);
            }
            else if (Mathf.Abs(limit % 8) == 7)
            {
                limit += 1;
                Debug.Log(limit);
            }
            Debug.Log(limit);
        }
    }
}
