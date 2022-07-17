using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlugIn : MonoBehaviour
{
    public event EventHandler<OnPlugInUnlockedEventArgs> OnPlugInUnlocked;
    public class OnPlugInUnlockedEventArgs : EventArgs
    {
        public PlugInType plugInType;
    }

    public enum PlugInType
    {
        Gauntlet_Enhance,
        RangeAttack_Enhance,
        Health_BarrierMax,
        SummonAttack,
        AttributeAttack,
    }

    private List<PlugInType> unlockedPlugInTypeList;

    public PlayerPlugIn()
    {
        unlockedPlugInTypeList = new List<PlugInType>();
    }
    
    public void UnlockPlugIn(PlugInType plugInType)
    {
        if (!IsPlugInUnlocked(plugInType))
        {
            unlockedPlugInTypeList.Add(plugInType);
            OnPlugInUnlocked?.Invoke(this, new OnPlugInUnlockedEventArgs { plugInType = plugInType });
        }
    }

    public bool IsPlugInUnlocked(PlugInType plugInType)
    {
        return unlockedPlugInTypeList.Contains(plugInType);
    }
}
