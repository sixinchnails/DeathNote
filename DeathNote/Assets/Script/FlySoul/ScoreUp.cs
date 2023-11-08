using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
        //OnTriggerEnter2D : 진입할 때
    {
        Score.score++;
    }
}
