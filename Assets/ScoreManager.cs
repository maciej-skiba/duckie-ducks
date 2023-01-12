using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI ScoreText;
    static public int score;

    private void Awake()
    {
        ScoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        score = 0;
        ScoreText.text = score.ToString();
    }

    private void Update()
    {
        ScoreText.text = score.ToString();
    }
}
