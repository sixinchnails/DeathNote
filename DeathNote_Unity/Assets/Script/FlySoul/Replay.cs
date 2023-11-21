using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    public void ReplayGame()
    {
        GameStartController.shouldStartCountdown = false; // 카운트다운 시작하지 않음
        SceneFader.DisableNextFadeOut();
        SceneManager.LoadScene("PlayScene");
    }

    public void MoveToGarden()
    {
        SceneManager.LoadScene("GardenScene");
    }
}
