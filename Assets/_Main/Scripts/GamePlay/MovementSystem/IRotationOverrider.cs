using System;
using UnityEngine;

namespace _Main.Scripts.GamePlay.MovementSystem
{
    public interface IRotationOverrider
    {
        public event Action<IRotationOverrider> OnRotationOverrideStarted;
        public event Action OnRotationOverrideEnded;
        public event Action<Vector3, float> OnRotationOverridePerformed;
    }
}
