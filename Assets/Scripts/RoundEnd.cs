using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEnd : MonoBehaviour
{
    [SerializeField] private GameObject _reloadBarWindow;
    [SerializeField] private GameObject _reloadText;
    [SerializeField] private GameObject _reloadingCircularCursor;

    public static RoundEnd Instance { get; private set; } //Singleton
    public static int currentLevel;

    private void Awake()
    {
        Instance = this;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void LevelEnd()
    {
        ClearRemainings();

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

    private void ClearRemainings()
    {
        _reloadBarWindow.SetActive(false);
        Weapon.s_isReloading = false;
        Weapon.s_reloadingCircleIsAnimating = false;
        _reloadingCircularCursor.SetActive(false);
        _reloadText.SetActive(false);

        PointsGain[] RemainingPointGainObjects = FindObjectsOfType<PointsGain>();

        for (int i = 0; i < RemainingPointGainObjects.Length; i++)
        {
            Destroy(RemainingPointGainObjects[i].gameObject);
        }
    }
}
