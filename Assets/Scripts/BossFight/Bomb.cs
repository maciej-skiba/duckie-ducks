using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float _bombDamage = 50.0f;

    private void OnMouseDown()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        this.transform.position += new Vector3(0, - 5 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BombTarget")
        {
            BossFight.s_playerHealth -= _bombDamage;
            Destroy(this.gameObject);
        }
    }
}