using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
	public class RangedEnemyAttackSelector : EnemyAttackSelectorBase
	{
        protected override void SelectAttack()
        {
            if(_attackController.GetRangedAttacks().Count > 0)
            {

            }
        }
    }
}
