using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.InputSystem
{
    public class RangedEnemyInput : EnemyInputBase
    {
        [SerializeField] private float aimCorrectionAssist = 14f;
        private bool isAiming;

        private MovementBase _targetMovement;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            if (attackSelector.Target != null)
            {
                if (CanAttack() && !isAiming)
                {
                    isAiming = true;
                    OnAimActionStarted?.Invoke();
                    var rangedAnimData = (RangedAttackAnimationData)attackSelector.SelectedAttack.CurrentComboAnimationData;
                    DOVirtual.DelayedCall(rangedAnimData.castTime, () =>
                    {
                        OnAimActionEnded?.Invoke();
                        isAiming = false;
                    });
                }
            }
        }

        public override Vector3 GetLookInput()
        {
            if (attackSelector.Target != null)
            {
                if (_targetMovement == null)
                {
                    if (attackSelector.Target.TryGetComponent(out MovementBase movement))
                    {
                        _targetMovement = movement;
                    }
                }
                var targetsVelocity = _targetMovement ? _targetMovement.GetVelocity() : Vector3.zero;
                return (attackSelector.Target.position + aimCorrectionAssist * Time.deltaTime * targetsVelocity - transform.position).normalized;
            }
            else
            {
                return (startPos - transform.position).normalized;
            }
        }

        public override Vector3 GetMovementInput()
        {
            if (attackSelector.Target != null)
            {
                if (!TargetIsInAttackRange() || !TargetIsInLineOfSight())
                {
                    return attackSelector.Target.position;
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

        private bool CanAttack()
        {
            return TargetIsInAttackRange() && TargetIsInLineOfSight();
        }

        protected override bool TargetIsInAttackRange()
        {
            return Vector3.Distance(transform.position, attackSelector.Target.position) <= attackSelector.SelectedAttack.CurrentComboDamageData.attackRange;
        }
    }
}