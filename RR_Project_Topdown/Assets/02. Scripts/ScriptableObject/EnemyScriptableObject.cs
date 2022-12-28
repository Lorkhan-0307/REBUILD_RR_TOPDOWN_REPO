using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyVariable", menuName = "ScriptableObject/EnemyVariable")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Enemy Health")]
    public float enemyHealth = 5f;

    [Header("Enemy Speed")]
    public float enemySpeedMax = 3f;
    public float enemySpeedMin = 1f;

    public float fKnockbackMultiplier = 30;
    public float fKnockbackDuration = 0.2f;
    public GameObject iceLock;
    public GameObject fireBurst;
    public GameObject corrosion;
    public Transform corrosionExplosion;
}
