using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BossFight : MonoBehaviour
{
    [SerializeField] private Animator _bigQuackAnimator;
    [SerializeField] private GameObject __bigQuackArrivalDialogue;
    [SerializeField] private AudioSource _bigQuackUfoSound;
    [SerializeField] private Image[] _healthBars;
    [SerializeField] private TextMeshProUGUI[] _healthNames;

    public static byte s_fightStage = 0;
    public static float s_bossHealth = 100;
    public static float s_playerHealth = 100;

    private void Start()
    {
        Weapon._weaponLocked = true;
        StartCoroutine(BigQuackArrive());
        
    }
    IEnumerator BigQuackArrive()
    {
        yield return new WaitUntil(() => Time.timeScale == 1);
        _bigQuackAnimator.SetTrigger("QuackArrive");
        
        while(_bigQuackUfoSound.volume < 1)
        {
            _bigQuackUfoSound.volume += 0.1f;

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(4);

        Time.timeScale = 0;
        __bigQuackArrivalDialogue.SetActive(true);

        yield return new WaitUntil(() => Time.timeScale == 1);

        while(_healthBars[0].color.a < 1)
        {
            for (int i = 0; i < _healthBars.Length; i++)
            {
                var healthBarColor = _healthBars[i].color;
                healthBarColor.a += 0.04f;
                _healthBars[i].color = healthBarColor;

                var healthNameColor = _healthNames[i].color;
                healthNameColor.a += 0.04f;
                _healthNames[i].color = healthNameColor;
            }

            yield return new WaitForSeconds(0.04f);
        }
         
        Weapon._weaponLocked = false;
    }
}
