using _Main.Scripts.GamePlay.BehaviourSystem;
using _Main.Scripts.GamePlay.HealthSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.InputSystem
{
    public class BossEnemyInput : EnemyInputBase
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
            if (attackSelector.Target != null)
            {
                var targetsVelocity = _targetMovement ? _targetMovement.GetVelocity() : Vector3.zero;
                return attackSelector.Target.position + aimCorrectionAssist * Time.deltaTime * targetsVelocity;
            }
            else
            {
                return startPos;
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

        protected override void Update()
        {
            if (attackSelector.Target != null)
            {
                if (CanAttack() && !isAiming)
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

        private bool CanAttack()
        {
            return TargetIsInAttackRange() && TargetIsInLineOfSight();
        }
    }
}