using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public Texture2D cursor_normal;
    public Vector2 normalCursorHotSpot;

    public Texture2D cursor_OnButton;
    public Vector2 onButtonCursorSpot;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Cursor.SetCursor(cursor_normal, normalCursorHotSpot, CursorMode.Auto);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Cursor.SetCursor(cursor_OnButton, normalCursorHotSpot, CursorMode.Auto);
        }
    }

    public void OnButtonCursorEnter() {
        Cursor.SetCursor(cursor_OnButton, normalCursorHotSpot, CursorMode.Auto);
    }
    public void OnButtonCursorExit() {
        Cursor.SetCursor(cursor_normal, normalCursorHotSpot, CursorMode.Auto);
    }
}
