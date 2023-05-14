using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

    private bool _bigQuackIsFlying = false;
    private float _min_X_SpawnPoint;
    private float _max_X_SpawnPoint;
    private float _min_Y_SpawnPoint;
    private float _max_Y_SpawnPoint;
    private float _nextLaserEyeSpawnTime = 0;
    private Vector3 _initialPosition;

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
        _initialPosition = this.transform.position;
    }

    private void Start()
    {

        Weapon.s_weaponLocked = true;
        StartCoroutine(CoBigQuackArrive());
    }

    private void Update()
    {
        print($"fight stage: {s_fightStage}");
        switch(s_fightStage)
        {
            case 0:
                if (!Weapon.s_weaponLocked)
                {
                    s_fightStage = 1;
                }

                break;
            case 1:
                if (!_bigQuackIsFlying)
                {
                    BigQuack.s_BigQuackIsShootable = true;
                    _bigQuackIsFlying = true;
                    StartCoroutine(CoBiqQuackFlying());
                }

                if (BigQuack.s_BigQuack_Health <= 66)
                {
                    s_fightStage = 2;
                }

                break;
            case 2:
                if (BigQuack.s_BigQuackIsShootable)
                {
                    BigQuack.s_BigQuackIsShootable = false;
                    StartCoroutine(CoBigQuackHide());
                }

                if (LaserEyeReadyToSpawn())
                {
                    SpawnLaserEye();
                }

                break;
            case 3:
                BigQuack.s_BigQuackIsShootable = true;

                break;
            case 4: 
                break;
            case 5:
                BigQuack.s_BigQuackIsShootable = true;

                break;
            default: break;
        }
        print(BigQuack.Instance.transform.position);

    }
    IEnumerator CoBigQuackArrive()
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

        Weapon.s_weaponLocked = false;

        while (_healthBars[0].color.a < 1)
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

    IEnumerator CoBiqQuackFlying()
    {
        var floatingInitialPosition = BigQuack.Instance.transform.position;
        _bigQuackAnimator.SetTrigger("FlyingAround");
        BigQuack.Instance.transform.position = floatingInitialPosition;

        while(s_bossHealth > 80)
        { 
            BigQuack.Instance.gameObject.transform.position = GetRandomSpawnpoint();

            print(BigQuack.Instance.transform.position);
            yield return new WaitForSeconds(1f);
        }

        s_fightStage = 2;

        Weapon.s_weaponLocked = true;
        BigQuack.Instance.transform.position = floatingInitialPosition;
    }

    IEnumerator CoBigQuackHide()
    {
        BigQuack.Instance.transform.position = _initialPosition;
        yield return null;
    }
}
