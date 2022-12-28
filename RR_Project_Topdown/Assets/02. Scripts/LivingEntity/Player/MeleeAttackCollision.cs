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
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();
            if (attackTarget != null)
            {
                PlayerMove player = this.GetComponentInParent<PlayerMove>();
                
                //속성공격
                switch(player.currentElement)
                {
                    case PlayerMove.Element.Physical:
                        break;
                    case PlayerMove.Element.Fire:
                        attackTarget.ApplyBurn(playerScriptableObject.burnTicks,playerScriptableObject.maxBurnTicks, playerScriptableObject.burnDamage);
                        break;
                    case PlayerMove.Element.Ice:
                        attackTarget.ApplyIce(playerScriptableObject.slowDownSpeed,playerScriptableObject.enabledSecondUpgrade, playerScriptableObject.enabledThirdUpgrade);
                        break;
                    case PlayerMove.Element.Corrosion:
                        attackTarget.ApplyCorrosion(playerScriptableObject.corrosionTicks,playerScriptableObject.maxCorrosionTicks, playerScriptableObject.corrosionDamage, playerScriptableObject.enabledFourthUpgrade);
                        break;

                }

                if (player.skillActivated)
                    attackTarget.OnDamage(playerScriptableObject.skillActiveMeleeAttackDamage);
                else
                    attackTarget.OnDamage(playerScriptableObject.meleeAttackDamage);
                
                if(attackTarget.GetComponent<Enemy>())
                {
                    if (!attackTarget.isKnockback && !attackTarget.isStun)
                    {
                        attackTarget.GetComponent<Enemy>().EnemyKnockBack();
                    }
                }
                
                
                
            }
        }
    }
}
