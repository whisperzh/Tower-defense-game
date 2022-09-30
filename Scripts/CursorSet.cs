using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSet : MonoBehaviour
{
    public Texture2D crosshairUnpress, crosshairPress;

    private void Start()
    {
        Cursor.visible = false;
    }

    public void OnGUI()
    {
        Texture2D crosshairTexture;
        if (Input.GetMouseButton(0))
        {
            crosshairTexture = crosshairPress;
        }
        else
        {
            crosshairTexture = crosshairUnpress;
        }

        Vector3 mousePos = Input.mousePosition;
        Rect pos = new Rect(mousePos.x - crosshairTexture.width * 0.1f, Screen.height - mousePos.y - crosshairTexture.height * 0.1f,
          crosshairTexture.width * 0.4f, crosshairTexture.height * 0.4f);
        GUI.DrawTexture(pos, crosshairTexture);
    }
}
