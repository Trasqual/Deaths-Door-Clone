using System;
using UnityEngine;

namespace _Main.Scripts.GamePlay.MovementSystem
{
    public interface IMovementOverrider
    {
        public event Action<IMovementOverrider> OnMovementOverrideStarted;
        public event Action OnMovementOverrideEnded;
        public event Action<Vector3, float> OnMovementOverridePerformed;
    }
}
