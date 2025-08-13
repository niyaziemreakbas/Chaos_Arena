using UnityEngine;

public class MeleeWeapon : CharAttackController, IDamager
{
    [SerializeField] private DamageInfo damageInfo;

    public void DealDamage(IDamageable target, int damage)
    {
        damageInfo.amount = damage;
        target?.TakeDamage(damageInfo);
    }

    protected override void PerformAttack(GameObject target)
    {
        DealDamage(target.GetComponent<IDamageable>(), owner.CharacterData.damage);
    }
}
