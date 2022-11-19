using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected float _maxTravelDistance = 15f;
        [SerializeField] protected float _lifeTime = 3f;
        protected float _damage;
        protected DamageDealerType _damageDealerType;
        protected IDamageable _caster;

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

        public virtual void KillProjectile()
        {

        }
    }
}
