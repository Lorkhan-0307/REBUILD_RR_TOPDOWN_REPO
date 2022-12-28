using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetFX : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();
        
            if(attackTarget!=null && !attackTarget.dead)
            {
                attackTarget.OnDamage(gameObject.GetComponentInParent<Pet>().attackDamage);
            }
        }
    }

    public void AnimationTrigger()
    {
        gameObject.SetActive(false);
    }


}
