using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();
            attackTarget.isForceField = true;
        }
    }
}
