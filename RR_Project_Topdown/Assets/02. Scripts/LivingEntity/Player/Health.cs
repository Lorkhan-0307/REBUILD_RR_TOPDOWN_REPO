using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : LivingEntity
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 3f;
    [SerializeField] private HealthBar healthBar;
    //[SerializeField] private float ShieldCoolTime = 3f;
    //public GameObject ShieldEffect;
    Animate animate;
    PlayerMove playermove;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 2f;
    [SerializeField] private int numberOfFlashes = 3;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animate = GetComponent<Animate>();
        playermove = GetComponent<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        FindObjectOfType<StageManager>().UpgradeHealth += UpgradeHealth;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        currentHealth = startingHealth;
        //ShieldEffect.SetActive(false);
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

            //play hurt sound
        }

        base.OnDamage(damage);
    }

    public override void Die()
    {
        base.Die();

        //play die sound
        FindObjectOfType<AudioManager>().Play("PlayerDie", 0);
        FindObjectOfType<AudioManager>().Stop("Pre-MainTheme", 1);
        //play die animation
        //Disable Player
        gameObject.SetActive(false);
    }

    public void UpgradeHealth(object sender, EventArgs e)
    {
        //Upgrade Health 수치
        //Upgrade Health Bar UI
        healthBar.UpgradeHealthBar();
        startingHealth = 5f;//수치 조정 필요
        currentHealth = startingHealth;

        FindObjectOfType<StageManager>().UpgradeHealth -= UpgradeHealth;
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
