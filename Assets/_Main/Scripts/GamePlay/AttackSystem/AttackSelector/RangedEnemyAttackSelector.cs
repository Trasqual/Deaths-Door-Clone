
namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class RangedEnemyAttackSelector : EnemyAttackSelectorBase
    {
        private void Start()
        {
            Invoke(nameof(SelectAttack), 0.5f);
        }

        protected override void SelectAttack()
        {
            if (SelectedAttack != null) return;
            if (_attackController.GetRangedAttacks().Count > 0)
            {
                _attackController.SetSelectedRangedAttack();
                SelectedAttack = _attackController.SelectedRangedAttack;
            }
        }
    }
}
