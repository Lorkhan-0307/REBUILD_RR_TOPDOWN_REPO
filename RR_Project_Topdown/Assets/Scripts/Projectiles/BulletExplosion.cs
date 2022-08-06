using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    [SerializeField] private float bulletDamage = 20f;
    [SerializeField] private float stunTime = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy attackTarget = collision.GetComponent<Enemy>();
            if (attackTarget != null)
            {
                attackTarget.OnDamage(bulletDamage);
                if(!attackTarget.isStun && !attackTarget.isKnockback)
                {
                    attackTarget.EnemyStun(stunTime);
                }
            }
        }
    }
    

    private void EndExplosion()
    {
        Destroy(gameObject);
    }
}
