using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float currentHealth { get; protected set; }
    public bool dead { get; protected set; }
    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        dead = false;
    }

    public virtual void OnDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }

        dead = true;
    }



}
