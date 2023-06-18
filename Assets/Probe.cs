using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probe : MonoBehaviour
{
    [SerializeField] private AudioClip _deathSound;
    private SpriteRenderer _sprite;

    [HideInInspector] public int probeHealth;

    private void Awake()
    {
        _sprite = this.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        probeHealth = 10;
    }

    private void OnMouseDown()
    {

        if (!Weapon.s_isReloading
            && Time.timeScale != 0
            && Weapon.s_bulletsInMagazine > 0
            && !Weapon.s_weaponLocked
            && !BigQuack.s_BigQuackIsShootable)
        {
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        print("Probe damaged.");
        this.probeHealth -= 5;

        if (probeHealth <= 0)
        {
            Death();
        }

        Color newColor = _sprite.color;
        float delta = 0.04f;

        while (newColor.g > 0.5f)
        {
            newColor.g -= delta;
            newColor.b -= delta;

            _sprite.color = newColor;

            yield return new WaitForSeconds(0.01f);
        }

        while (newColor.g < 1)
        {
            newColor.b += delta;
            newColor.g += delta;

            _sprite.color = newColor;

            yield return new WaitForSeconds(0.01f);
        }
    }
    private void Death()
    {
        BossFight.Instance.BubbleDisappear();
        AudioSource.PlayClipAtPoint(_deathSound, this.transform.position, 2f);
        Destroy(this.gameObject);
    }

    public IEnumerator CoAppear()
    {
        var probeColor = _sprite.color;
        probeColor.a = 0;
        _sprite.color = probeColor;

        for (float f = 0; f < 1.0f; f += 0.005f)
        {
            probeColor.a += 0.005f;
            _sprite.color = probeColor;

            yield return new WaitForSeconds(0.005f);
        }
    }
}
