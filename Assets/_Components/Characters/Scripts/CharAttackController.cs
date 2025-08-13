using UnityEngine;

public abstract class CharAttackController : MonoBehaviour
{
    protected Character owner;
    protected float attackCooldown;
    protected float lastAttackTime;

    public virtual void Initialize(Character owner, float cooldown)
    {
        this.owner = owner;
        this.attackCooldown = cooldown;
        lastAttackTime = -cooldown; // baþta hemen saldýrabilsin
    }

    public bool IsReadyToAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    public void TryAttack(GameObject target)
    {
        if (target == null) return;
        if (!IsReadyToAttack()) return;

        PerformAttack(target);
        lastAttackTime = Time.time;
    }

    protected abstract void PerformAttack(GameObject target);
}
