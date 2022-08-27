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
        if (!CompareEnums(damageDealerType, _effectedByType)) return;

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

    private bool CompareEnums(DamageDealerType effector, DamageDealerType effected)
    {
        int commonBitmask = (int)effector & (int)effected;

        foreach (DamageDealerType currentEnum in Enum.GetValues(typeof(DamageDealerType)))
        {
            if ((commonBitmask & (int)currentEnum) != 0)
            {
                return true;
            }
        }

        return false;
    }
}
