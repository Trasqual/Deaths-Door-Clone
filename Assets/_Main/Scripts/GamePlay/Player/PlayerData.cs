using _Main.Scripts.GamePlay.Indicators.AimingIndicator;
using _Main.Scripts.Others;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        [TitleGroup("Base")]
        [SerializeField] private string name = "";

        [TitleGroup("Locomotion")]
        [SerializeField] private float movementSpeed = 5F;
        [SerializeField] private float rotationSpeed = 5F;
        [SerializeField] private float dodgeSpeed = 5F;

        [TitleGroup("Health")]
        [SerializeField] private int health = 4;
        [SerializeField] public float damageTakenDuration = 0.5f;

        public AimActionIndicator aimingIndicator;
        public CameraTargetGroup cameraTargetGroup;
    }
}