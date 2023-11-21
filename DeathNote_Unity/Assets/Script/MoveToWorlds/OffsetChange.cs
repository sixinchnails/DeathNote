using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffsetChange : MonoBehaviour
{
    [SerializeField] Text offset;

    float curOffset;

    private void Awake()
    {
        curOffset = float.Parse(offset.text);
    }

    public void PlusOffset()
    {
        if(curOffset < 1)
        {
            curOffset += 0.1f;
        }
        offset.text = (Mathf.Floor(curOffset * 10f) / 10f).ToString();
    }

    public void MinusOffset()
    {
        if (curOffset > 0.1f)
        {
            curOffset -= 0.1f;
        }
        offset.text = (Mathf.Floor(curOffset * 10f) / 10f).ToString();
    }
}
