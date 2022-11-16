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
                return (_target.GetTransform().position + targetsVelocity * Time.deltaTime * aimCorrectionAssist - transform.position).normalized;
            }
            else
            {
                return (startPos - transform.position).normalized;
            }
        }

        public override Vector3 GetMovementInput()
        {
            if (_target != null)
            {
                agent.stoppingDistance = attackController.SelectedRangedAttack.CurrentComboDamageData.attackRange;
                if (Vector3.Distance(transform.position, _target.GetTransform().position) > agent.stoppingDistance)
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
                agent.stoppingDistance = 0.5f;
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
                if (Vector3.Distance(transform.position, _target.GetTransform().position) <= agent.stoppingDistance && !isAiming)
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