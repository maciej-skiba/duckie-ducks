using UnityEngine;

public class Weapon : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Animator _animator;
    private float _fireRate;
    private Vector3 _mousePosition;
    private float _weaponWidth;

    public static bool _canShoot = true;

    private WeaponChoice weaponChoice;
    enum WeaponChoice
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

        if (_mousePosition.x < Screen.width - _weaponWidth * 0.7f
            && _mousePosition.x > 0)
        {
            _rectTransform.transform.position = new Vector3(_mousePosition.x + _weaponWidth / 2, _rectTransform.transform.position.y);

        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _canShoot)
        {
            _animator.SetTrigger("Shoot");
        }

    }
}
