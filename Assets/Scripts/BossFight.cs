using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BossFight : MonoBehaviour
{
    /*  Boss fight stages:
     *  0. Big Quack arrival
     *  1. Big Quack flying
     *  2. Fighting Laser Eyes
     *  3. Big Quack flying 2
     *  4. Catching bombs
     *  5. Finishing Big Quack 
     */

    [SerializeField] private Animator _bigQuackAnimator;
    [SerializeField] private GameObject __bigQuackArrivalDialogue;
    [SerializeField] private AudioSource _bigQuackUfoSound;
    [SerializeField] private Image[] _healthBars;
    [SerializeField] private TextMeshProUGUI[] _healthNames;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _laserEyePrefab;

    private bool _readyToChangeFightStage = false;
    private float _min_X_SpawnPoint;
    private float _max_X_SpawnPoint;
    private float _min_Y_SpawnPoint;
    private float _max_Y_SpawnPoint;
    private float _nextLaserEyeSpawnTime = 0;

    public static byte s_fightStage = 0;
    public static float s_bossHealth = 100;
    public static float s_playerHealth = 100;

    private void Awake()
    {
        // TODO: change it to be generic (e.g depended on screensize)
        _min_X_SpawnPoint = -4.5f;
        _max_X_SpawnPoint = 4.5f;
        _min_Y_SpawnPoint = -1;
        _max_Y_SpawnPoint = 3;
    }

    private void Start()
    {
        Weapon._weaponLocked = true;
        StartCoroutine(BigQuackArrive());
    }

    private void Update()
    {
        switch(s_fightStage)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                if (LaserEyeReadyToSpawn())
                {
                    SpawnLaserEye();
                }
                break;
            case 3:
                break;
            case 4: 
                break;
            case 5:
                break;
            default: break;
        }
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

    private Vector3 GetRandomSpawnpoint()
    {
        return new Vector3(
            Random.Range(_min_X_SpawnPoint, _max_X_SpawnPoint), 
            Random.Range(_min_Y_SpawnPoint, _max_Y_SpawnPoint));
    }

    private void SpawnLaserEye()
    {
        Instantiate(_laserEyePrefab, GetRandomSpawnpoint(), Quaternion.Euler(Vector3.zero));

        _nextLaserEyeSpawnTime  = Time.time + LaserEye.spawnTime + Random.Range(0, 0.5f);
        print("time: " + _nextLaserEyeSpawnTime);
    }

    private bool LaserEyeReadyToSpawn()
    {
        if (_nextLaserEyeSpawnTime + LaserEye.spawnTime < Time.time)
        {
            return true;
        }

        return false;
    }
}
