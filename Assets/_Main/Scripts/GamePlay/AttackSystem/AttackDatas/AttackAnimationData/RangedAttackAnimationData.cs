using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
	[System.Serializable]
	[CreateAssetMenu(menuName = "AttackAnimationData/RangedAttackAnimationData")]
	public class RangedAttackAnimationData : AttackAnimationDataBase
	{
		public float castTime = 3f;
		public float recoilDelay = 0.5f;
	} 
}
