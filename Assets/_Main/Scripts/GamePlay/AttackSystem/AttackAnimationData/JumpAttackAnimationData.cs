using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    [CreateAssetMenu(menuName = "AttackAnimationData/MeleeAttackAnimationData/JumpAttackAnimationData")]
    public class JumpAttackAnimationData : MeleeAttackAnimationData
    {
        [SerializeField] private LayerMask groundMask;

        public override Vector3 GetAttackEndPosition(Transform caster)
        {
            var endPos = caster.position + caster.forward * attackMovementAmount;
            if (Physics.Raycast(endPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, groundMask, QueryTriggerInteraction.Ignore))
            {
                Debug.Log(hit.transform.name);
                endPos = hit.point;
            }
            return endPos;
        }
    }
}