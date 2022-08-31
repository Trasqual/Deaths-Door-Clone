using _Main.Scripts.Utilities;
using System;
using System.Collections;
using UnityEngine;

public class HealthManagerBase : MonoBehaviour, IDamagable
{
    public Action<int> OnDamageTaken;
    public Action OnDeath;
    [SerializeField] public DamageDealerType _effectedByType;
    [SerializeField] protected int _maxHealth = 4;
    [SerializeField] float _invulnerablityTimeAfterDamage = 0.5f;

    bool _isInvulnerable = false;

    protected int _currentHealth = 4;

    protected bool _isDead;

    public int MaxHealth => _maxHealth;

    public virtual void TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (_isInvulnerable) return;
        StartCoroutine(SetInvulnerableForDuration(_invulnerablityTimeAfterDamage));
        if (!Enums.CompareEnums(damageDealerType, _effectedByType)) return;

        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }

        OnDamageTaken?.Invoke(_currentHealth);
    }

    private void Die()
    {
        SetInvulnerable();
        OnDeath?.Invoke();
    }

    public void SetInvulnerable()
    {
        _isInvulnerable = true;
    }

    public void SetVulnerable()
    {
        _isInvulnerable = false;
    }

    public IEnumerator SetInvulnerableForDuration(float invulTime)
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(invulTime);
        _isInvulnerable = false;
    }
}
