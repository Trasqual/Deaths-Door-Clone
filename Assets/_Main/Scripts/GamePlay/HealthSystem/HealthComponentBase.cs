using _Main.Scripts.Utilities;
using System;
using UnityEngine;

public abstract class HealthComponentBase : MonoBehaviour, IDamagable
{
    [SerializeField] public DamageDealerType effectedByType;
    [SerializeField] protected int maxHealth = 4;

    protected int CurrentHealth = 4;

    public bool IsDead { protected set; get; } = false;

    public int MaxHealth => maxHealth;
    
    #region Damageable Feature
    
    public Action<int> OnDamageTaken;
    public Action OnDeath;
    
    public virtual void TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (!Enums.CompareEnums(damageDealerType, effectedByType)) return;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }

        OnDamageTaken?.Invoke(CurrentHealth);
    }
    public virtual void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    #endregion
}