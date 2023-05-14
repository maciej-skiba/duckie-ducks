using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText;

    private Timer() { }

    [HideInInspector] static public short timeLeft = 15;

    public static Timer Instance { get; private set; } //Singleton

    private void Awake()
    {
        Instance = this;
        _timerText = this.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        _timerText.text = timeLeft.ToString();
        InvokeRepeating("DecrementTimeLeft", 1, 1);
    }

    private void OnEnable()
    {
        timeLeft = 15;
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
            RoundEnd.Instance.LevelEnd();
        }
    }
}
