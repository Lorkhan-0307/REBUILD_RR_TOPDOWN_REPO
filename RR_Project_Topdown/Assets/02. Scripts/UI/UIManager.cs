using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private AudioMixer audioMixer;
    private PauseManager pauseManager;

    // Start is called before the first frame update
    void Start()
    {
        pauseManager = GetComponent<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PauseMenu.activeSelf)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

    }

    public void Pause()
    {
        pauseManager.PauseGame();
        PauseMenu.SetActive(true);
    }

    public void Resume()
    {
        pauseManager.ResumeGame();
        PauseMenu.SetActive(false);
    }

    public void EndGame()
    {
        Debug.Log("End Game!");
    }
    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetBGVolume(float volume)
    {
        audioMixer.SetFloat("BGVolume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
