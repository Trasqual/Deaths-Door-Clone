using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachineSystem
{
    public abstract class StateBase : MonoBehaviour
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CancelState();
    }
}