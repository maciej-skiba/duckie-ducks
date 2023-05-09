using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Animator _reloadPointerAnimator;
    [SerializeField] protected GameObject _reloadBarWindow;
    [SerializeField] protected TextMeshProUGUI _reloadText;
    [SerializeField] protected TextMeshProUGUI _magazineText;
    [SerializeField] protected Slider _circularReloadSlider;
    [SerializeField] protected AudioSource _emptyMagazineSound;

    protected GameObject _reloadCursor;
    protected RectTransform _rectTransform;
    protected Animator _animator;
    protected float _fireRate;
    protected Vector3 _mousePosition;
    protected float _weaponWidth;
    protected int _maxBulletsInMagazine;
    protected float _delayBetweenRClicks = 0.1f;
    protected Animator _reloadTextAnimator;
    protected bool _reloadGameInProgress = false;
    protected const float PerfectReloadTime = 0.4f;
    protected const float GoodReloadTime = 0.8f;
    protected const float JammedReloadTime = 1.4f;
    protected bool _weaponRecoliing = false;
    protected float _lastShotTime;
    protected AudioSource _laserPistolShot;

    protected float _lastReloadSpeed = 1.0f;
    protected float _currentReloadingTime = 0;

    [HideInInspector] public float reloadSpeed;
    public WeaponChoice weaponChoice;
    public static bool s_isReloading = false;
    public static bool s_reloadingCircleIsAnimating = false;
    public static int s_bulletsInMagazine;


    public enum WeaponChoice
    {
        Pistol,
        DualPistol,
        SniperRifle
    }

    protected void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _animator = this.GetComponent<Animator>();
        _weaponWidth = this.GetComponent<RectTransform>().rect.width;
        _reloadCursor = _reloadPointerAnimator.gameObject;
        _reloadTextAnimator = _reloadText.GetComponent<Animator>();
        _laserPistolShot = this.GetComponent<AudioSource>();

        switch (weaponChoice)
        {
            case WeaponChoice.Pistol:
                reloadSpeed = 2.0f;
                _maxBulletsInMagazine = 6;
                s_bulletsInMagazine = _maxBulletsInMagazine;
                _fireRate = 0.5f;
                break;
            case WeaponChoice.DualPistol:
                reloadSpeed = 2.0f;
                _maxBulletsInMagazine = 12;
                s_bulletsInMagazine = _maxBulletsInMagazine;
                _fireRate = 0.3f;
                break;
            case WeaponChoice.SniperRifle:
                break;
            default:
                Debug.LogError("No weapon was chosen in Weapon.cs enum.");
                break;
        }

        _magazineText.text = $"{s_bulletsInMagazine}/{_maxBulletsInMagazine}";
    }

    protected void Update()
    {
        _mousePosition = Input.mousePosition;

        if (_weaponRecoliing && Time.time - _lastShotTime > _fireRate)
        {
            _weaponRecoliing = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && s_bulletsInMagazine < _maxBulletsInMagazine && !s_isReloading)
        {
            _reloadGameInProgress = true;
            StartCoroutine("CoStartReloading", _delayBetweenRClicks);
        }
        else if (Input.GetKeyDown(KeyCode.R) && _reloadGameInProgress)
        {
            _lastReloadSpeed = GetRealReloadSpeed();
            StartCoroutine("CoReload", _lastReloadSpeed);
        }

        UpdateWeaponPosition();

        if (Input.GetKeyDown(KeyCode.Mouse0) && !s_isReloading && Time.timeScale != 0)
        {
            if (s_bulletsInMagazine > 0)
            {
                if (!_weaponRecoliing)
                {
                    ShootAnimation();
                    s_bulletsInMagazine--;
                    _laserPistolShot.Play();
                    _magazineText.text = $"{s_bulletsInMagazine}/{_maxBulletsInMagazine}";
                    _weaponRecoliing = true;
                    _lastShotTime = Time.time;
                }
            }
            else
            {
                EmptyMagazine();
            }
        }

        if (s_reloadingCircleIsAnimating)
        {
            _currentReloadingTime += Time.deltaTime;

            float t = _currentReloadingTime / _lastReloadSpeed;
            _circularReloadSlider.value = Mathf.Lerp(0f, 1f, t);
        }
    }

    IEnumerator CoStartReloading(float DelayBetweenRClicks)
    {
        yield return new WaitForSeconds(DelayBetweenRClicks);

        _reloadBarWindow.SetActive(true);
        s_isReloading = true;
        _reloadPointerAnimator.SetTrigger("Reload");

        yield return new WaitForSeconds(2);

        if (_reloadBarWindow.activeInHierarchy)
        {
            _lastReloadSpeed = GetRealReloadSpeed();
            StartCoroutine("CoReload", _lastReloadSpeed);
        }
    }
    IEnumerator CoReload(float RealReloadSpeed)
    {
        _currentReloadingTime = 0;
        Debug.Log("CoReloading");
        _reloadBarWindow.SetActive(false);
        _circularReloadSlider.value = 0;
        s_reloadingCircleIsAnimating = true;

        yield return new WaitForSeconds(RealReloadSpeed);

        s_isReloading = false;
        _reloadGameInProgress = false;
        s_reloadingCircleIsAnimating = false;

        s_bulletsInMagazine = _maxBulletsInMagazine;
        _magazineText.text = $"{s_bulletsInMagazine}/{_maxBulletsInMagazine}";
    }

    protected float GetRealReloadSpeed()
    {
        float reloadMultiplier = _reloadCursor.GetComponent<ReloadBar>().reloadMultiplier;

        switch (reloadMultiplier)
        {
            case PerfectReloadTime:
                _reloadText.text = "Perfect";
                break;
            case GoodReloadTime:
                _reloadText.text = "Good";
                break;
            case JammedReloadTime:
                _reloadText.text = "Jammed";
                break;
        }

        _reloadTextAnimator.SetTrigger("ShowText");

        Debug.Log($"Final Reload speed:  {reloadSpeed * reloadMultiplier}");

        return reloadSpeed * reloadMultiplier;
    }

    protected void EmptyMagazine()
    {
        _emptyMagazineSound.Play();
        Debug.Log("Magazine is empty.");
    }

    protected virtual void UpdateWeaponPosition()
    {
        if (_mousePosition.x < Screen.width - _weaponWidth * 0.7f
            && _mousePosition.x > 0
            && Time.timeScale != 0)
        {
            _rectTransform.transform.position = new Vector3(_mousePosition.x + _weaponWidth / 2, _rectTransform.transform.position.y);
        }
    }

    protected virtual void ShootAnimation()
    {
        _animator.SetTrigger("Shoot");
    }    
}
