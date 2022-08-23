using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagerBase : MonoBehaviour, IDamagable
{
    public Action<float> OnDamageTaken;
    public Action OnDeath;
    [SerializeField] private DamageDealerType _effectedByType;
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth = 100f;

    public void TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (damageDealerType != _effectedByType) return;

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
        OnDeath?.Invoke();
    }
}
