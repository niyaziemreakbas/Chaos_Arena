using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamager
{
    void DealDamage(IDamageable target, int damage);
}

public enum DamageType
{
    Physical,
    Fire,
    Ice,
}