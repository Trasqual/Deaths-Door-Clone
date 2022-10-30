using UnityEngine;

public interface IDamageable
{
    public Transform GetTransform();
    public DamageDealerType GetEffectedByType();
    public void TakeDamage(int amount, DamageDealerType damageDealerType);
    public void Die();
}