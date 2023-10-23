using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{

    EffectManager[] effectManager;
    ScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        effectManager = FindObjectsOfType<EffectManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Note note = collision.GetComponent<Note>();

        if (collision.CompareTag("Metro"))
        {
            if (collision.GetComponent<Note>().GetIsLeft())
            {
                ObjectPool.instance.leftMetronome.Enqueue(collision.gameObject);
            }
            else
                ObjectPool.instance.rightMetronome.Enqueue(collision.gameObject);

            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Note"))
        {
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                if (note.GetIsLeft()) effectManager[0].JudgeEffect(4);
                else effectManager[1].JudgeEffect(4);
                scoreManager.IncreaseCombo(false);
            }
            if (collision.GetComponent<Note>().GetIsLeft())
            {
                ObjectPool.instance.leftQueue.Enqueue(collision.gameObject);
            }
            else
                ObjectPool.instance.rightQueue.Enqueue(collision.gameObject);

            collision.gameObject.SetActive(false);
        }
    }

}
