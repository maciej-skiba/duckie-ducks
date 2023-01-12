using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [HideInInspector] public short timeLeft;
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        timerText = this.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        timeLeft = 15;
        timerText.text = timeLeft.ToString();
        InvokeRepeating("DecrementTimeLeft", 1, 1);
    }

    private void DecrementTimeLeft()
    {
        if(timeLeft > 0)
        {
            timeLeft--;
            timerText.text = timeLeft.ToString();
        }
        else
        {
            LevelManager.LevelEnd();
        }
    }
}
