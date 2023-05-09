using UnityEngine;

public class DualPistol : Weapon
{
    [SerializeField] private Animator _secondaryPistolAnim;
    protected override void UpdateWeaponPosition()
    {
        if (_mousePosition.x < Screen.width - _weaponWidth * 0.7f
        && _mousePosition.x > _weaponWidth
        && Time.timeScale != 0)
        {
            _rectTransform.transform.position = new Vector3(_mousePosition.x + _weaponWidth / 2, _rectTransform.transform.position.y);
        }
    }

    protected override void ShootAnimation()
    {
        if (s_bulletsInMagazine % 2 == 0)
        {
            _animator.SetTrigger("Shoot");
        }
        else
        {
            _secondaryPistolAnim.SetTrigger("Shoot");
        }
    }
}
