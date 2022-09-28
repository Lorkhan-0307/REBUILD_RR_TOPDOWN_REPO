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


        //버튼 추가함(Utility 3,4, SummonAttack2,3,4, ice,fire,corrosion 2,3,4)
        plugInButtonList = new List<PlugInButton>();
        plugInButtonList.Add(new PlugInButton(transform.Find("GauntletAttack_1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.GauntletAttack_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("GauntletAttack_2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.GauntletAttack_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("GauntletAttack_3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.GauntletAttack_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("GauntletAttack_4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.GauntletAttack_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("Utility_1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Utility_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("Utility_2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Utility_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("Utility_3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Utility_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("Utility_4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.Utility_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("SummonAttack1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.SummonAttack_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("SummonAttack2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.SummonAttack_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("SummonAttack3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.SummonAttack_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("SummonAttack4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.SummonAttack_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("FireAttack1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.FireAttack_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("FireAttack2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.FireAttack_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("FireAttack3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.FireAttack_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("FireAttack4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.FireAttack_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("IceAttack1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.IceAttack_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("IceAttack2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.IceAttack_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("IceAttack3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.IceAttack_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("IceAttack4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.IceAttack_4));
        plugInButtonList.Add(new PlugInButton(transform.Find("CorrosionAttack1Btn"), playerPlugIn, PlayerPlugIn.PlugInType.CorrosionAttack_1));
        plugInButtonList.Add(new PlugInButton(transform.Find("CorrosionAttack2Btn"), playerPlugIn, PlayerPlugIn.PlugInType.CorrosionAttack_2));
        plugInButtonList.Add(new PlugInButton(transform.Find("CorrosionAttack3Btn"), playerPlugIn, PlayerPlugIn.PlugInType.CorrosionAttack_3));
        plugInButtonList.Add(new PlugInButton(transform.Find("CorrosionAttack4Btn"), playerPlugIn, PlayerPlugIn.PlugInType.CorrosionAttack_4));


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
