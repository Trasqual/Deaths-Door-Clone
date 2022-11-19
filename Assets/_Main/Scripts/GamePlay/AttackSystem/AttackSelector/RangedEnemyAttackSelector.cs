
namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class RangedEnemyAttackSelector : EnemyAttackSelectorBase
    {
        protected override void SelectAttack()
        {
            if (SelectedAttack != null) return;
            if (_attackController.GetRangedAttacks().Count > 0)
            {
                _attackController.SetSelectedRangedAttack();
                SelectedAttack = _attackController.SelectedRangedAttack;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _attackController.OnRangedAttackStateSet += SelectAttack;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _attackController.OnRangedAttackStateSet -= SelectAttack;
        }
    }
}
