using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } //Singleton

    private void Awake()
    {
        Instance = this;
    }

    public void LevelEnd()
    {
        if (ScoreManager.score >= ScoreManager.Instance.scoreRequiredToWin)
        {
            ShowNextLevelWindow();
        }
        else
        {
            ShowRetryWindow();
        }
    }

    private void ShowNextLevelWindow()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void ShowRetryWindow()
    {
        this.transform.GetChild(1).gameObject.SetActive(true);
    }
}
