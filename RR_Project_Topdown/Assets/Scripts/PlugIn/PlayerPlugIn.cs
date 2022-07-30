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
        Gauntlet_Damage_1, //Increase Gauntlet Damage
        Gauntlet_Damage_2, //Decrease Attack Speed, Gauntlet Damage * 2
        Gauntlet_Damage_3, //
        Gauntlet_Damage_4, //
        Gauntlet_Range_1,
        Gauntlet_Range_2,
        Gauntlet_Range_3,
        Gauntlet_Range_4,
        Gauntlet_Speed_1,
        Gauntlet_Speed_2,
        Gauntlet_Speed_3,
        Gauntlet_Speed_4,
        Health_BarrierMax_1,
        Health_BarrierMax_2,
        SummonAttack,
        FireAttack_1,
        FireAttack_2,
        FireAttack_3,
        FireAttack_4,
        IceAttack_1,
        IceAttack_2,
        IceAttack_3,
        IceAttack_4,
        ElectricAttack_1,
        ElectricAttack_2,
        ElectricAttack_3,
        ElectricAttack_4,
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
            case PlugInType.Gauntlet_Damage_2: return PlugInType.Gauntlet_Damage_1;
            case PlugInType.Gauntlet_Damage_3: return PlugInType.Gauntlet_Damage_1;
            case PlugInType.Gauntlet_Damage_4:
                return IsPlugInUnlocked(PlugInType.Gauntlet_Damage_2) ? PlugInType.Gauntlet_Damage_2 : PlugInType.Gauntlet_Damage_3;
            case PlugInType.Gauntlet_Range_2: return PlugInType.Gauntlet_Range_1;
            case PlugInType.Gauntlet_Range_3: return PlugInType.Gauntlet_Range_1;
            case PlugInType.Gauntlet_Range_4:
                return IsPlugInUnlocked(PlugInType.Gauntlet_Range_2) ? PlugInType.Gauntlet_Range_2 : PlugInType.Gauntlet_Range_3;
            case PlugInType.Gauntlet_Speed_2: return PlugInType.Gauntlet_Speed_1;
            case PlugInType.Gauntlet_Speed_3: return PlugInType.Gauntlet_Speed_1;
            case PlugInType.Gauntlet_Speed_4:
                return IsPlugInUnlocked(PlugInType.Gauntlet_Speed_2) ? PlugInType.Gauntlet_Speed_2 : PlugInType.Gauntlet_Speed_3;

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
