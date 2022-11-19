using System;
using UnityEngine;

namespace _Main.Scripts.GamePlay.HealthSystem
{
    public interface IDamageable
    {
        public Action<float> OnDamageTaken { get; set; }
        public Action OnDeath { get; set; }
        public Transform GetTransform();
        public DamageDealerType GetEffectedByType();
        public bool TakeDamage(float amount, DamageDealerType damageDealerType);
        public void Die();
        public bool IsDead();
    }
}