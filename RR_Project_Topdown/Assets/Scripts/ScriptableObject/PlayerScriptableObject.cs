using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerVariable", menuName = "ScriptableObject/PlayerVariable")]
public class PlayerScriptableObject : ScriptableObject
{
    public float movementSpeed = 7f;
    public float movementSpeedWhileAttack = 4f;
    public float meleeAttackDamage = 5f;
    public float skillActiveMeleeAttackDamage = 8f;
    public float enableRangeAttackTime = 3f;
    public float skillBarMax = 150f;
    public float maxBulletCount = 3f;
    public float reloadBulletTime = 10f;
    public AudioClip meleeAttackAudio;

    //속성공격

    public int burnTicks = 5;
    public float burnDamage = 3f;


}
