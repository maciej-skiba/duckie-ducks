using System.Collections;
using UnityEngine;

public class BigQuack : MonoBehaviour
{
    private SpriteRenderer _sprite;

    public static int s_BigQuack_Health = 100;

    private void Awake()
    {
        _sprite = this.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        print("quack shot");
        if (BossFight.s_fightStage == 0)
        {
            StartCoroutine(TakeDamage());

            if (s_BigQuack_Health <= 80)
            {

            }
        }
    }

    IEnumerator TakeDamage()
    {
        s_BigQuack_Health -= 2;

        for(int i = 254; i > 130; i++)
        {
            var eyeColor = _sprite.color;
            eyeColor.g = i;
            eyeColor.b = i;
            _sprite.color = eyeColor;

            yield return new WaitForSeconds(0.02f);
        }
    }
}
