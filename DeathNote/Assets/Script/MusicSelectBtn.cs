using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelectBtn : MonoBehaviour
{
    //x 리미트를 둔다. 0, -1920, -3840, -5760
    public static int x;
    public static int[] stage = { 0, -1, -2, -3 };
    public static int idx = 0;
    public static int limit = 0;

    public void Start()
    {
        limit = 0;
        idx = 0;
    }
    public void MoveNext()
    {
        if (idx < 3) 
        {
            idx++;
            limit = stage[idx] * 1920;
            Debug.Log(limit);
            Debug.Log(x);
        }
    }

    public void MoveBack()
    {
        if( idx > 0 )
        {
            idx--;
            limit = stage[idx] * 1920;
            Debug.Log(limit);
            Debug.Log(x);
        }
    }
}
