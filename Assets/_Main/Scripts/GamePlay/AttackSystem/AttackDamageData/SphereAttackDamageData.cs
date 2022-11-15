using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
	[CreateAssetMenu(menuName = "AttackDamageData/SphereAttackDamageData")]
	public class SphereAttackDamageData : AttackDamageDataBase
	{
		public float radius = 1;
		public float range = 1;
	} 
}
