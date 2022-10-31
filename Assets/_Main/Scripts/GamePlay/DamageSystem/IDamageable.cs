using System;
using UnityEngine;

public interface IDamageable
{
    public Action OnDeath { get; set; }
    public Transform GetTransform();
    public DamageDealerType GetEffectedByType();
    public void TakeDamage(int amount, DamageDealerType damageDealerType);
    public void Die();
    public bool IsDead();
}