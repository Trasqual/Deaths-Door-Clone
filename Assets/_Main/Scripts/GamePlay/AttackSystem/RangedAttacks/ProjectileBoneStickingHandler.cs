using UnityEngine;

namespace _Main.Scripts.GamePlay.AttackSystem
{
    public class ProjectileBoneStickingHandler : MonoBehaviour
    {
        [SerializeField] Transform[] bones;

        public Transform GetClosestBone(Vector3 pointOfImpact)
        {
            Transform closestBone = null;
            var closestDist = float.MaxValue;

            for (int i = 0; i < bones.Length; i++)
            {
                var mag = (pointOfImpact - bones[i].position).sqrMagnitude;
                if (mag < closestDist)
                {
                    closestDist = mag;
                    closestBone = bones[i];
                }
            }
            return closestBone;
        }
    }
}