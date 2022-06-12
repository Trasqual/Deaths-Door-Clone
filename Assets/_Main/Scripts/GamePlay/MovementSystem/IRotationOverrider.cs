using System;
using UnityEngine;

public interface IRotationOverrider
{
    public event Action<IRotationOverrider> OnRotationOverrideStarted;
    public event Action OnRotationOverrideEnded;
    public event Action<Vector3, float> OnRotationOverridePerformed;
}
