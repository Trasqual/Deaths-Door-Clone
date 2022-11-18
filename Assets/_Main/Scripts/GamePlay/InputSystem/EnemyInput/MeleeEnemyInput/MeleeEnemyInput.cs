using UnityEngine;

namespace _Main.Scripts.GamePlay.InputSystem
{
    public class MeleeEnemyInput : EnemyInputBase
    {
        protected override void Update()
        {
            if (attackSelector.Target != null)
            {
                float distance = Vector3.Distance(transform.position, attackSelector.Target.position);

                SetCurrentAttack(distance);

                if (distance <= attackController.SelectedMeleeAttack.CurrentComboDamageData.attackRange)
                {
                    OnAttackActionStarted?.Invoke();
                }
            }
        }

        private void SetCurrentAttack(float distance)
        {
            if (distance >= attackController.GetMeleeAttacks()[1].CurrentComboDamageData.attackRange * .8f && !attackController.GetMeleeAttacks()[1].IsOnCooldown)
            {
                OnMeleeWeaponSwitchedWithID?.Invoke(1);
            }
            else
            {
                OnMeleeWeaponSwitchedWithID?.Invoke(0);
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
            return Vector3.Distance(transform.position, attackSelector.Target.position) <= attackController.SelectedMeleeAttack.CurrentComboDamageData.attackRange;
        }
    }
}