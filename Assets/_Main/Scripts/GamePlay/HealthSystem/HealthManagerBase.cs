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

    IEnumerator invulnerabilityCo;

    public virtual void TakeDamage(int amount, DamageDealerType damageDealerType)
    {
        if (_isInvulnerable) return;
        StopInvulnerabilityCo();
        invulnerabilityCo = SetInvulnerableForDuration(_invulnerablityTimeAfterDamage);
        StartCoroutine(invulnerabilityCo);
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
        StopInvulnerabilityCo();
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

    private void StopInvulnerabilityCo()
    {
        if (invulnerabilityCo != null)
        {
            StopCoroutine(invulnerabilityCo);
        }
    }
}
