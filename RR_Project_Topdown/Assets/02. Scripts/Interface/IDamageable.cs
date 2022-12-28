using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage);

 
    IEnumerator DOTApply(float tickDamage, int type);

    void ApplyBurn(int ticks, int maxTicks, float tickDamage);
    void ApplyIce(float slowDownSpeed, bool enabledSecondUpgrade, bool enabledThirdUpgrade);
    void ApplyCorrosion(int ticks, int maxTicks, float tickDamage, bool fourthUpgrade);
}
