using UnityEngine;

public class RoundBegin : MonoBehaviour
{
    [SerializeField] private GameObject[] _roundBeginObjs;
    private int _currentWindow = 0;

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void NextWindow()
    {
        _roundBeginObjs[_currentWindow++].SetActive(false);
        _roundBeginObjs[_currentWindow].SetActive(true);
    }

    public void StartGame()
    {
        _roundBeginObjs[_currentWindow++].SetActive(false);
        Time.timeScale = 1;
    }
}
