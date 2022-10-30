using UnityEngine;

namespace _Main.Scripts.GamePlay.MovementSystem
{
    public abstract class MovementBase : MonoBehaviour
    {
        protected bool _canMove = true;
        protected bool _canRotate = true;

        protected abstract bool IsMoving();

        public virtual void StartMovement()
        {
            _canMove = true;
        }

        public virtual void StopMovement()
        {
            _canMove = false;
        }

        public virtual void StartRotation()
        {
            _canRotate = true;
        }

        public virtual void StopRotation()
        {
            _canRotate = false;
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