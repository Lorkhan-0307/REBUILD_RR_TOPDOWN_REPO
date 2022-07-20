using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class UI_PlugInTree : MonoBehaviour
{
    private PlayerPlugIn playerPlugIn;

    private void Awake()
    {
        transform.Find("GauntletBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerPlugIn.TryUnlockPlugIn(PlayerPlugIn.PlugInType.Gauntlet_Enhance))
            {
                Debug.Log("Cannot Unlock!");
            }
        };
        transform.Find("Health_BarrierMax1Btn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerPlugIn.TryUnlockPlugIn(PlayerPlugIn.PlugInType.Health_BarrierMax_1))
            {
                Debug.Log("Cannot Unlock!");
            }
        };
        transform.Find("Health_BarrierMax2Btn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerPlugIn.TryUnlockPlugIn(PlayerPlugIn.PlugInType.Health_BarrierMax_2))
            {
                Debug.Log("Cannot Unlock!");
            }
        };
        transform.Find("SummonAttackBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerPlugIn.TryUnlockPlugIn(PlayerPlugIn.PlugInType.SummonAttack))
            {
                Debug.Log("Cannot Unlock!");
            }
        };
        transform.Find("AttributeAttackBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerPlugIn.TryUnlockPlugIn(PlayerPlugIn.PlugInType.AttributeAttack))
            {
                Debug.Log("Cannot Unlock!");
            }
        };

    }

    public void SetPlayerPlugIn(PlayerPlugIn playerPlugIn)
    {
        this.playerPlugIn = playerPlugIn;
    }

}
