using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;

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
    [SerializeField] private Animator _bigQuackBubbleAnimator;
    [SerializeField] private GameObject _bigQuackArrivalDialogue;
    [SerializeField] private GameObject _afterStage1Dialogue;
    [SerializeField] private GameObject _afterStage2Dialogue;
    [SerializeField] private GameObject _afterStage3Dialogue;
    [SerializeField] private GameObject _afterStage4Dialogue;
    [SerializeField] private AudioSource _bigQuackUfoSound;
    [SerializeField] private Image[] _healthBars;
    [SerializeField] private Sprite _healthBarSprite0;
    [SerializeField] private Sprite _healthBarSprite15;
    [SerializeField] private Sprite _healthBarSprite35;
    [SerializeField] private Sprite _healthBarSprite50;
    [SerializeField] private Sprite _healthBarSprite65;
    [SerializeField] private Sprite _healthBarSprite80;
    [SerializeField] private TextMeshProUGUI[] _healthNames;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _laserEyePrefab;
    [SerializeField] private TextMeshProUGUI _playerHealthAmount;
    [SerializeField] private TextMeshProUGUI _bossHealthAmount;
    [SerializeField] private BombSpawner _bombSpawner;

    private BossFight() { }

    private byte _laserEyeKilledGoal = 2;
    private bool _bigQuackReadyToArrive = false;
    private bool _bigQuackIsFlying = false;
    private bool _bigQuackHidden = true;
    private float _min_X_SpawnPoint;
    private float _max_X_SpawnPoint;
    private float _min_Y_SpawnPoint;
    private float _max_Y_SpawnPoint;
    private float _nextLaserEyeSpawnTime = 0;
    private Vector3 _initialPosition;
    private Vector3 _bigQuackMiddlePosition = new Vector3(0, 0.81f, 0);
    private Vector3 _bigQuackPreviousSpawnpoint;
    private Vector3 _laserEyePreviousSpawnpoint;
    private SpriteRenderer _bigQuackSpriteRen;

    public static byte s_fightStage;
    public static byte s_LaserEyesDestroyed;
    public static float s_bossHealth;
    public static float s_playerHealth;
    public static bool s_finalStageStarted = false;

    public static BossFight Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        s_fightStage = 0;
        s_LaserEyesDestroyed = 0;
        s_bossHealth = 100;
        s_playerHealth = 100;

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
                    _bigQuackIsFlying = true;
                    StartCoroutine(CoBiqQuackFlying(targetHealth: 65, nextStage: 2));
                }

                _bossHealthAmount.text = s_bossHealth.ToString();

                break;
            case 2:
                if (_bigQuackIsFlying)
                {
                    BigQuack.s_BigQuackIsShootable = false;
                    _bigQuackIsFlying = false;
                    StartCoroutine(CoBigQuackHide(_initialPosition));
                }

                if (s_LaserEyesDestroyed >= _laserEyeKilledGoal && !_bigQuackReadyToArrive)
                {
                    _bigQuackReadyToArrive = true;
                    StartCoroutine(CoWaitUntilAllEyesDestroyed());
                }
                
                if (LaserEyeReadyToSpawn() && s_LaserEyesDestroyed < _laserEyeKilledGoal)
                {
                    SpawnLaserEye();
                }

                break;
            case 3:
                if (!_bigQuackIsFlying && Time.timeScale != 0)
                {
                    _bigQuackIsFlying = true;
                    StartCoroutine(CoBiqQuackFlying(targetHealth: 35, nextStage: 4));
                }

                _bossHealthAmount.text = s_bossHealth.ToString();

                break;
            case 4:
                if(_bigQuackIsFlying)
                {
                    _bigQuackIsFlying = false;
                    BigQuack.s_BigQuackIsShootable = false;

                    StartCoroutine(CoBigQuackHide(_initialPosition));
                    StartCoroutine(_bombSpawner.CoSpawnBombs());
                }

                break;
            case 5:
                if (!s_finalStageStarted)
                {
                    s_finalStageStarted = true;

                    StartCoroutine(CoFinalStageBubbleVanishing());
                    StartCoroutine(_bombSpawner.CoSpawnBombs());
                }

                if (LaserEyeReadyToSpawn())
                {
                    SpawnLaserEye();
                }

                break;
            default: break;
        }

        _playerHealthAmount.text = s_playerHealth.ToString();
        CheckHealthAndCheckHpBarImage();

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

        _bigQuackHidden = false;

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

    private Vector3 GetRandomSpawnpoint(Vector3 previousSpawnpoint)
    {
        var newSpawnpoint = new Vector3(
            UnityEngine.Random.Range(_min_X_SpawnPoint, _max_X_SpawnPoint),
            UnityEngine.Random.Range(_min_Y_SpawnPoint, _max_Y_SpawnPoint));

        while (Vector3.Distance(previousSpawnpoint, newSpawnpoint) < 5.0f)
        {
            newSpawnpoint = new Vector3(
            UnityEngine.Random.Range(_min_X_SpawnPoint, _max_X_SpawnPoint),
            UnityEngine.Random.Range(_min_Y_SpawnPoint, _max_Y_SpawnPoint));
        }

        return newSpawnpoint;
    }

    private void SpawnLaserEye()
    {
        var newSpawnpoint = GetRandomSpawnpoint(_laserEyePreviousSpawnpoint);
        Instantiate(_laserEyePrefab, newSpawnpoint, Quaternion.Euler(Vector3.zero));
        _laserEyePreviousSpawnpoint = newSpawnpoint;    

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

    IEnumerator CoBiqQuackFlying(int targetHealth, byte nextStage)
    {
        var floatingInitialPosition = BigQuack.Instance.transform.position;
        _bigQuackAnimator.SetTrigger("FlyingAround");

        _bigQuackAnimator.enabled = false;

        BigQuack.Instance.transform.position = floatingInitialPosition;

        while (s_bossHealth > targetHealth)
        {
            var newPosition = GetRandomSpawnpoint(_bigQuackPreviousSpawnpoint);
            _bigQuackPreviousSpawnpoint = BigQuack.Instance.gameObject.transform.position;

            var dashDuration = UnityEngine.Random.Range(0.3f, 1);

            for(float f=0; f <= 1; f+= 0.01f)
            {
                if (s_bossHealth <= targetHealth) goto End;

                BigQuack.Instance.gameObject.transform.position = Vector3.Lerp(_bigQuackPreviousSpawnpoint, newPosition, f);
                
                yield return new WaitForSeconds(0.01f);
            }

            var randWait = UnityEngine.Random.Range(0, 0.1f);

            for (int i = 0; i < 10; i++)
            {
                if (s_bossHealth <= targetHealth) goto End;

                yield return new WaitForSeconds(randWait / 10.0f);
            }
        }

        End:
            s_fightStage = nextStage;
    }

    IEnumerator CoBigQuackHide(Vector3 positionToHide)
    {
        Time.timeScale = 0;
        if (s_fightStage == 2) _afterStage1Dialogue.SetActive(true);
        if (s_fightStage == 4) _afterStage3Dialogue.SetActive(true);

        yield return new WaitUntil(() => Time.timeScale == 1);

        var interpolationRatio = 3f;

        for (float f = 0; f <= interpolationRatio; f += 0.01f)
        {
            BigQuack.Instance.gameObject.transform.position = Vector3.Lerp(BigQuack.Instance.transform.position, positionToHide, (f / interpolationRatio));
            yield return new WaitForSeconds(0.01f);
        }

        _bigQuackHidden = true;
        Time.timeScale = 1;
        BubbleAppear();
    }

    IEnumerator CoWaitUntilAllEyesDestroyed()
    {
        yield return new WaitUntil(() => AreAllLaserEyesDead());
        Weapon.s_weaponLocked = true;

        yield return new WaitForSeconds(1.5f);

        var bigQuackColor = _bigQuackSpriteRen.color;
        bigQuackColor.a = 0;
        _bigQuackSpriteRen.color = bigQuackColor;

        BigQuack.Instance.transform.position = _bigQuackMiddlePosition;

        var probe = FindObjectOfType<Probe>();
        StartCoroutine(probe.CoAppear());
        BubbleAppear();

        StartCoroutine(CoBigQuackAppear());

        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0;

        _afterStage2Dialogue.SetActive(true);
        s_fightStage = 3;

        Weapon.s_weaponLocked = false;
    }

    private void CheckHealthAndCheckHpBarImage()
    {
        List<float> healths = new List<float> { BossFight.s_playerHealth, BossFight.s_bossHealth};

        for (int i = 0; i <= 1; i++)
        {
            if (healths[i] <= 0) _healthBars[i].sprite = _healthBarSprite0;  
            else if (healths[i] <= 15) _healthBars[i].sprite = _healthBarSprite15;  
            else if (healths[i] <= 35) _healthBars[i].sprite = _healthBarSprite35;  
            else if (healths[i] <= 50) _healthBars[i].sprite = _healthBarSprite50;  
            else if (healths[i] <= 65) _healthBars[i].sprite = _healthBarSprite65;  
            else if (healths[i] <= 80) _healthBars[i].sprite = _healthBarSprite80;
        }
    }

    public void BubbleDisappear()
    {
        _bigQuackBubbleAnimator.SetTrigger("Disappear");
        BigQuack.s_BigQuackIsShootable = true;
    }

    public void BubbleAppear()
    {
        _bigQuackBubbleAnimator.SetTrigger("Appear");
        BigQuack.s_BigQuackIsShootable = false;
    }

    IEnumerator CoFinalStageBubbleVanishing()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(3.0f, 5.0f));
        BubbleDisappear();
        yield return new WaitForSeconds(UnityEngine.Random.Range(2.0f, 3.0f));
        BubbleAppear();
    }

    IEnumerator CoBigQuackAppear()
    {
        var bigQuackColor = _bigQuackSpriteRen.color;
        bigQuackColor.a = 0;
        _bigQuackSpriteRen.color = bigQuackColor;

        for (float f = 0; f < 1.0f; f += 0.005f)
        {
            bigQuackColor.a += 0.005f;
            _bigQuackSpriteRen.color = bigQuackColor;

            yield return new WaitForSeconds(0.005f);
        }
    }

    public IEnumerator CoPrepareLastFightStage()
    {
        var bombs= FindObjectsOfType<Bomb>();

        for (int i = 0; i < bombs.Length; i++)
        {
            Destroy(bombs[i].gameObject);
        }

        _bombSpawner.amountOfBombs = 120;
        _bombSpawner.PrepareBombSpawns();

        var bigQuackColor = _bigQuackSpriteRen.color;
        bigQuackColor.a = 0;
        _bigQuackSpriteRen.color = bigQuackColor;

        BigQuack.Instance.transform.position = _bigQuackMiddlePosition;

        StartCoroutine(CoBigQuackAppear());

        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0;

        _afterStage4Dialogue.SetActive(true);
        s_fightStage = 5;

        Weapon.s_weaponLocked = false;
    }
}
