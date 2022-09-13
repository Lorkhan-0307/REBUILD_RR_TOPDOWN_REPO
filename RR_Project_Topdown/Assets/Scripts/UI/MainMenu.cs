using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject AnyKeyText;
    [SerializeField] private GameObject ButtonPanel;

    private void Awake()
    {
        AnyKeyText.SetActive(true);
        ButtonPanel.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            //Debug.Log("Main menu");
            AnyKeyText.SetActive(false);
            ButtonPanel.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndGame()
    {
        Debug.Log("End Game");
        Application.Quit();
    }
}
