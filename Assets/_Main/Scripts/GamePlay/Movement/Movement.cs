using UnityEngine;

namespace _Main.Scripts.GamePlay.Movement
{
    public abstract class Movement : MonoBehaviour
    {
        protected abstract bool IsMoving();
        protected abstract bool CanMove();
        protected abstract void StartMovement();
        protected abstract void StopMovement();
        protected abstract void ProcessMovement();
    }
}