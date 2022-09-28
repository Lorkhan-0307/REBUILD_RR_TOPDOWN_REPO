using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTestScript : MonoBehaviour
{
    [SerializeField] PopUpManager PopUpManager;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PopUpManager.Open("Test01");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PopUpManager.Open("Test02");
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            PopUpManager.Close();
        }
    }
}
