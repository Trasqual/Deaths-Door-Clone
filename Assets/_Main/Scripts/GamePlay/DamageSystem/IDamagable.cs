using UnityEngine;

public interface IDamagable
{
    public Transform GetTransform();
    public DamageDealerType GetEffectedByType();
    public void TakeDamage(int amount, DamageDealerType damageDealerType);
    public void Die();
}