using UnityEngine;

public class RoundBegin : MonoBehaviour
{
    [SerializeField] private GameObject[] _roundBeginObjs;
    private int _currentWindow = 0;
    private AudioClip _clickSound;

    private void Awake()
    {
        Time.timeScale = 0;
        _clickSound = Resources.Load<AudioClip>("ScifiClickSound");
    }

    public void NextWindow()
    {
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));
        _roundBeginObjs[_currentWindow++].SetActive(false);
        _roundBeginObjs[_currentWindow].SetActive(true);
    }

    public void StartGame()
    {
        _roundBeginObjs[_currentWindow++].SetActive(false);
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(_clickSound, new Vector3(0, 0, 0));

    }
}
