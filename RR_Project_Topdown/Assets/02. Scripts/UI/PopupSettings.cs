using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSettings : Popup
{
    protected override void OnClose()
    {
        GameObject.Destroy(gameObject);

        Debug.Log("POPUP CLOSE");
    }

    protected override void OnOpen()
    {
        Debug.Log("POPUP OPEN");
    }
}

