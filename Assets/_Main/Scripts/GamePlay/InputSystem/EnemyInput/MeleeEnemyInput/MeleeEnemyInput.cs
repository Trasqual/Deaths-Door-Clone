using UnityEngine;

namespace _Main.Scripts.GamePlay.InputSystem
{
    public class MeleeEnemyInput : EnemyInputBase
    {
        protected override void Update()
        {
            if (_target != null)
            {
                float distance = Vector3.Distance(transform.position, _target.GetTransform().position);

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
            if (_target != null)
            {
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
    }
}