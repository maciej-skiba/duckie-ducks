using UnityEngine;
using UnityEngine.UI;

public class LaserEye : Bird
{
    [SerializeField] private AudioClip _deathSound;
    private AudioSource _shootSound;
    private SpriteRenderer _sprite;
    private Animator _animator;

    public static int spawnTime = 1;

    private void Awake()
    {
        _sprite = this.GetComponent<SpriteRenderer>();
        _animator = this.GetComponent<Animator>();
        _shootSound= this.GetComponent<AudioSource>();
    }

    void Start()
    {
        var eyeColor = _sprite.color;
        eyeColor.a = 0;
        _sprite.color = eyeColor;

        this.gameObject.SetActive(true);

        InvokeRepeating("ShootPlayer", 1.5f, 1.1f);
    }

    private void ShootPlayer()
    {
        this._shootSound.Play();
        _animator.SetTrigger("Shoot");
        BossFight.s_playerHealth -= 5;
        if (BossFight.s_playerHealth < 0) BossFight.s_playerHealth = 0;
    }

    override protected void Death()
    {
        AudioSource.PlayClipAtPoint(_deathSound, this.transform.position, 2f);
        Destroy(this.gameObject);
    }
}
