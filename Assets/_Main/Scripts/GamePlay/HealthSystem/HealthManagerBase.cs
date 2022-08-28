using _Main.Scripts.Utilities;
using System;
using UnityEngine;

public class HealthManagerBase : MonoBehaviour, IDamagable
{
    public Action<float> OnDamageTaken;
    public Action OnDeath;
    [SerializeField] public DamageDealerType _effectedByType;
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth = 100f;

    private bool _isDead;

    public void TakeDamage(float amount, DamageDealerType damageDealerType)
    {
        if (_isDead) return;
        if (!Enums.CompareEnums(damageDealerType, _effectedByType)) return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

        OnDamageTaken?.Invoke(_currentHealth / _maxHealth);
    }

    private void Die()
    {
        _isDead = true;
        OnDeath?.Invoke();
    }
}
