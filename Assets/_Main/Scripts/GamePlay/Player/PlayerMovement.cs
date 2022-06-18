using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovement : Movement.Movement
    {
        [SerializeField] private float baseMovementSpeed = 5f;
        [SerializeField] private float baseRotationSpeed = 20f;

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

        protected void MoveInDirection(Vector3 dir, float speed)
        {
            player.Controller.Move(dir * Time.deltaTime * speed);
        }

        protected void RotateInDirection(Vector3 direction, float rotationSpeed)
        {
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            }
        }

        public override void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier)
        {
            if (canMove)
            {
                MoveInDirection(dir, baseMovementSpeed * movementSpeedMultiplier);
            }

            if (canRotate)
            {
                RotateInDirection(dir, baseRotationSpeed * rotationSpeedMultiplier);
            }

            player.PlayerAnim.PlayMovementAnim(canMove ? dir : Vector3.zero);
        }
    }
}