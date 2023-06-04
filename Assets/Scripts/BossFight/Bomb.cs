using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnMouseDown()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        this.transform.position += new Vector3(0, - 5 * Time.deltaTime, 0);
    }
}