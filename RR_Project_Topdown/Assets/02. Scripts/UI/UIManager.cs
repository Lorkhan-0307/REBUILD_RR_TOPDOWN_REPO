using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    //[SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject GuideUI;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text userName;
    [SerializeField] private AudioMixer audioMixer;

    // Added Popup Manager
    [SerializeField] PopUpManager PopUpManager;
    [SerializeField] DialogueTrigger dialogueTrigger;
    [SerializeField] ScoreManager scoreManager;
    public bool canInteract;
    public bool isGamePaused;
    //private PauseManager pauseManager;

    private int enemyKillScore;
    private int bossKillScore;
    private int timeScore;
    private string user;
    private bool killScoreUpdated;
    private bool timeScoreUpdated;

    // Start is called before the first frame update
    void Start()
    {
        //pauseManager = GetComponent<PauseManager>();
        //dialogueTrigger.TriggerDialogue();
        isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale >0)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if(killScoreUpdated & timeScoreUpdated)
        {
            OpenGameOverUI();
            UpdateScoreText(enemyKillScore, bossKillScore, timeScore);
        }
        //Debug.Log(PlayerPrefs.GetFloat("SFXVolume"));
    }

    public void UpdateKillScore(int enemyScore, int bossScore)
    {
        enemyKillScore = enemyScore;
        bossKillScore = bossScore;
        killScoreUpdated = true;
    }

    public void UpdateTimeScore(int time)
    {
        timeScore = time;
        timeScoreUpdated = true;
    }

    public void UpdateUserName()
    {
        user = userName.text;
        Debug.Log(user);
        scoreManager.AddScore(new Score(user, timeScore, bossKillScore, enemyKillScore));
        scoreManager.SaveScore();
    }

    public void OpenGameOverUI()
    {
        GameOverUI.SetActive(true);
    }

    public void UpdateScoreText(int enemyKillScore, int bossKillScore, int timeScore)
    {
        scoreText.text = $"Clear Time { timeScore / 60:00} : {timeScore % 60:00}\n Enemy Kill Count: {enemyKillScore}\n Boss Kill Count: {bossKillScore}";
    }

    public void Pause()
    {
        Popup PauseMenu;
        PauseMenu = PopUpManager.Open("PauseMenu");
        isGamePaused = true;
    }

    public void Resume()
    {
        PopUpManager.Close();
        isGamePaused = false;
    }

    public void EndGame()
    {
        Debug.Log("End Game!");
        Application.Quit();
    }
    public void OpenOptions()
    {
        OptionsMenu.SetActive(true);
    }

    public void OpenGuideUI()
    {
        canInteract = true;
        GuideUI.SetActive(true);
    }
    public void CloseGuideUI()
    {
        canInteract = false;
        GuideUI.SetActive(false);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        //PlayerPrefs.SetFloat("SFXVolume", volume);
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
