using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Popup : MonoBehaviour
{
    public void Open()
    {
        PopUpManager.Instance.Open(this);

        OnOpen();
    }

    public void Close()
    {
        PopUpManager.Instance.Close(this);

        OnClose();
    }
    protected abstract void OnOpen();
    protected abstract void OnClose();
}
