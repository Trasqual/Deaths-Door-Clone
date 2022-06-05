using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovement : Movement.Movement
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float rotationSpeed = 5f;

        private Quaternion lastRot;

        private Player player;
        private CharacterController controller;

        private bool canMove = true;

        private void Start()
        {
            player = GetComponent<Player>();
            controller = GetComponent<CharacterController>();

            player.Input.OnAimButtonPressed += StopMovement;
            player.Input.OnAimButtonReleased += StartMovement;
        }

        protected override bool IsMoving()
        {
            return controller.velocity.magnitude > 0f;
        }

        protected override void StartMovement()
        {
            canMove = true;
        }

        protected override void StopMovement()
        {
            canMove = false;
        }

        protected override void ProcessMovement()
        {
            var movementDirection = player.Input.GetMovementInput();
            controller.Move(movementDirection * Time.deltaTime * movementSpeed);
            ProcessRotation(movementDirection);
        }

        private void ProcessRotation(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
                lastRot = transform.rotation;
            }
            else
            {
                transform.rotation = lastRot;
            }
        }

        private void ProcessAimRotation()
        {
            ProcessRotation(player.Input.GetLookInput());
        }

        protected override bool CanMove()
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            if (canMove)
            {
                ProcessMovement();
            }
            else
            {
                ProcessAimRotation();
            }
        }
    }
}