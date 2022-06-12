using UnityEngine;

namespace _Main.Scripts.GamePlay.Player
{
    public class PlayerMovement : Movement.Movement, IOverrideChecker
    {
        [SerializeField] private float baseMovementSpeed = 5f;
        [SerializeField] private float baseRotationSpeed = 5f;

        private Player player;

        private bool canMove = true;
        private bool canRotate = true;
        Vector3 movementDirection;
        Vector3 rotationDirection;
        float currentMovementSpeed;
        float currentRotationSpeed;


        #region
        IMovementOverrider currentMovementOverrider;
        IRotationOverrider currentRotationOverrider;
        private bool isMovementOverriden = false;
        private bool isRotationOverriden = false;
        private Vector3 movementOverrideDirection;
        private Vector3 rotationOverrideDirection;
        private float movementOverrideSpeed = 0f;
        private float rotationOverrideSpeed = 0f;
        #endregion

        private void Start()
        {
            player = GetComponent<Player>();

            foreach (var movementOverrider in GetComponents<IMovementOverrider>())
            {
                movementOverrider.OnMovementOverrideStarted += StartMovementOverride;
                movementOverrider.OnMovementOverrideEnded += EndMovementOverride;
            }

            foreach (var rotationOverrider in GetComponents<IRotationOverrider>())
            {
                rotationOverrider.OnRotationOverrideStarted += StartRotationOverride;
                rotationOverrider.OnRotationOverrideEnded += EndRotationOverride;
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

        public bool CanOverride()
        {
            return !isMovementOverriden;
        }

        private void StartMovementOverride(IMovementOverrider currentOverrider)
        {
            isMovementOverriden = true;
            currentMovementOverrider = currentOverrider;
            currentMovementOverrider.OnMovementOverridePerformed += ProcessMovementOverride;
        }

        private void EndMovementOverride()
        {
            if (!isMovementOverriden) return;
            isMovementOverriden = false;
            currentMovementOverrider.OnMovementOverridePerformed -= ProcessMovementOverride;
            currentMovementOverrider = null;
        }

        private void ProcessMovementOverride(Vector3 overrideDirection, float overrideSpeed)
        {
            movementOverrideDirection = overrideDirection;
            movementOverrideSpeed = overrideSpeed;
        }

        private void StartRotationOverride(IRotationOverrider currentOverrider)
        {
            isRotationOverriden = true;
            currentRotationOverrider = currentOverrider;
            currentRotationOverrider.OnRotationOverridePerformed += ProcessRotationOverride;
        }

        private void EndRotationOverride()
        {
            if (!isRotationOverriden) return;
            isRotationOverriden = false;
            currentRotationOverrider.OnRotationOverridePerformed -= ProcessRotationOverride;
            currentRotationOverrider = null;
        }

        private void ProcessRotationOverride(Vector3 overrideDirection, float overrideSpeed)
        {
            rotationOverrideDirection = overrideDirection;
            rotationOverrideSpeed = overrideSpeed;
        }

        public override void MoveInDirection(Vector3 dir, float speed)
        {
            player.Controller.Move(dir * Time.deltaTime * speed);
        }

        public void RotateInDirection(Vector3 direction, float rotationSpeed)
        {
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            }
        }

        protected override bool CanMove()
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            if (canMove)
            {
                if (isMovementOverriden)
                {
                    movementDirection = movementOverrideDirection;
                    currentMovementSpeed = movementOverrideSpeed;

                }
                else
                {
                    movementDirection = player.Input.GetMovementInput();
                    currentMovementSpeed = baseMovementSpeed;
                }
                MoveInDirection(movementDirection, currentMovementSpeed);
            }


            if (canRotate)
            {
                if (isRotationOverriden)
                {
                    rotationDirection = rotationOverrideDirection;
                    currentRotationSpeed = rotationOverrideSpeed;
                }
                else
                {
                    rotationDirection = player.Input.GetMovementInput();
                    currentRotationSpeed = baseRotationSpeed;
                }
                RotateInDirection(rotationDirection, currentRotationSpeed);
            }

            player.PlayerAnim.PlayMovementAnim(canMove ? movementDirection : Vector3.zero);
        }
    }
}