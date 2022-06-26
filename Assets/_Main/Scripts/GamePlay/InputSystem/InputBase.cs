using System;
using UnityEngine;

namespace _Main.Scripts.GamePlay.InputSystem
{
    public abstract class InputBase : MonoBehaviour
    {
        public Action OnRollAction;
        public Action OnAimActionStarted;
        public Action OnAimActionEnded;
        public Action OnAttackAction;

        public abstract Vector3 GetMovementInput();

        public abstract Vector3 GetLookInput();
    }
}
