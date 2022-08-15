using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCollision : MonoBehaviour
{
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy attackTarget = collision.GetComponent<Enemy>();
            if (attackTarget != null)
            {
                attackTarget.OnDamage(playerScriptableObject.meleeAttackDamage);
                
                if (!attackTarget.isKnockback && !attackTarget.isStun)
                {
                    attackTarget.EnemyKnockback();
                }
                
                
            }
        }
    }
}
