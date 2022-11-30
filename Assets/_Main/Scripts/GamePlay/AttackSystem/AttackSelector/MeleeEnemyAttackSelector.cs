
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
        }

        protected override void SelectAttack()
        {
            if (!IsAttackAvailable(_regularAttack) && IsAttackAvailable(_jumpAttack))
            {
                _attackController.SetSelectedMeleeAttack(1);
            }
            else if (!IsAttackAvailable(_jumpAttack))
            {
                _attackController.SetSelectedMeleeAttack(0);
            }
        }

        private void SetSelectedAttack(AttackBase selectedAttack)
        {
            if (SelectedAttack != selectedAttack)
                SelectedAttack = selectedAttack;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _attackController.OnSelectedMeleeAttackChanged += SetSelectedAttack;
            _attackController.OnMeleeAttackStateSet += InitialSelection;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _attackController.OnMeleeAttackStateSet -= InitialSelection;
        }
    }
}
