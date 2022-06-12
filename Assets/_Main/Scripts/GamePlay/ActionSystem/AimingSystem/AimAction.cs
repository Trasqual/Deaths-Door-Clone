using DG.Tweening;
using System;
using UnityEngine;

public class AimAction : ActionBase, IRotationOverrider, IMovementOverrider
{
    public event Action<IRotationOverrider> OnRotationOverrideStarted;
    public event Action OnRotationOverrideEnded;
    public event Action<Vector3, float> OnRotationOverridePerformed;
    public event Action<IMovementOverrider> OnMovementOverrideStarted;
    public event Action OnMovementOverrideEnded;
    public event Action<Vector3, float> OnMovementOverridePerformed;

    [SerializeField] private float aimRotationSpeed = 5f;
    [SerializeField] private float aimEndDelayForRecoilAnim = 0.2f;

    private InputBase input;
    private IOverrideChecker overrideChecker;

    bool isAiming;

    private void Awake()
    {
        input = GetComponent<InputBase>();
        overrideChecker = GetComponent<IOverrideChecker>();

        input.OnAimActionStarted += OnAimStarted;
        input.OnAimActionEnded += OnAimEnded;
    }

    protected virtual void OnAimStarted()
    {
        if (!overrideChecker.CanOverride()) return;
        if (isAiming) return;
        isAiming = true;
        OnActionStarted?.Invoke();
        OnRotationOverrideStarted?.Invoke(this);
        OnMovementOverrideStarted?.Invoke(this);
        OnMovementOverridePerformed?.Invoke(Vector3.zero, 0f);

    }

    protected virtual void OnAimEnded()
    {
        if (!isAiming) return;
        isAiming = false;
        OnActionEnded?.Invoke();
        DOVirtual.DelayedCall(aimEndDelayForRecoilAnim, () =>
        {
            OnRotationOverrideEnded?.Invoke();
            OnMovementOverrideEnded?.Invoke();
        });
    }

    protected virtual void ProcessAimRotation()
    {
        OnRotationOverridePerformed?.Invoke(input.GetLookInput(), aimRotationSpeed);
        OnActionPerformed?.Invoke();
    }

    private void Update()
    {
        if (isAiming)
        {
            ProcessAimRotation();
        }
    }
}
