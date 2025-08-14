using UnityEngine;

public class RangedWeapon : CharAttackController
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 5f;

    protected override void PerformAttack(GameObject target)
    {
        if (projectilePrefab == null || firePoint == null) return;

        Vector2 dir = (target.transform.position - firePoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        proj.GetComponent<Projectile>().Initialize(owner, owner.CharacterData.damage);

        var projRb = proj.GetComponent<Rigidbody2D>();
        if (projRb != null)
            projRb.velocity = dir * projectileSpeed;

        //var damager = proj.GetComponent<IDamager>();
        //if (damager != null)
        //    damager.DealDamage(target.GetComponent<IDamageable>(), owner.CharacterData.damage);
    }
}
