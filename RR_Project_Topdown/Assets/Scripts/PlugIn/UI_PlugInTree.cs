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
            playerPlugIn.UnlockPlugIn(PlayerPlugIn.PlugInType.Gauntlet_Enhance);
        };
        transform.Find("RangeAttackBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            playerPlugIn.UnlockPlugIn(PlayerPlugIn.PlugInType.RangeAttack_Enhance);
        };
        transform.Find("Health_BarrierMaxBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            playerPlugIn.UnlockPlugIn(PlayerPlugIn.PlugInType.Health_BarrierMax);
        };
        transform.Find("SummonAttackBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            playerPlugIn.UnlockPlugIn(PlayerPlugIn.PlugInType.SummonAttack);
        };
        transform.Find("AttributeAttackBtn").GetComponent<Button_UI>().ClickFunc = () =>
        {
            playerPlugIn.UnlockPlugIn(PlayerPlugIn.PlugInType.AttributeAttack);
        };

    }

    public void SetPlayerPlugIn(PlayerPlugIn playerPlugIn)
    {
        this.playerPlugIn = playerPlugIn;
    }

}
