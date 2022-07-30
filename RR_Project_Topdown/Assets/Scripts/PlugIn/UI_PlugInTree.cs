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
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Damage1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Damage_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Damage2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Damage_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Damage3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Damage_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Damage4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Damage_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Range1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Range_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Range2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Range_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Range3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Range_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Range4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Range_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Speed1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Speed_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Speed2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Speed_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Speed3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Speed_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("Gauntlet_Speed4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Gauntlet_Speed_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("Health_BarrierMax1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Health_BarrierMax_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("Health_BarrierMax2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Health_BarrierMax_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("SummonAttackBtn"), playerPlugIn, PlayerPlugIn.PlugInType.SummonAttack));
        plugInButtonList.Add(new PlugInButton(transform.Find("FireAttackBtn"), playerPlugIn, PlayerPlugIn.PlugInType.FireAttack_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("IceAttackBtn"), playerPlugIn, PlayerPlugIn.PlugInType.IceAttack_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("ElectricAttackBtn"), playerPlugIn, PlayerPlugIn.PlugInType.ElectricAttack_1));


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
