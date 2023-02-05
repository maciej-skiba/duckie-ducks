using UnityEngine;
using TMPro;

public class PointsGain : MonoBehaviour
{
    private Animator _animator;
    private TextMeshProUGUI _pointsGainText;

    [HideInInspector] public int pointsGain = 0; 

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _pointsGainText = this.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (pointsGain > 0)
        {
            _pointsGainText.text = "+" + pointsGain.ToString();
            _animator.SetTrigger("GainPoints");
            Invoke(nameof(DestroyThis), 1.5f);
        }
        else if (pointsGain < 0)
        {
            _pointsGainText.text = pointsGain.ToString();
            _animator.SetTrigger("LosePoints");
            Invoke(nameof(DestroyThis), 1.5f);
        }
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }

}
