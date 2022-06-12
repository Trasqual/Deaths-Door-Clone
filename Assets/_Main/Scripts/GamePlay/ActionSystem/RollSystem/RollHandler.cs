using DG.Tweening;
using System;
using UnityEngine;

public class RollHandler : MonoBehaviour, IMovementOverrider, IRotationOverrider
{
    public event Action<IMovementOverrider> OnMovementOverrideStarted;
    public event Action OnMovementOverrideEnded;
    public event Action<Vector3, float> OnMovementOverridePerformed;
    public event Action<IRotationOverrider> OnRotationOverrideStarted;
    public event Action OnRotationOverrideEnded;
    public event Action<Vector3, float> OnRotationOverridePerformed;

    [SerializeField] float rollSpeed = 10f;
    [SerializeField] float rollDuration = 1.5f;

    private InputBase input;
    private AnimationBase anim;
    private IOverrideChecker overrideChecker;

    bool isRolling;

    private void Awake()
    {
        input = GetComponent<InputBase>();
        anim = GetComponent<AnimationBase>();
        overrideChecker = GetComponent<IOverrideChecker>();

        input.OnRollAction += StartRoll;
    }

    private void StartRoll()
    {
        if (!overrideChecker.CanOverride()) return;
        if (isRolling) return;
        isRolling = true;
        anim.PlayRollAnim();
        OnMovementOverrideStarted?.Invoke(this);
        OnRotationOverrideStarted?.Invoke(this);
        OnRotationOverridePerformed?.Invoke(input.GetMovementInput(), rollSpeed);
        OnMovementOverridePerformed?.Invoke(input.GetMovementInput() == Vector3.zero ? transform.forward : input.GetMovementInput().normalized, rollSpeed);
        DOVirtual.DelayedCall(rollDuration, () => StopRoll());
    }

    private void StopRoll()
    {
        if (!isRolling) return;
        isRolling = false;
        OnMovementOverrideEnded?.Invoke();
        OnRotationOverrideEnded?.Invoke();
    }
}
