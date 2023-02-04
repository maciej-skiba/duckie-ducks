using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } //Singleton
    public static int currentLevel;

    private void Awake()
    {
        Instance = this;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void LevelEnd()
    {
        PointsGain[] RemainingPointGainObjects = FindObjectsOfType<PointsGain>();

        for (int i=0; i < RemainingPointGainObjects.Length; i++)
        {
            Destroy(RemainingPointGainObjects[i].gameObject);
        }

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
