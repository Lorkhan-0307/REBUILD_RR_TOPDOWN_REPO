using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
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
                PauseMenu.SetActive(true);
                pauseManager.PauseGame();
            }
            else
            {
                PauseMenu.SetActive(false);
                pauseManager.ResumeGame();
            }
        }

    }
}
