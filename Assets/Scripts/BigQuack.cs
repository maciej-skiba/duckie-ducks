using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class BigQuack : MonoBehaviour
{
    private SpriteRenderer _sprite;

    private BigQuack() { }
     
    public static int s_BigQuack_Health = 100;
    public static bool s_BigQuackIsShootable = false;
    public static BigQuack Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        _sprite = this.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (s_BigQuackIsShootable)
        {
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        print("BigQuack damaged.");
        BossFight.s_bossHealth -= 5;

        Color newColor = _sprite.color;
        float delta = 0.04f;

        while(newColor.g > 0.5f)
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
}
