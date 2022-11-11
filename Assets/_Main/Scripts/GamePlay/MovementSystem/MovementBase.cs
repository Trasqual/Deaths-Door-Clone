using UnityEngine;

namespace _Main.Scripts.GamePlay.MovementSystem
{
    public abstract class MovementBase : MonoBehaviour
    {
        protected bool canMove = true;
        protected bool canRotate = true;

        public virtual Vector3 GetVelocity() => Vector3.zero;
        protected abstract bool IsMoving();

        public virtual void StartMovement()
        {
            canMove = true;
        }

        public virtual void StopMovement()
        {
            canMove = false;
        }

        public virtual void StartRotation()
        {
            canRotate = true;
        }

        public virtual void StopRotation()
        {
            canRotate = false;
        }

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

        public abstract void MoveOverTime(Vector3 endPos, float duration, float setDelay = 0f, bool useGravity = true, bool useAnimationMovement = false);
    }
}