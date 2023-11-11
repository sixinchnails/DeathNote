using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountingScore : MonoBehaviour
{
    [SerializeField] Text score;

    float duration = 1f;

    void Start()
    {
        float num = float.Parse(score.text);
        StartCoroutine(Count(num, 0));  
    }

    IEnumerator Count(float target, float current)
    {
        float offset = (target - current) / duration;
        string beforeText;
        while (current < target)
        {
            current += offset * Time.deltaTime;
            beforeText = string.Format("{0:#,###}", current);
            score.text = beforeText;
            yield return null;
        }

        current = target;
        beforeText = string.Format("{0:#,###}", current);
        score.text = beforeText;
    }

}
