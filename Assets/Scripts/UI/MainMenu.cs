using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _musicOnOffText;
    private short _firstLevelIndex = 1;
    private bool _musicOn = true;
    private AudioClip _clickSound;

    private void Awake()
    {
        _clickSound = Resources.Load<AudioClip>("ScifiClickSound");
    }

    public void StartGame()
    {
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));

        SceneManager.LoadScene(_firstLevelIndex);
    }

    public void SwitchMusicOnOff()
    {
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));

        switch (_musicOn)
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
