using UnityEngine;

[CreateAssetMenu(menuName = "AttackAnimationData/MeleeAttackAnimationData")]
public class MeleeAttackAnimationData : AttackAnimationDataBase
{
    public string fadeToAnimationName = "MeleeAttack";
    public float attackDamageDelay = 0f;
    public float attackMovementDelay = 0f;
    public float attackMovementAmount = 1f;
    public float attackMovementDuration = 1f;
    public float attackCD = 1f;
}
