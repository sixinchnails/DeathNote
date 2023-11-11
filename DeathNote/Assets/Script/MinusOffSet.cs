using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinusOffSet : MonoBehaviour
{
    public void MinusOffset()
    {
        OffSet.num -= 1;
        OffSet.offset = 0.1 * OffSet.num;
    }
}
