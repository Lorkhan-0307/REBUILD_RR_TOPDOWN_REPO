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
                PlayerMove player = this.GetComponentInParent<PlayerMove>();
                
                //속성공격
                switch(player.currentElement)
                {
                    case PlayerMove.Element.Physical:
                        break;
                    case PlayerMove.Element.Fire:
                        attackTarget.ApplyBurn(playerScriptableObject.burnTicks, playerScriptableObject.burnDamage);
                        break;
                    case PlayerMove.Element.Ice:
                        break;
                    case PlayerMove.Element.Corrosion:
                        break;

                }

                attackTarget.OnDamage(playerScriptableObject.meleeAttackDamage);
                
                if (!attackTarget.isKnockback && !attackTarget.isStun)
                {
                    attackTarget.EnemyKnockback();
                }
                
                
            }
        }
    }
}
