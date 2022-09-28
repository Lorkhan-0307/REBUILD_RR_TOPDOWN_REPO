using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyVariable", menuName = "ScriptableObject/EnemyVariable")]
public class EnemyScriptableObject : ScriptableObject
{
    public float fKnockbackMultiplier = 30;
    public float fKnockbackDuration = 0.2f;
    public GameObject iceLock;
    public GameObject fireBurst;
    public GameObject corrosion;
}
