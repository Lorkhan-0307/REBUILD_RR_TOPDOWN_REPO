using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    private Vector3 shootDir;
    [SerializeField] private float fireSpeed = 20f;
    [SerializeField] private Transform pfBulletExplosion;

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
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            Instantiate(pfBulletExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
