using UnityEngine;

namespace _Main.Scripts.GamePlay.InputSystem
{
    public class MeleeEnemyInput : EnemyInputBase
    {
        protected override void Update()
        {
            if (attackSelector.Target != null)
            {
                if (TargetIsInAttackRange())
                {
                    OnAttackActionStarted?.Invoke();
                }
            }
        }

        public override Vector3 GetMovementInput()
        {
            if (attackSelector.Target != null)
            {
                if (!TargetIsInAttackRange())
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

        protected override bool TargetIsInAttackRange()
        {
            return Vector3.Distance(transform.position, attackSelector.Target.position) <= attackSelector.SelectedAttack.CurrentComboDamageData.attackRange;
        }
    }
}