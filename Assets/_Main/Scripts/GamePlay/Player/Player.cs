using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(PlayerMovement), 
        typeof(PlayerAnimation))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData data;
        [SerializeField] private PlayerInput input;

        public PlayerData Data => data;

        public PlayerInput Input => input;
    }
}