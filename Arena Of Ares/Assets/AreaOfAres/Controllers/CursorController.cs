using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    [SerializeField] private Texture2D _cursorTexture;

    private void Awake()
    {
        Cursor.SetCursor(_cursorTexture, new Vector2(10, 10), CursorMode.Auto);
    }
}
