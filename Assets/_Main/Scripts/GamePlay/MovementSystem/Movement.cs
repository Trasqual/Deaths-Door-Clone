using UnityEngine;

namespace _Main.Scripts.GamePlay.Movement
{
    public abstract class Movement : MonoBehaviour
    {
        protected abstract bool IsMoving();
        protected abstract bool CanMove();
        public abstract void StartMovement();
        public abstract void StopMovement();
        public abstract void StartRotation();
        public abstract void StopRotation();
        public abstract void MoveInDirection(Vector3 dir, float speed);

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

        public bool IsInSpecialAction;
    }
}