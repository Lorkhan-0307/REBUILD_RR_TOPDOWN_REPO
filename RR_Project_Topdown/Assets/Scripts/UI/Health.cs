using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : LivingEntity
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 3f;
    [SerializeField] private float ShieldCoolTime = 3f;
    public GameObject ShieldEffect;
    Animate animate;
    PlayerMove playermove;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 2f;
    [SerializeField] private int numberOfFlashes = 3;
    private SpriteRenderer spriteRenderer;



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
    private void Awake()
    {
        animate = GetComponent<Animate>();
        playermove = GetComponent<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        currentHealth = startingHealth;
        ShieldEffect.SetActive(false);
    }

    public override void OnDamage(float damage)
    {
        /*if (ShieldEffect.activeSelf)
        {
            return;
        }*/

        if (!dead)
        {
            //play hurt animation
            animate.HurtAnimationActive();
            StartCoroutine(Invunerablility());

            //playerMove State Hurt

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

    private void OnTriggerEnter2D(Collider2D collision)
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

    private IEnumerator Invunerablility()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        //invunerability duration
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/(numberOfFlashes *2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

}
