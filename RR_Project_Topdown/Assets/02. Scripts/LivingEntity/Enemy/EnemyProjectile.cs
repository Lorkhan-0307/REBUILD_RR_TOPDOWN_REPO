using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Transform player;
    private Vector3 shootDir;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        shootDir = (player.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Bullet"))
        {
            transform.position += shootDir * speed * Time.deltaTime;
        }
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Hit!");
            LivingEntity attackTarget = collision.GetComponent<LivingEntity>();
            attackTarget.OnDamage(1);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }


}
