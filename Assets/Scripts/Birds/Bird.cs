using UnityEngine;

public abstract class Bird : MonoBehaviour
{
    protected short health;
    protected float speed;
    protected short pointsGain;
    protected BirdSpawnProperties.BirdTypes birdType;

    private void OnMouseDown()
    {
        if (!Weapon.s_isReloading && Time.timeScale != 0 && Weapon._bulletsInMagazine > 0)
        {
            ScoreManager.Instance.AddScore(pointsGain, Input.mousePosition);
            Death();
        }
    }

    protected virtual void Death()
    {
        Destroy(this.gameObject);
    }

    protected void Update()
    {
        this.transform.position += transform.right * speed * Time.deltaTime;
    }
}
