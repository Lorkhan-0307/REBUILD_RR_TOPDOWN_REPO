using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Vector3 shootDir;
    [SerializeField] private float fireSpeed = 20f;
    [SerializeField] PlayerScriptableObject playerScriptableObject;

    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir;
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.position += shootDir * fireSpeed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LivingEntity livingEntity = collision.GetComponent<LivingEntity>();
            livingEntity.ApplyBurn(playerScriptableObject.burnTicks,playerScriptableObject.maxBurnTicks, playerScriptableObject.burnDamage);
        }
    }
}
