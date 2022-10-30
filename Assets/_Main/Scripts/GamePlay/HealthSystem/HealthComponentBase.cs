using _Main.Scripts.Utilities;
using System;
using UnityEngine;

public abstract class HealthComponentBase : MonoBehaviour, IDamageable
{
    [SerializeField] public DamageDealerType EffectedByType;
    [SerializeField] protected int _maxHealth = 4;

    protected int _currentHealth = 4;

    public bool IsDead { protected set; get; } = false;

    public int MaxHealth => _maxHealth;

    #region Damageable Feature

    public Action<int> OnDamageTaken;
    public Action OnDeath;

    public Transform GetTransform() => transform;

    public DamageDealerType GetEffectedByType() => EffectedByType;


    public virtual void TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (!Enums.CompareEnums(damageDealerType, EffectedByType)) return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

        OnDamageTaken?.Invoke(_currentHealth);
    }
    public virtual void Die()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }

    #endregion
}