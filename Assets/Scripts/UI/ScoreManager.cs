using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject _newPointsTextPrefab;
    private TextMeshProUGUI _scoreText;

    private ScoreManager() { }
    
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

    public void AddScore(int PointsGain, Vector3 MousePosition)
    {
        score += PointsGain;
        GameObject pointsGainObject = 
            Instantiate(
                _newPointsTextPrefab,
                MousePosition, 
                new Quaternion(0, 0, 0, 0), 
                this.transform);
        pointsGainObject.GetComponent<PointsGain>().pointsGain = PointsGain; 
    }
}
