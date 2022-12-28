using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject AnyKeyText;
    [SerializeField] private GameObject ButtonPanel;
    [SerializeField] private GameObject FadeImage;
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

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
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void InfinateGame()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 2));
    }

    public void ZatoichiGame()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 3));
    }

    public void EndGame()
    {
        Debug.Log("End Game");
        Application.Quit();
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        FadeImage.SetActive(true);
        transition.SetBool("Start", true);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
