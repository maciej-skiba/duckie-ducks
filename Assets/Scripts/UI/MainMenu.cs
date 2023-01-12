using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private short firstLevelIndex = 1;
    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelIndex);
    }
}
