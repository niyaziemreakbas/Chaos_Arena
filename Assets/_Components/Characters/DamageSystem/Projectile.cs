using UnityEngine;

public class Projectile : MonoBehaviour, IDamager
{
    private int damage;
    private Character charOwner;
    private float lifeTime = 1.5f;

    public void Initialize(Character charOwner, int damage)
    {
        this.charOwner = charOwner;
        this.damage = damage;

        Destroy(gameObject, lifeTime);
    }

    public void DealDamage(IDamageable target, int dmg)
    {
        target?.TakeDamage(new DamageInfo { amount = dmg });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<Character>();
        if (character != null && character.TeamOwner != charOwner.TeamOwner)
        {
            DealDamage(character, damage);
            Destroy(gameObject);
        }
    }
}
