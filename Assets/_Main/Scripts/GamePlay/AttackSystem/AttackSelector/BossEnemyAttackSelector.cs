
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class BossEnemyAttackSelector : EnemyAttackSelectorBase
    {
        AttackBase threeShotRangedAttack;
        AttackBase laserRayRangedAttack;

        protected override void Awake()
        {
            base.Awake();

            threeShotRangedAttack = _attackController.GetRangedAttacks()[0];
            laserRayRangedAttack = _attackController.GetRangedAttacks()[1];
        }

        private void InitialSelection()
        {
            _attackController.SetSelectedMeleeAttack(0);
            SelectedAttack = _attackController.GetMeleeAttacks()[0];
            _attackController.SetSelectedRangedAttack(1);
        }

        protected override void SelectAttack()
        {
            if (!IsAttackAvailable(laserRayRangedAttack) && IsAttackAvailable(threeShotRangedAttack))
            {
                _attackController.SetSelectedRangedAttack(0);
                SelectedAttack = _attackController.GetRangedAttacks()[0];
            }
            else if (IsAttackAvailable(laserRayRangedAttack) && !IsAttackAvailable(threeShotRangedAttack))
            {
                _attackController.SetSelectedRangedAttack(1);
                SelectedAttack = _attackController.GetRangedAttacks()[1];
            }
            else
            {
                _attackController.SetSelectedMeleeAttack(0);
                SelectedAttack = _attackController.GetMeleeAttacks()[0];
            }
            Debug.Log(SelectedAttack);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _attackController.OnMeleeAttackStateSet += InitialSelection;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _attackController.OnMeleeAttackStateSet -= InitialSelection;
        }
    }
}
