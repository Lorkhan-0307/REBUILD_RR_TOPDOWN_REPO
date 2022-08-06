using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCollision : MonoBehaviour
{
    private float meleeDamage;

    private void Awake()
    {
        meleeDamage = GetComponentInParent<MCAttack>().meleeDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy attackTarget = collision.GetComponent<Enemy>();
            if (attackTarget != null)
            {
                attackTarget.OnDamage(meleeDamage);
                
                if (!attackTarget.isKnockback && !attackTarget.isStun)
                {
                    attackTarget.EnemyKnockback();
                }
                
                
            }
        }
    }
}
