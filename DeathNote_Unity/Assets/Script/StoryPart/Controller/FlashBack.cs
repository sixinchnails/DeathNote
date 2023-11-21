using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class FlashBack : MonoBehaviour
{


    GameObject past1;
    Image past;
    Color color;
    float t = 0.005f;

    void Start()
    {
        color = new Color(0, 0, 0, 1);
    }

    async public Task show(GameObject obj, Image img)
    {
        past1 = obj;
        past = img;
        past1.SetActive(true);
        await FadeIn();
    }

    private async Task FadeIn()
    {
        print("¹à¾ÆÁ®¶ó");
        while (color.r < 1)
        {
            color.r += t;
            color.g += t;
            color.b += t;
            past.color = new Color(color.r, color.g, color.b, color.a);
            await Task.Delay(5);
        }
    }

    async public Task hide(GameObject obj, Image img)
    {
        await Task.Delay(1000);
        past1 = obj;
        past = img;
        await FadeOut();
        past1.SetActive(false);
    }

    async public Task  FadeOut()
    {
        print("¾îµÖÁ®¶ó");
        while (color.r > 0)
        {
            color.r -= t;
            color.g -= t;
            color.b -= t;
            past.color = new Color(color.r, color.g, color.b, color.a);
            await Task.Delay(5);
        }
    }
}
