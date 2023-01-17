using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorSprite;
    private Vector2 _cursorHotspot;
    private void Start()
    {
        _cursorHotspot = new Vector2(cursorSprite.width / 2, cursorSprite.height / 2);
        Cursor.SetCursor(cursorSprite, _cursorHotspot, CursorMode.ForceSoftware);
    }
}
