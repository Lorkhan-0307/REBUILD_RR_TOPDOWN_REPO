using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrisionPond : MonoBehaviour
{
    [SerializeField] PlayerScriptableObject playerScriptableObject;
    private float time;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LivingEntity livingEntity = collision.GetComponent<LivingEntity>();
            livingEntity.ApplyCorrosion(playerScriptableObject.corrosionTicks,playerScriptableObject.maxCorrosionTicks, playerScriptableObject.corrosionDamage, playerScriptableObject.enabledFourthUpgrade);
            livingEntity.ApplyCorrosion(playerScriptableObject.corrosionTicks,playerScriptableObject.maxCorrosionTicks, playerScriptableObject.corrosionDamage, playerScriptableObject.enabledFourthUpgrade);
        }
    }


    private void Start()
    {
        Destroy(gameObject, 4f);
    }
}
