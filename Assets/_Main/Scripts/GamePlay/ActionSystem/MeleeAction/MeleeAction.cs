using System;
using UnityEngine;

public abstract class MeleeAction : ActionBase, IMovementOverrider, IRotationOverrider
{
    public event Action<IMovementOverrider> OnMovementOverrideStarted;
    public event Action OnMovementOverrideEnded;
    public event Action<Vector3, float> OnMovementOverridePerformed;
    public event Action<IRotationOverrider> OnRotationOverrideStarted;
    public event Action OnRotationOverrideEnded;
    public event Action<Vector3, float> OnRotationOverridePerformed;

    private InputBase input;
    protected IOverrideChecker overrideChecker;

    private void Awake()
    {
        input = GetComponent<InputBase>();
        overrideChecker = GetComponent<IOverrideChecker>();

        input.OnAttackAction += PerformAction;
    }

    public abstract void PerformAction();
}
