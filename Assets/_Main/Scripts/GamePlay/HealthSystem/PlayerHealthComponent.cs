using _Main.Scripts.GamePlay.BehaviourSystem;
using _Main.Scripts.GamePlay.UI;
using _Main.Scripts.Utilities;
using System;
using UnityEngine;

namespace _Main.Scripts.GamePlay.HealthSystem
{
    public class PlayerHealthComponent : HealthComponentBase, IVisualizable
    {
        [SerializeField] VisualizableBarHandler visualizerPrefab;
        [SerializeField] private Vector3 visualizerPosition;

        private InvulnerableBase _invulnerable = null;
        private PlayerBehaviourData _behaviourData = null;

        private void Awake()
        {
            _invulnerable = gameObject.AddComponent<Invulnerable>();
        }

        public void Init(PlayerBehaviourData baseData)
        {
            _behaviourData = baseData;
            var visualizer = Instantiate(visualizerPrefab, transform.TransformPoint(visualizerPosition), transform.rotation,transform);
            visualizer.Initialize(this);
        }

        public override bool TakeDamage(int amount, DamageDealerType damageDealerType)
        {
            if (_invulnerable.IsActive) return false;

            _invulnerable.InvulnerableForDuration(_behaviourData.InvulnerabilityDurationAfterTakingDamage);

            if (!Enums.CompareEnums(damageDealerType, effectedByType)) return false;

            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }

            OnDamageTaken?.Invoke(CurrentHealth);
            OnValueChanged?.Invoke(CurrentHealth);

            return true;
        }

        public override void Die()
        {
            base.Die();
            _invulnerable.SetInvulnerable();
            OnClose?.Invoke();
        }

        #region IVisualizable
        public Action<int> OnValueChanged { get; set; }
        public Action OnMaxValueChanged { get; set; }
        public Action OnClose { get; set; }

        public int GetMaxValue() => maxHealth;
        #endregion
    }
}