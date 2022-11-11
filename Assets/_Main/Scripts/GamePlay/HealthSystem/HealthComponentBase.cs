using _Main.Scripts.Utilities;
using System;
using UnityEngine;

public abstract class HealthComponentBase: MonoBehaviour, IDamageable
{
    [SerializeField] public DamageDealerType effectedByType;
    [SerializeField] protected int maxHealth = 4;

    protected int CurrentHealth = 4;

    private bool isDead;

    public int MaxHealth => maxHealth;

    #region Damageable Feature

    public Action<int> OnDamageTaken;
    public Action OnDeath { get; set; }

    public Transform GetTransform() => transform;

    public DamageDealerType GetEffectedByType() => effectedByType;

    public virtual bool TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (!Enums.CompareEnums(damageDealerType, effectedByType)) return false;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }

        OnDamageTaken?.Invoke(CurrentHealth);

        return true;
    }
    public virtual void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
    }

    public bool IsDead() => isDead;

    #endregion
}