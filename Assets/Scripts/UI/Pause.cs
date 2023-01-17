using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private GameObject pauseWindow;
    private bool gamePaused = false;

    private void Awake()
    {
        pauseWindow = this.transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
        {
            PauseOn();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && gamePaused)
        {
            PauseOff();
        }
    }

    public void PauseOn()
    {
        Time.timeScale = 0;
        pauseWindow.SetActive(true);
        gamePaused = true;
    }

    public void PauseOff()
    {
        Time.timeScale = 1;
        pauseWindow.SetActive(false);
        gamePaused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
