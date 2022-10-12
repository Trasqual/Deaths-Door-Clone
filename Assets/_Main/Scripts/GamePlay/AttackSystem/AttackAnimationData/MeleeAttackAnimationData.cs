using UnityEngine;

[CreateAssetMenu(menuName = "AttackAnimationData/MeleeAttackAnimationData")]
public class MeleeAttackAnimationData : AttackAnimationDataBase
{
    public float attackMovementAmount = 1f;
    public float attackMovementDuration = 1f;
    public float attackCD = 1f;
}
