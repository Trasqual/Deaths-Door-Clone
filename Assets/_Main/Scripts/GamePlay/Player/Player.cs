using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    [RequireComponent(typeof(PlayerMovement), 
        typeof(PlayerAnimation))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerData data;

        public PlayerData Data => data;

        PlayerInput input;
        public PlayerInput Input => input;

        private void Start()
        {
            input = GetComponent<PlayerInput>();
        }
    }
}