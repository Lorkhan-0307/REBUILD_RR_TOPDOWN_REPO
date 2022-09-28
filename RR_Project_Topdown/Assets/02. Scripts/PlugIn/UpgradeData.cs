using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlugInType
{
    GauntletAttack_1,
    GauntletAttack_2,
    GauntletAttack_3,
    GauntletAttack_4,
    CorrosionAttack_1,
    CorrosionAttack_2,
    CorrosionAttack_3,
    CorrosionAttack_4,
    FireAttack_1,
    FireAttack_2,
    FireAttack_3,
    FireAttack_4,
    IceAttack_1,
    IceAttack_2,
    IceAttack_3,
    IceAttack_4,
    Utility_1,
    Utility_2,
    Utility_3,
    Utility_4,
    SummonAttack_1,
    SummonAttack_2,
    SummonAttack_3,
    SummonAttack_4,

}

[CreateAssetMenu]
public class UpgradeData : ScriptableObject
{
    public PlugInType plugInType;
    public string Name;
    public Sprite icon;
    public List<UpgradeData> upgrades;
    public int index;
    public int imageIndex;
}
