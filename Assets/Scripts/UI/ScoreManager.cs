using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
    
    public int scoreRequiredToWin;
    public static int score;

    public static ScoreManager Instance { get; private set; } //Singleton

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        Instance = this;
    }

    private void Start()
    {
        score = 0;
    }

    private void Update()
    {
        _scoreText.text = "Score: " + score.ToString();
    }
}
