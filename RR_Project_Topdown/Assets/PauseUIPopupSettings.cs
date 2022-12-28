using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIPopupSettings : Popup
{
    protected override void OnClose()
    {
        GameObject.Destroy(gameObject);
        Time.timeScale = 1f;
        Debug.Log("POPUP CLOSE");
    }

    protected override void OnOpen()
    {
        Debug.Log("POPUP OPEN");
        Time.timeScale = 0f;

    }

    public void Resume()
    {
        FindObjectOfType<UIManager>().Resume();
    }
    public void EndGame()
    {
        FindObjectOfType<UIManager>().EndGame();
    }

    public void SetSFXVolume(float volume)
    {
        FindObjectOfType<UIManager>().SetSFXVolume(volume);
    }

    public void SetBGVolume(float volume)
    {
        FindObjectOfType<UIManager>().SetBGVolume(volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

}

