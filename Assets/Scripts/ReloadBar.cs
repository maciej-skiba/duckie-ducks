using UnityEngine;

public class ReloadBar : MonoBehaviour
{
    public float reloadMultiplier = 1.0f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "FastReloadArea")
        {
            reloadMultiplier = 0.5f;
        }
        else if (collision.tag == "NormalReloadArea")
        {
            reloadMultiplier = 1.0f;
        }
        else if (collision.tag == "SlowReloadArea")
        {
            reloadMultiplier = 1.5f;
        }
    }
}
