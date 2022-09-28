using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSettings : Popup
{
    public override void Close()
    {
        GameObject.Destroy(gameObject);
        Debug.Log("POPUP CLOSE");
    }

    public override void Open()
    {
        Debug.Log("POPUP OPEN");
    }
}

