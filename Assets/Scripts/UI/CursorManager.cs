using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorSprite;
    [SerializeField] private GameObject _reloadingCursor;
    private Vector2 _cursorHotspot;

    private void Start()
    {
        _cursorHotspot = new Vector2(_cursorSprite.width / 2, _cursorSprite.height / 2);
        Cursor.SetCursor(_cursorSprite, _cursorHotspot, CursorMode.ForceSoftware);

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            _reloadingCursor.SetActive(false);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        if (Weapon.s_reloadingCircleIsAnimating && !_reloadingCursor.activeInHierarchy)
        {
            Cursor.visible = false;
            _reloadingCursor.transform.position = Input.mousePosition;
            _reloadingCursor.SetActive(true);
        }
        else if(!Weapon.s_reloadingCircleIsAnimating && _reloadingCursor.activeInHierarchy)
        {
            _reloadingCursor.SetActive(false);
            Cursor.visible = true;
        }
        else if (Weapon.s_reloadingCircleIsAnimating && _reloadingCursor.activeInHierarchy)
        {
            _reloadingCursor.transform.position = Input.mousePosition;
        }
    }
}
