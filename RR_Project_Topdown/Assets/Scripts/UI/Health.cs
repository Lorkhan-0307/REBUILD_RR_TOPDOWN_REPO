using System.Collections;
using UnityEngine;

public class Health : LivingEntity
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float ShieldCoolTime = 3f;
    [SerializeField] public GameObject ShieldEffect;

    /*public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //hurt animation
        }

        else
        {
            //dead animation
        }
    }*/

    protected override void OnEnable()
    {
        base.OnEnable();

        currentHealth = startingHealth;
        ShieldEffect.SetActive(false);
    }

    public override void OnDamage(float damage)
    {
        if (ShieldEffect.activeSelf)
        {
            return;
        }

        if (!dead)
        {
            //play hurt animation
            //play hurt sound
        }

        base.OnDamage(damage);
    }

    public override void Die()
    {
        base.Die();

        //play die sound
        //play die animation
        //Disable Player
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Separate case Enemy & Projectile?
        //sheild effect
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Projectile"))
        {
            if (ShieldEffect.activeSelf)
            {
                StartCoroutine(ShieldCycle());
            }
        }

    }

    //Test Later when Enemy Script is completed
    private IEnumerator ShieldCycle()
    {
        //play shield off animation
        //play player knockback animation
        ShieldEffect.SetActive(false);

        yield return new WaitForSeconds(ShieldCoolTime);

        ShieldEffect.SetActive(true);
    }


}
