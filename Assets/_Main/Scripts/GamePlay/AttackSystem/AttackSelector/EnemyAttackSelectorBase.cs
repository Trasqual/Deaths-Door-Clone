using _Main.Scripts.GamePlay.DetectionSystem;
using _Main.Scripts.GamePlay.HealthSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class EnemyAttackSelectorBase : AttackSelectorBase
    {
        protected EnemyTriggerDetector _detector;

        protected override void Awake()
        {
            base.Awake();
            _detector = GetComponentInChildren<EnemyTriggerDetector>();
        }

        private void Update()
        {
            if (_targetTransform != null)
            {
                SelectAttack();
            }
        }

        protected override void SelectAttack()
        {
        }

        protected override bool IsAttackAvailable(AttackBase attack)
        {
            return TargetIsInAttackRange(attack) && !attack.IsOnCooldown;
        }

        protected override bool TargetIsInAttackRange(AttackBase attack)
        {
            return Vector3.Distance(transform.position, _targetTransform.position) <= attack.CurrentComboDamageData.attackRange;
        }

        private void OnTargetFoundCallback(IDamageable target)
        {
            _targetTransform = target.GetTransform();
        }

        private void OnTargetLostCallback()
        {
            _targetTransform = null;
        }

        protected virtual void OnEnable()
        {
            _detector.OnTargetFound += OnTargetFoundCallback;
            _detector.OnTargetLost += OnTargetLostCallback;
        }

        protected virtual void OnDisable()
        {
            _detector.OnTargetFound -= OnTargetFoundCallback;
            _detector.OnTargetLost -= OnTargetLostCallback;
        }
    }
}

