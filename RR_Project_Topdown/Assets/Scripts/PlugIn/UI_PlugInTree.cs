using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class UI_PlugInTree : MonoBehaviour
{
    private PlayerPlugIn playerPlugIn;
    private List<PlugInButton> plugInButtonList;


    public void SetPlayerPlugIn(PlayerPlugIn playerPlugIn)
    {
        this.playerPlugIn = playerPlugIn;

        plugInButtonList = new List<PlugInButton>();
        plugInButtonList.Add(new PlugInButton(transform.Find("GauntletBtn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Enhance));
        plugInButtonList.Add(new PlugInButton(transform.Find("Health_BarrierMax1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Health_BarrierMax_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("Health_BarrierMax2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Health_BarrierMax_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("SummonAttackBtn"), playerPlugIn, PlayerPlugIn.PlugInType.SummonAttack));
        plugInButtonList.Add(new PlugInButton(transform.Find("AttributeAttackBtn"), playerPlugIn, PlayerPlugIn.PlugInType.AttributeAttack));

    }

    private class PlugInButton
    {
        private Transform transform;
        private Image image;
        private Image backgroundImage;
        private PlayerPlugIn playerPlugIn;
        private PlayerPlugIn.PlugInType plugInType;

        public PlugInButton(Transform transform, PlayerPlugIn playerPlugIn, PlayerPlugIn.PlugInType plugInType)
        {
            this.transform = transform;
            this.playerPlugIn = playerPlugIn;
            this.plugInType = plugInType;

            transform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                if (!playerPlugIn.TryUnlockPlugIn(plugInType))
                {
                    Debug.Log("Cannot Unlock!");
                }
            };
        }
    }

}
