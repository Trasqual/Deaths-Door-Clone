using UnityEngine;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{

    [CreateAssetMenu]
    public class EnemyBehaviourData : BehaviourBaseData
    {
        [Header("Locomotion")]
        public float MovementSpeed = 5F;
        public float RotationSpeed = 5F;

        [Header("Health")]
        public int Health = 4;
    } 
}