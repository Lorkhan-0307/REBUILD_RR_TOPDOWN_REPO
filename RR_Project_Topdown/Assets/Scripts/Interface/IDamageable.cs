using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage);

    IEnumerator KnockBack();
    IEnumerator Restraint(float time);
    IEnumerator DOTApply(float tickDamage, int type);

    void ApplyBurn(int ticks, float tickDamage);
    void ApplyIce();
    void ApplyCorrosion(int ticks, float tickDamage);

    void EnemyStun(float time);
    void EnemyKnockback();
}
