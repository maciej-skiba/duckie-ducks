using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private GameObject _pauseWindow;
    private AudioClip _clickSound;
    private bool _gamePaused = false;

    private void Awake()
    {
        _pauseWindow = this.transform.GetChild(0).gameObject;
        _clickSound = Resources.Load<AudioClip>("ScifiClickSound");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_gamePaused)
        {
            PauseOn();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && _gamePaused)
        {
            PauseOff();
        }
    }

    public void PauseOn()
    {
        Time.timeScale = 0;
        _pauseWindow.SetActive(true);
        _gamePaused = true;
    }

    public void PauseOff()
    {
        Time.timeScale = 1;
        _pauseWindow.SetActive(false);
        _gamePaused = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));
    }

    public void TryAgain()
    {
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
