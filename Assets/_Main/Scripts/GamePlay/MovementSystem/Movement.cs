using UnityEngine;

namespace _Main.Scripts.GamePlay.Movement
{
    public abstract class Movement : MonoBehaviour
    {
        protected abstract bool IsMoving();
        public abstract void StartMovement();
        public abstract void StopMovement();
        public abstract void StartRotation();
        public abstract void StopRotation();

        public void StartMovementAndRotation()
        {
            StartMovement();
            StartRotation();
        }

        public void StopMovementAndRotation()
        {
            StopMovement();
            StopRotation();
        }

        public abstract void Move(Vector3 dir, float movementSpeedMultiplier, float rotationSpeedMultiplier);
    }
}