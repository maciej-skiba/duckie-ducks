using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText;

    [HideInInspector] public short timeLeft;

    public static Timer Instance { get; private set; } //Singleton

    private void Awake()
    {
        Instance = this;
        _timerText = this.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        timeLeft = 15;
        _timerText.text = timeLeft.ToString();
        InvokeRepeating("DecrementTimeLeft", 1, 1);
    }

    private void DecrementTimeLeft()
    {
        if(timeLeft > 0)
        {
            timeLeft--;
            _timerText.text = timeLeft.ToString();
        }
        else
        {
            Time.timeScale = 0;
            LevelManager.Instance.LevelEnd();
        }
    }
}
