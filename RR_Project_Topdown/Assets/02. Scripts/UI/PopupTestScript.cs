using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTestScript : MonoBehaviour
{
    [SerializeField] PopUpManager PopUpManager;
    Popup popupComponent1;
    Popup popupComponent2;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            popupComponent1 = PopUpManager.Open("Test01");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            popupComponent2 = PopUpManager.Open("Test02");
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            PopUpManager.Close();
        }
    }
}
