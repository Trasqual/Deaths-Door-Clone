using UnityEngine;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
	[CreateAssetMenu]
	public class RangedEnemyBehaviourData : EnemyBehaviourData
	{
		[Header("RangedAttack")]
		public float CastSpeed = 1f;
	}
}
