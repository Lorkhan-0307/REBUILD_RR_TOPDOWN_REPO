using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerVariable", menuName = "ScriptableObject/PlayerVariable")]
public class PlayerScriptableObject : ScriptableObject
{
    public float movementSpeed = 7f;
    public float movementSpeedWhileAttack = 4f;
    public float meleeAttackDamage = 5f;
    public float enableRangeAttackTime = 3f;
    public float skillBarMax = 150f;
    public float maxBulletCount = 3f;
    public float reloadBulletTime = 10f;
}
