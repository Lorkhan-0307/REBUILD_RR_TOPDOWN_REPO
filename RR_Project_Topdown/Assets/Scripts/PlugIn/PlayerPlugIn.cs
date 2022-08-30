using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        GauntletAttack_1, // 단순 데미지 증가 15%
        GauntletAttack_2, // 데미지 30% 증가, 공격속도 10% 감소
        GauntletAttack_3, // 공격속도 15% 증가, 데미지 25% 감소
        GauntletAttack_4, // 공격력 50% 증가, 범위증가 OR 공격속도 10% 증가[고민중]
        Utility_1, // 체력 강화
        Utility_2, // 실드 생성
        Utility_3, // 이동속도 증가
        Utility_4, // 체력 강화 및 피해시 화면 전체의 적에게 적은 데미지

        /*소환수의 경우 
         * 1. 소환수 생성
         * 2. 소환수 Melee 공격 강화
         * 3. 소환수 Debuff 강화
         * 4. 소환수 Damage&속성공격 플레이어 강화 상태에 맞춰서 강화
         */
        //SummonAttack 4까지 생성
        SummonAttack_1,
        SummonAttack_2,
        SummonAttack_3,
        SummonAttack_4,

        FireAttack_1,
        FireAttack_2,
        FireAttack_3,
        FireAttack_4,
        IceAttack_1,
        IceAttack_2,
        IceAttack_3,
        IceAttack_4,

        //전기 공격을 부식으로 교체한 관계로 부식[Corrosion]으로 명칭 변경
        CorrosionAttack_1,
        CorrosionAttack_2,
        CorrosionAttack_3,
        CorrosionAttack_4,
        ElectricAttack_1,
        ElectricAttack_2,
        ElectricAttack_3,
        ElectricAttack_4,

    }

    private List<PlugInType> unlockedPlugInTypeList;
    //private int plugInNum = 29;

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
            case PlugInType.GauntletAttack_2: return PlugInType.GauntletAttack_1;
            case PlugInType.GauntletAttack_3: return PlugInType.GauntletAttack_1;
            case PlugInType.GauntletAttack_4:
                return IsPlugInUnlocked(PlugInType.GauntletAttack_2) ? PlugInType.GauntletAttack_2 : PlugInType.GauntletAttack_3;

            case PlugInType.Utility_2: return PlugInType.Utility_1;
            case PlugInType.Utility_3: return PlugInType.Utility_2;
            case PlugInType.Utility_4: return PlugInType.Utility_3;

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
