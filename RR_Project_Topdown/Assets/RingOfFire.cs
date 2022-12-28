using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfFire : MonoBehaviour
{
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LivingEntity livingEntity = collision.GetComponent<LivingEntity>();
            livingEntity.OnDamage(playerScriptableObject.ringOfFireDamage);
            livingEntity.ApplyBurn(playerScriptableObject.burnTicks,playerScriptableObject.maxBurnTicks, playerScriptableObject.burnDamage);
        }
    }
}
