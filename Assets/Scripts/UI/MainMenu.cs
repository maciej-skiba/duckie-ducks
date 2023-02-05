using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _musicOnOffText;
    private short _firstLevelIndex = 1;
    private bool _musicOn = true;

    public void StartGame()
    {
        SceneManager.LoadScene(_firstLevelIndex);
    }

    public void SwitchMusicOnOff()
    {
        switch(_musicOn)
        {
            case true:
                _musicOn = false;
                _musicOnOffText.text = "Music: Off";
                break;
            case false:
                _musicOn = true;
                _musicOnOffText.text = "Music: On";
                break;
        }
    }
}
