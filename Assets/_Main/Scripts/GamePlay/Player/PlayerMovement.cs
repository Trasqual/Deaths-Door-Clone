using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovement : Movement.Movement
    {
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float rotationSpeed = 5f;
        Quaternion lastRot;

        Player player;
        CharacterController controller;

        private void Start()
        {
            player = GetComponent<Player>();
            controller = GetComponent<CharacterController>();
        }

        protected override bool IsMoving()
        {
            throw new System.NotImplementedException();
        }

        protected override void StartMovement()
        {
            throw new System.NotImplementedException();
        }

        protected override void StopMovement()
        {
            throw new System.NotImplementedException();
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
            if (player.Input.IsAiming)
            {
                ProcessAimRotation();
            }
            else
            {
                ProcessMovement();
            }
        }
    }
}