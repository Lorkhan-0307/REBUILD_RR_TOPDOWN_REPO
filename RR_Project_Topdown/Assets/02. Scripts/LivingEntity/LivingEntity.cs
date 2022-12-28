using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float currentHealth { get; protected set; }
    public List<int> dotTickTimers { get; protected set; }
    public bool dead { get; protected set; }
    public event Action OnDeath;
    public bool isForceField = false;

    [HideInInspector]
    public bool isKnockback = false;
    [HideInInspector]
    public bool isStun = false;

    public float enemySpeed;
    public float normalEnemySpeed;

    protected virtual void OnEnable()
    {
        dead = false;
    }

    public virtual void OnDamage(float damage)
    {
        if (isForceField)
        {
            damage *= 1.5f;
        }
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

    public virtual IEnumerator DOTApply(float tickDamage, int type)
    {
        while (dotTickTimers.Count > 0)
        {
            for (int i = 0; i < dotTickTimers.Count; i++)
            {
                dotTickTimers[i]--;

                if (type == 2)
                {
                    OnDamage(tickDamage);
                }
            }
            if (type == 0)
            {
                OnDamage(tickDamage);

            }

            dotTickTimers.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
        if (dotTickTimers.Count <= 0)
        {
            

            for (var i = this.transform.childCount - 1; i >= 0; i--)
            {
                
                if(this.transform.GetChild(i).CompareTag("fx"))
                {
                    if(type == 1)
                    {
                        enemySpeed = normalEnemySpeed;
                    }
                    Destroy(this.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    public virtual void ApplyBurn(int ticks, int maxTicks, float tickDamage)
    {
    }

    public virtual void ApplyIce(float slowDownSpeed, bool enabledSecondUpgrade, bool enabledThirdUpgrade)
    {
    }

    public virtual void ApplyCorrosion(int ticks, int maxTicks, float tickDamage, bool fourthUpgrade)
    {
    }

}
