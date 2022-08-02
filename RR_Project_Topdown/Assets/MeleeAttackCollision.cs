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
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();
            if (attackTarget != null)
            {
                attackTarget.OnDamage(meleeDamage);
                
            }
        }
    }
}
