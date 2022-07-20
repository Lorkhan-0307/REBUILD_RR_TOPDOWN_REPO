using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlugIn
{
    public event EventHandler<OnPlugInUnlockedEventArgs> OnPlugInUnlocked;
    public class OnPlugInUnlockedEventArgs : EventArgs
    {
        public PlugInType plugInType;
    }

    public enum PlugInType
    {
        None,
        Gauntlet_Enhance,
        Health_BarrierMax_1,
        Health_BarrierMax_2,
        SummonAttack,
        AttributeAttack,
    }

    private List<PlugInType> unlockedPlugInTypeList;

    public PlayerPlugIn()
    {
        unlockedPlugInTypeList = new List<PlugInType>();
    }
    
    private void UnlockPlugIn(PlugInType plugInType)
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

    public PlugInType GetPlugInRequirement(PlugInType plugInType)
    {
        switch (plugInType)
        {
            case PlugInType.Health_BarrierMax_2: return PlugInType.Health_BarrierMax_1;
        }
        return PlugInType.None;
    }

    public bool TryUnlockPlugIn(PlugInType plugInType)
    {
        PlugInType plugInRequirement = GetPlugInRequirement(plugInType);

        if (plugInRequirement != PlugInType.None)
        {
            if (IsPlugInUnlocked(plugInRequirement))
            {
                UnlockPlugIn(plugInType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            UnlockPlugIn(plugInType);
            return true;
        }
    }
}
