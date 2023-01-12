using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static public void LevelEnd()
    {
        SceneManager.LoadScene(0); //temporary
    }
}
