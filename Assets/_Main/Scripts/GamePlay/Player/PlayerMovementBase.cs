using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovementBase : MovementBase
    {
        [SerializeField] private float baseMovementSpeed = 5f;
        [SerializeField] private float baseRotationSpeed = 20f;
        [SerializeField] private float fallToDeathTime = 2f;

        private Player player;

        private bool canMove = true;
        private bool canRotate = true;
        private bool applyGravity = true;

        private float fallTimer = 0f;

        private void Start()
        {
            player = GetComponent<Player>();
        }

        private void Update()
        {
            if (!player.Controller.isGrounded)
            {
                fallTimer += Time.deltaTime;
                if (fallTimer >= fallToDeathTime)
                {
                    canMove = false;
                    applyGravity = false;
                    transform.position = new Vector3(-13f, 1f, 0f);
                    fallTimer = 0f;
                    DOVirtual.DelayedCall(1f, () => { canMove = true; applyGravity = true; });
                }
            }
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

        private void ApplyGravity()
        {
            player.Controller.Move(Physics.gravity * Time.deltaTime);
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

            if (applyGravity)
            {
                ApplyGravity();
            }
        }
    }
}