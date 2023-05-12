using UnityEngine;
using UnityEngine.UI;

public class LaserEye : Bird
{
    private SpriteRenderer _sprite;
    private Animator _animator;
    private int _animLayer = 0;

    private void Awake()
    {
        _sprite = this.GetComponent<SpriteRenderer>();
        _animator = this.GetComponent<Animator>();
    }

    void Start()
    {
        var eyeColor = _sprite.color;
        eyeColor.a = 0;
        _sprite.color = eyeColor;

        this.gameObject.SetActive(true);

        InvokeRepeating("ShootPlayer", 1.5f, 1f);
    }

    private void ShootPlayer()
    {
        _animator.SetTrigger("Shoot");
        BossFight.s_playerHealth -= 5;
    }

}
