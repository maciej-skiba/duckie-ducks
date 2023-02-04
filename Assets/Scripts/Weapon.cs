using System.Collections;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator _reloadCursorAnimator;
    [SerializeField] private GameObject _reloadBarWindow;
    [SerializeField] private TextMeshProUGUI _reloadText;

    private GameObject _reloadCursor;
    private RectTransform _rectTransform;
    private Animator _animator;
    private float _fireRate;
    private Vector3 _mousePosition;
    private float _weaponWidth;
    private int _bulletsInMagazine;
    private int _maxBulletsInMagazine;
    private float _delayBetweenRClicks = 0.1f;
    private Animator _reloadTextAnimator;
    private bool _reloadGameInProgress = false;

    [HideInInspector] public float reloadSpeed;
    public WeaponChoice weaponChoice;
    public static bool s_isReloading = false;

    public enum WeaponChoice
    {
        Pistol,
        DualPistol,
        SniperRifle
    }

    private void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _animator = this.GetComponent<Animator>();
        _weaponWidth = this.GetComponent<RectTransform>().rect.width;
        _reloadCursor = _reloadCursorAnimator.gameObject;
        _reloadTextAnimator = _reloadText.GetComponent<Animator>();

        switch (weaponChoice)
        {
            case WeaponChoice.Pistol:
                reloadSpeed = 2.0f;
                _maxBulletsInMagazine = 6;
                _bulletsInMagazine = _maxBulletsInMagazine;
                break;
            case WeaponChoice.DualPistol:
                break;
            case WeaponChoice.SniperRifle:
                break;
            default:
                Debug.LogError("No weapon was chosen in Weapon.cs enum.");
                break;
        }
    }

    private void Start()
    {
        switch(weaponChoice)
        {
            case WeaponChoice.Pistol:
                _fireRate = 0.5f;
                break;
            default:
                _fireRate = 0.5f;
                break;
        }

    }

    private void Update()
    {
        _mousePosition = Input.mousePosition;

        Debug.Log($"is reloading: {s_isReloading}, ");

        if (Input.GetKeyDown(KeyCode.R) && _bulletsInMagazine < _maxBulletsInMagazine && !s_isReloading)
        {
            StartCoroutine("CoStartReloading", _delayBetweenRClicks);
        }
        else if (Input.GetKeyDown(KeyCode.R) && !_reloadGameInProgress)
        {
            _reloadGameInProgress = true;
            StartCoroutine("CoReload", GetRealReloadSpeed());
        }

        if (_mousePosition.x < Screen.width - _weaponWidth * 0.7f
        && _mousePosition.x > 0
        && Time.timeScale != 0)
        {
            _rectTransform.transform.position = new Vector3(_mousePosition.x + _weaponWidth / 2, _rectTransform.transform.position.y);

        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !s_isReloading)
        {
            if (_bulletsInMagazine > 0)
            {
                _bulletsInMagazine--;
                _animator.SetTrigger("Shoot");
            }
            else
            {
                EmptyMagazine();
            }
        }

    }

    IEnumerator CoStartReloading(float DelayBetweenRClicks)
    {
        yield return new WaitForSeconds(DelayBetweenRClicks);

        _reloadBarWindow.SetActive(true);
        s_isReloading = true;
        _reloadCursorAnimator.SetTrigger("Reload");

        yield return new WaitForSeconds(1.0f);

        if (!_reloadGameInProgress)
        {
            StartCoroutine("CoReload", GetRealReloadSpeed());
        }
    }
    IEnumerator CoReload(float RealReloadSpeed)
    {
        Debug.Log("CoReloading");
        _reloadBarWindow.SetActive(false);
        _reloadCursorAnimator.SetTrigger("EndReload");

        yield return new WaitForSeconds(RealReloadSpeed);

        s_isReloading = false;
        _reloadGameInProgress = false;
    }

    private float GetRealReloadSpeed()
    {
        float reloadMultiplier = _reloadCursor.GetComponent<ReloadBar>().reloadMultiplier;
        _reloadTextAnimator.SetTrigger("ShowText");

        switch (reloadMultiplier)
        {
            case 0.5f:
                _reloadText.text = "Perfect";
                break;
            case 1.0f:
                _reloadText.text = "Good";
                break;
            case 1.5f:
                _reloadText.text = "Jammed";
                break;
        }

        Debug.Log($"Final Reload speed:  {reloadSpeed * reloadMultiplier}");

        return reloadSpeed * reloadMultiplier;
    }

    private void EmptyMagazine()
    {
        // TODO: do something, e.g. play empty magazine sound
        Debug.Log("Magazine is empty.");
    }
}
