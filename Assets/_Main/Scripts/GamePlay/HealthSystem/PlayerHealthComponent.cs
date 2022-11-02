using _Main.Scripts.Utilities;

namespace _Main.Scripts.GamePlay.HealthSystem
{
    public class PlayerHealthComponent : HealthComponentBase
    {
        private InvulnerableBase _invulnerable = null;
        private BehaviourData _behaviourData = null;

        private void Awake()
        {
            _invulnerable = gameObject.AddComponent<Invulnerable>();
        }

        public void Init(BehaviourData data)
        {
            _behaviourData = data;
        }

        public override void TakeDamage(int amount, DamageDealerType damageDealerType)
        {
            if (_invulnerable.IsActive) return;
        
            _invulnerable.InvulnerableForDuration(_behaviourData.InvulnerabilityDurationAfterTakingDamage);
        
            if (!Enums.CompareEnums(damageDealerType, effectedByType)) return;

            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }

            OnDamageTaken?.Invoke(CurrentHealth);
        }

        public override void Die()
        {
            base.Die();
            _invulnerable.SetInvulnerable();
        }
    }
}