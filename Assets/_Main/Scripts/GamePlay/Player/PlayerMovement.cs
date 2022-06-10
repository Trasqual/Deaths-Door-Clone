using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovement : Movement.Movement
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float rotationSpeed = 5f;

        private Player player;

        private bool canMove = true;
        private bool canRotate = true;

        private void Start()
        {
            player = GetComponent<Player>();
        }

        protected override bool IsMoving()
        {
            return player.Controller.velocity.magnitude > 0f;
        }

        public override void StartMovement()
        {
            canMove = true;
        }

        public override void StopMovement()
        {
            canMove = false;
        }

        public override void StartRotation()
        {
            canRotate = true;
        }

        public override void StopRotation()
        {
            canRotate = false;
        }

        protected override void ProcessMovement()
        {
            var movementDirection = player.Input.GetMovementInput();
            player.Controller.Move(movementDirection * Time.deltaTime * movementSpeed);
        }

        private void ProcessRotation()
        {
            player.Rotator.ProcessRotation(player.Input.GetMovementInput(), rotationSpeed);
        }

        protected override bool CanMove()
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            player.PlayerAnim.PlayMovementAnim(canMove? player.Input.GetMovementInput() : Vector3.zero);

            if (canMove)
            {
                ProcessMovement();
            }

            if (canRotate)
            {
                ProcessRotation();
            }
        }
    }
}