using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(PlayerMovement), 
        typeof(PlayerAnimation))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData data;
        [SerializeField] private PlayerInput input;
        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerAnimation playerAnim;
        [SerializeField] private RotationHandler rotator;

        public PlayerData Data => data;

        public PlayerInput Input => input;

        public CharacterController Controller => controller;

        public PlayerAnimation PlayerAnim => playerAnim;

        public RotationHandler Rotator => rotator;
    }
}