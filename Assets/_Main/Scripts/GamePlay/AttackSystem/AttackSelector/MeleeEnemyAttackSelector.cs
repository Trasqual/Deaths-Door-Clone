
using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class MeleeEnemyAttackSelector : EnemyAttackSelectorBase
    {
        private AttackBase _jumpAttack;
        private AttackBase _regularAttack;

        protected override void Awake()
        {
            base.Awake();

            _regularAttack = _attackController.GetMeleeAttacks()[0];

            _jumpAttack = _attackController.GetMeleeAttacks()[1];
        }

        private void InitialSelection()
        {
            _attackController.SetSelectedMeleeAttack(0);
            SelectedAttack = _attackController.SelectedMeleeAttack;
        }

        protected override void SelectAttack()
        {
            if (!IsAttackAvailable(_regularAttack) && IsAttackAvailable(_jumpAttack))
            {
                _attackController.SetSelectedMeleeAttack(1);
            }
            else
            {
                _attackController.SetSelectedMeleeAttack(0);
            }
            if (SelectedAttack != _attackController.SelectedMeleeAttack)
            {
                SelectedAttack = _attackController.SelectedMeleeAttack;
            }
            Debug.Log(SelectedAttack);
        }

        private void OnEnable()
        {
            _attackController.OnMeleeAttackStateSet += InitialSelection;
        }

        private void OnDisable()
        {
            _attackController.OnMeleeAttackStateSet -= InitialSelection;
        }
    }
}
