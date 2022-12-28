using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariable
{
    public float movespeed; //
    public float constant_movementSpeed = 7f;
    public float movementSpeedMultiplier = 5f;
    public float movementSpeedWhileAttack = 4f;
    public float meleeAttackDamage = 17f;
    public float skillActiveMeleeAttackDamage = 30f;
    public float enableRangeAttackTime = 3f;
    public float skillBarMax = 150f;
    public float maxBulletCount = 3f;
    public float reloadBulletTime = 10f;
    public float rangeAttackDamage = 20f;

    //속성공격

    public int burnTicks = 3;
    public int maxBurnTicks = 3;
    public float burnDamage = 7f;


    public float ringOfFireDamage = 5f;


    public int corrosionTicks = 10;
    public int maxCorrosionTicks = 10;
    public float corrosionDamage = 1f;
    public float corrisionExplosionDamage = 10f;

    public float slowDownSpeed = 0.85f;
    public bool enabledSecondUpgrade = false;
    public bool enabledThirdUpgrade = false;
    public bool enabledFourthUpgrade = false;

    [Header("Gauntlet Upgrade")]
    public float upgradeDamage_Gauntlet1 = 1.15f;
    public float upgradeDamage_Gauntlet2 = 1.3f;
    public float upgradeSpeed_Gauntlet2 = 0.9f;
    public float upgradeDamage_Gauntlet3 = 0.75f;
    public float upgradeSpeed_Gauntlet3 = 1.15f;
    public float upgradeDamage_Gauntlet4 = 1.5f;

    [Header("Summon Attack Upgrade")]
    public float latestGauntletDamageRate = 1f;
    public bool isPetUpgraded = false;
    public float petCoolTimeMultiplier = 0.5f;

}
