using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrisionExplosion : MonoBehaviour
{
    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    [SerializeField] private Transform corrosionPond;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();

            if (attackTarget != null)
            {
                attackTarget.OnDamage(playerScriptableObject.corrisionExplosionDamage);
            }
        }

    }


    private void EndExplosion()
    {
        Transform _CorrosionPond = Instantiate(corrosionPond);
        _CorrosionPond.transform.position = this.transform.position;
        Destroy(gameObject);
    }
}
