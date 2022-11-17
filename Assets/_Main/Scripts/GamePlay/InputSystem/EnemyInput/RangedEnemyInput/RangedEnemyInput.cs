using _Main.Scripts.GamePlay.BehaviourSystem;
using _Main.Scripts.GamePlay.HealthSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.InputSystem
{
    public class RangedEnemyInput : EnemyInputBase
    {
        [SerializeField] private float aimCorrectionAssist = 14f;
        private RangedEnemyBehaviourData enemyBehaviourData;
        private bool isAiming;

        private MovementBase _targetMovement;

        protected override void Awake()
        {
            base.Awake();
            var data = GetComponent<EnemyBehaviourBase>().Data;
            enemyBehaviourData = (RangedEnemyBehaviourData)data;
        }

        public override Vector3 GetLookInput()
        {
            if (_target != null)
            {
                var targetsVelocity = _targetMovement ? _targetMovement.GetVelocity() : Vector3.zero;
                return _target.GetTransform().position + aimCorrectionAssist * Time.deltaTime * targetsVelocity;
            }
            else
            {
                return startPos;
            }
        }

        public override Vector3 GetMovementInput()
        {
            if (_target != null)
            {
                if (Vector3.Distance(transform.position, _target.GetTransform().position) > attackController.SelectedRangedAttack.CurrentComboDamageData.attackRange)
                {
                    return _target.GetTransform().position;
                }
                else
                {
                    return Vector3.zero;
                }
            }
            else
            {
                var returnPos = shouldPatrol ? GetPatrolPosition() : startPos;
                if (Vector3.Distance(transform.position, returnPos) > agent.stoppingDistance)
                {
                    return returnPos;
                }
                else
                {
                    return Vector3.zero;
                }
            }
        }

        protected override void Update()
        {
            if (_target != null)
            {
                if (Vector3.Distance(transform.position, _target.GetTransform().position) <= attackController.SelectedRangedAttack.CurrentComboDamageData.attackRange && !isAiming)
                {
                    isAiming = true;
                    OnAimActionStarted?.Invoke();
                    DOVirtual.DelayedCall(enemyBehaviourData.CastSpeed, () =>
                    {
                        OnAimActionEnded?.Invoke();
                        isAiming = false;
                    });
                }
            }
        }

        protected override void OnTargetDetectedCallback(IDamageable target)
        {
            _target = target;
            if (_target.GetTransform().TryGetComponent(out MovementBase movement))
            {
                _targetMovement = movement;
            }
        }

        protected override void OnTargetLostCallback()
        {
            _target = null;
            _targetMovement = null;
        }
    }
}