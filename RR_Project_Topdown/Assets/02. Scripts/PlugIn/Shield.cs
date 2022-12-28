using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float ShieldCoolTime = 3f;
    [SerializeField] private float ShieldDamage = 1f;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask whatIsTarget;

    private bool enableEntireAttack;

    // Start is called before the first frame update

    private void Start()
    {
        FindObjectOfType<StageManager>().UpgradeEntireAttack += Shield_UpgradeEntireAttack;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    private void Shield_UpgradeEntireAttack(object sender, System.EventArgs e)
    {
        enableEntireAttack = true;
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
                    attackTarget.GetComponent<Enemy>().EnemyKnockBack();
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

        if (enableEntireAttack) EntireAttack();

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Collider2D[] enemyColliders = GetComponents<Collider2D>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }

        //Wait for Shield Cool Time
        yield return new WaitForSeconds(ShieldCoolTime);

        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = true;
        }
    }

    private void EntireAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, attackRange, whatIsTarget);

        for(int i = 0; i < collider2Ds.Length; i++)
        {
            LivingEntity livingEntity = collider2Ds[i].GetComponent<LivingEntity>();

            if(livingEntity != null && !livingEntity.dead)
            {
                livingEntity.OnDamage(ShieldDamage);
            }
        }
    }


}
