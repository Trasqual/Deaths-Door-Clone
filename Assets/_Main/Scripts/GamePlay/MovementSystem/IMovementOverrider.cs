using System;
using UnityEngine;

public interface IMovementOverrider
{
    public event Action<IMovementOverrider> OnMovementOverrideStarted;
    public event Action OnMovementOverrideEnded;
    public event Action<Vector3, float> OnMovementOverridePerformed;
}
