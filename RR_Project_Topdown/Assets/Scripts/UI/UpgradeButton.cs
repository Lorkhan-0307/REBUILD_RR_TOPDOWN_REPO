using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    public void Set(UpgradeData upgradeData)
    {
        icon.sprite = upgradeData.icon;
    }

    public void Clean()
    {
        icon.sprite = null;
    }
}
