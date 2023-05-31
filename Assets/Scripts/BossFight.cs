using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

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
    [SerializeField] private GameObject _bigQuackArrivalDialogue;
    [SerializeField] private GameObject _afterStage1Dialogue;
    [SerializeField] private GameObject _afterStage2Dialogue;
    [SerializeField] private AudioSource _bigQuackUfoSound;
    [SerializeField] private Image[] _healthBars;
    [SerializeField] private TextMeshProUGUI[] _healthNames;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _laserEyePrefab;
    [SerializeField] private TextMeshProUGUI _playerHealthAmount;
    [SerializeField] private TextMeshProUGUI _bossHealthAmount;

    private byte _laserEyeKilledGoal = 1;
    private bool _bigQuackReadyToArrive = false;
    private bool _bigQuackIsFlying = false;
    private float _min_X_SpawnPoint;
    private float _max_X_SpawnPoint;
    private float _min_Y_SpawnPoint;
    private float _max_Y_SpawnPoint;
    private float _nextLaserEyeSpawnTime = 0;
    private Vector3 _initialPosition;
    private Vector3 _bigQuackMiddlePosition = new Vector3(0, 0.81f, 0);
    private SpriteRenderer _bigQuackSpriteRen;

    public static byte s_fightStage = 0;
    public static byte s_LaserEyesDestroyed = 0;
    public static float s_bossHealth = 100;
    public static float s_playerHealth = 100;

    private void Awake()
    {
        // TODO: change it to be generic (e.g depended on screensize)
        _min_X_SpawnPoint = -4.5f;
        _max_X_SpawnPoint = 4.5f;
        _min_Y_SpawnPoint = -1;
        _max_Y_SpawnPoint = 3;
        _initialPosition = BigQuack.Instance.transform.position;
    }

    private void Start()
    {
        Weapon.s_weaponLocked = true;
        StartCoroutine(CoBigQuackArrive());
        _playerHealthAmount.text = s_playerHealth.ToString();
        _bossHealthAmount.text = s_bossHealth.ToString();
        _bigQuackSpriteRen = BigQuack.Instance.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _playerHealthAmount.text = s_playerHealth.ToString();

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

                _bossHealthAmount.text = s_bossHealth.ToString();

                break;
            case 2:
                if (_bigQuackIsFlying)
                {
                    _bigQuackIsFlying = false;
                    StartCoroutine(CoBigQuackHide());
                }

                if (s_LaserEyesDestroyed >= _laserEyeKilledGoal && !_bigQuackReadyToArrive)
                {
                    _bigQuackReadyToArrive = true;
                    StartCoroutine(CoWaitUntilAllEyesDestroyed());
                }
                else if (LaserEyeReadyToSpawn())
                {
                    SpawnLaserEye();
                }

                break;
            case 3:
                BigQuack.s_BigQuackIsShootable = true;

                _bossHealthAmount.text = s_bossHealth.ToString();

                break;
            case 4: 
                break;
            case 5:
                BigQuack.s_BigQuackIsShootable = true;

                break;
            default: break;
        }

        if (s_playerHealth <= 0)
        {
            Time.timeScale = 0;
            RoundEnd.Instance.ClearRemainings();
            RoundEnd.Instance.ShowRetryWindow();
        }
    }

    IEnumerator CoBigQuackArrive()
    {
        yield return new WaitUntil(() => Time.timeScale == 1);

        _bigQuackAnimator.SetTrigger("QuackArrive");

        while (_bigQuackUfoSound.volume < 1)
        {
            _bigQuackUfoSound.volume += 0.1f;

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(4);

        Time.timeScale = 0;
        _bigQuackArrivalDialogue.SetActive(true);

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
                _bossHealthAmount.color = healthNameColor;
                _playerHealthAmount.color = healthNameColor;
            }

            yield return new WaitForSeconds(0.04f);
        }
    }

    private Vector3 GetRandomSpawnpoint()
    {
        return new Vector3(
            UnityEngine.Random.Range(_min_X_SpawnPoint, _max_X_SpawnPoint),
            UnityEngine.Random.Range(_min_Y_SpawnPoint, _max_Y_SpawnPoint));
    }

    private void SpawnLaserEye()
    {
        Instantiate(_laserEyePrefab, GetRandomSpawnpoint(), Quaternion.Euler(Vector3.zero));

        _nextLaserEyeSpawnTime  = Time.time + LaserEye.spawnTime + UnityEngine.Random.Range(0, 0.5f);
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

    private bool AreAllLaserEyesDead()
    {
        if (GameObject.FindObjectsOfType<LaserEye>().Length > 0)
        {
            return false;
        }

        return true;
    }

    IEnumerator CoBiqQuackFlying()
    {
        var floatingInitialPosition = BigQuack.Instance.transform.position;
        _bigQuackAnimator.SetTrigger("FlyingAround");

        _bigQuackAnimator.enabled = false;

        BigQuack.Instance.transform.position = floatingInitialPosition;

        while(s_bossHealth > 66)
        {
            var newPosition = GetRandomSpawnpoint();
            var oldPosition = BigQuack.Instance.gameObject.transform.position;

            var dashDuration = UnityEngine.Random.Range(0.3f, 1);

            for(float f=0; f <= 1; f+= 0.01f)
            {
                BigQuack.Instance.gameObject.transform.position = Vector3.Lerp(oldPosition, newPosition, f);
                
                yield return new WaitForSeconds(0.01f);
            }

            var randWait = UnityEngine.Random.Range(0, 0.1f);

            yield return new WaitForSeconds(randWait);
        }

        s_fightStage = 2;
    }

    IEnumerator CoBigQuackHide()
    {

        Time.timeScale = 0;
        _afterStage1Dialogue.SetActive(true);

        yield return new WaitUntil(() => Time.timeScale == 1);

        var interpolationRatio = 3f;

        for (float f = 0; f <= interpolationRatio; f += 0.01f)
        {
            BigQuack.Instance.gameObject.transform.position = Vector3.Lerp(BigQuack.Instance.transform.position, _initialPosition, (f / interpolationRatio));
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator CoWaitUntilAllEyesDestroyed()
    {
        yield return new WaitUntil(() => AreAllLaserEyesDead());
        Weapon.s_weaponLocked = true;

        s_fightStage = 3;

        yield return new WaitForSeconds(1.5f);

        var bigQuackColor = _bigQuackSpriteRen.color;
        bigQuackColor.a = 0;
        _bigQuackSpriteRen.color = bigQuackColor;

        BigQuack.Instance.transform.position = _bigQuackMiddlePosition;

        for (float f=0; f<1.0f; f+=0.005f)
        {
            bigQuackColor.a += 0.005f;
            _bigQuackSpriteRen.color = bigQuackColor;

            yield return new WaitForSeconds(0.005f);
        }

        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0;
        _afterStage2Dialogue.SetActive(true);

        Weapon.s_weaponLocked = false;
    }
}
