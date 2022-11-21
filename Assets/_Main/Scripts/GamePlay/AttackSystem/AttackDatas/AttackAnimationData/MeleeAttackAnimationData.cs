using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    [CreateAssetMenu(menuName = "AttackAnimationData/MeleeAttackAnimationData")]
    public class MeleeAttackAnimationData : AttackAnimationDataBase
    {
        public string fadeToAnimationName = "MeleeAttack";
        public float attackDamageDelay = 0f;
        public float attackMovementDelay = 0f;
        public float attackMovementAmount = 1f;
        public float attackMovementDuration = 1f;
        public float attackDuration = 1f;
        public float comboWaitTime = 0.4f;
        public float animationSpeedMultiplier = 1f;
        public bool useGravity = true;
        public float jumpHeight = 0f;
        public AnimationCurve yCurve;

        public virtual Vector3 GetAttackEndPosition(Transform caster)
        {
            return caster.position + caster.forward * attackMovementAmount;
        }
    } 
}
