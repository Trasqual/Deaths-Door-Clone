using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
	[CreateAssetMenu(menuName = "AttackDamageData/SphereAttackDamageData")]
	public class SphereAttackDamageData : AttackDamageDataBase
	{
		public float radius = 1;
		public float range = 1;

		public void DealDamage(Transform caster, out int damagedTargets)
		{
            new SphereCastDamager(caster.position + caster.up + caster.forward, radius, caster.forward, range, damage, dmgDealerType, out damagedTargets);
        }
    } 
}
