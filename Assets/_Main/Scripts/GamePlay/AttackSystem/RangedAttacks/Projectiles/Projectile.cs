using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] protected float maxTravelDistance = 15f;
        [SerializeField] protected float projectileSpeed = 5f;
        protected float _damage;
        protected DamageDealerType _damageDealerType;
        protected IDamageable _caster;

        public Collider Col => GetComponent<Collider>();

        public void Init(float damage, DamageDealerType damageDealerType, IDamageable caster)
        {
            SetDamage(damage);
            SetDamageDealerType(damageDealerType);
            SetCaster(caster);
        }

        public virtual void SetDamage(float damage)
        {
            _damage = damage;
        }

        public virtual void SetDamageDealerType(DamageDealerType damageDealerType)
        {
            _damageDealerType = damageDealerType;
        }

        public virtual void SetCaster(IDamageable caster)
        {
            _caster = caster;
        }
    }
}
