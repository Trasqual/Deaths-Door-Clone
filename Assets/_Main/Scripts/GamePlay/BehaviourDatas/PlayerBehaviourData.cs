using _Main.Scripts.GamePlay.Indicators;
using _Main.Scripts.Others;
using UnityEngine;

namespace _Main.Scripts.GamePlay.BehaviourSystem
{
    [CreateAssetMenu]
    public class PlayerBehaviourData : BehaviourBaseData
    {
        [Header("Locomotion")]
        public float MovementSpeed  = 5F;
        public float RotationSpeed = 5F;

        [Header("Dodge")]
        public float DodgeSpeedMultiplier  = 2F;
        public float DodgeDuration  = 0.5F;
        public float DodgeCD  = 2F;

        [Header("Health")]
        public int Health = 4;
        public float DamageTakenStateDuration = 0.5f;
        public float InvulnerabilityDurationAfterTakingDamage = 1f;

        [Header("Attack")]
        public float AttackRange = 1.5f;
        
        public AimActionIndicator aimingIndicator;
        public CameraTargetGroup cameraTargetGroup;
    }
}