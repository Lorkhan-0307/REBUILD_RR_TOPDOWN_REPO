using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExplosion : MonoBehaviour
{
    [SerializeField] private float stunTime = 5f;
    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    private PlayerMove playerMove;

    private void Awake()
    {
        playerMove = PlayerMove.FindObjectOfType<PlayerMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.CompareTag("Enemy"))
        {
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();
            if (attackTarget != null)
            {
                attackTarget.OnDamage(bulletDamage);
                if(!attackTarget.isStun && !attackTarget.isKnockback)
                {
                    attackTarget.EnemyStun(stunTime);
                }
            }
        }*/

        if(collision.gameObject.CompareTag("Enemy"))
        {
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();

            if (attackTarget != null)
            {
                attackTarget.OnDamage(playerScriptableObject.rangeAttackDamage);
                if(playerMove.currentElement == PlayerMove.Element.Corrosion && playerScriptableObject.enabledThirdUpgrade == true)
                {
                    attackTarget.ApplyCorrosion(playerScriptableObject.corrosionTicks,playerScriptableObject.maxBurnTicks, playerScriptableObject.corrosionDamage, playerScriptableObject.enabledFourthUpgrade);
                    attackTarget.ApplyCorrosion(playerScriptableObject.corrosionTicks,playerScriptableObject.maxBurnTicks, playerScriptableObject.corrosionDamage, playerScriptableObject.enabledFourthUpgrade);
                    attackTarget.ApplyCorrosion(playerScriptableObject.corrosionTicks,playerScriptableObject.maxBurnTicks, playerScriptableObject.corrosionDamage, playerScriptableObject.enabledFourthUpgrade);
                }
                if (!attackTarget.isStun && !attackTarget.isKnockback)
                {
                    if(attackTarget.TryGetComponent(out Enemy enemy))
                        enemy.EnemyRestraint(stunTime);
                }
                
            }
        }

    }
    

    private void EndExplosion()
    {
        Destroy(gameObject);
    }
}
