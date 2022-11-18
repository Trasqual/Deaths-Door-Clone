
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

        private void OnEnable()
        {
            _attackController.OnRangedAttackStateSet += SelectAttack;
        }

        private void OnDisable()
        {
            _attackController.OnRangedAttackStateSet -= SelectAttack;
        }
    }
}
