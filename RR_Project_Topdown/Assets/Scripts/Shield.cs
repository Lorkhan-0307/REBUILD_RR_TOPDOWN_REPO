using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float ShieldCoolTime = 3f;
    [SerializeField] private float ShieldDamage = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Separate case Enemy & Projectile?
        //sheild effect
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Bullet"))
        {
            if (gameObject.activeSelf)
            {
                LivingEntity attackTarget = collision.GetComponent<LivingEntity>();
                if (attackTarget != null)
                {
                    attackTarget.OnDamage(ShieldDamage);
                    //attackTarget.EnemyKnockback();
                }

                StartCoroutine(ShieldCycle());
            }
        }

    }

    private IEnumerator ShieldCycle()
    {
        //play shield off animation
        //play Enemy knockback animation

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Collider2D[] enemyColliders = GetComponents<Collider2D>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        //Wait for Shield Cool Time
        yield return new WaitForSeconds(ShieldCoolTime);

        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        Collider2D[] enemyColliders_2 = GetComponents<Collider2D>();
        for (int i = 0; i < enemyColliders_2.Length; i++)
        {
            enemyColliders[i].enabled = true;
        }
    }


}
