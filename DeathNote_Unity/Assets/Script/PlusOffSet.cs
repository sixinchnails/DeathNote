using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusOffSet : MonoBehaviour
{
    public void PlusOffset()
    {
        OffSet.num += 1;
        OffSet.offset = 0.1 * OffSet.num;
    }
}
