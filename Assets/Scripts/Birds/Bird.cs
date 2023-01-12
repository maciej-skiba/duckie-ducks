using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Bird : MonoBehaviour
{
    protected short health;
    protected float speed;
    protected short pointsGain;


    private void OnMouseDown()
    {
        Death();
        ScoreManager.score += pointsGain;
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
