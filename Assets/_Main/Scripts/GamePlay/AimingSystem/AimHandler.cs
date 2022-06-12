using DG.Tweening;
using System;
using UnityEngine;

public class AimHandler : MonoBehaviour, IRotationOverrider, IMovementOverrider
{
    public event Action<IRotationOverrider> OnRotationOverrideStarted;
    public event Action OnRotationOverrideEnded;
    public event Action<Vector3, float> OnRotationOverridePerformed;
    public event Action<IMovementOverrider> OnMovementOverrideStarted;
    public event Action OnMovementOverrideEnded;
    public event Action<Vector3, float> OnMovementOverridePerformed;

    public Action OnAimActionStarted;
    public Action OnAimActionEnded;
    public Action OnAimActionPerformed;

    [SerializeField] private float aimRotationSpeed = 5f;
    [SerializeField] private float aimEndDelayForRecoilAnim = 0.2f;

    private InputBase input;
    private AnimationBase anim;
    private IOverrideChecker overrideChecker;


    bool isAiming;


    private void Awake()
    {
        input = GetComponent<InputBase>();
        anim = GetComponent<AnimationBase>();
        overrideChecker = GetComponent<IOverrideChecker>();

        input.OnAimActionStarted += OnAimStarted;
        input.OnAimActionEnded += OnAimEnded;
    }

    protected virtual void OnAimStarted()
    {
        if (!overrideChecker.CanOverride()) return;
        if (isAiming) return;
        isAiming = true;
        anim.PlayAimAnim(isAiming);
        OnAimActionStarted?.Invoke();
        OnRotationOverrideStarted?.Invoke(this);
        OnMovementOverrideStarted?.Invoke(this);
        OnMovementOverridePerformed?.Invoke(Vector3.zero, 0f);

    }

    protected virtual void OnAimEnded()
    {
        if (!isAiming) return;
        isAiming = false;
        anim.PlayAimAnim(isAiming);
        OnAimActionEnded?.Invoke();
        DOVirtual.DelayedCall(aimEndDelayForRecoilAnim, () =>
        {
            OnRotationOverrideEnded?.Invoke();
            OnMovementOverrideEnded?.Invoke();
        });
    }

    protected virtual void ProcessAimRotation()
    {
        OnRotationOverridePerformed?.Invoke(input.GetLookInput(), aimRotationSpeed);
        OnAimActionPerformed?.Invoke();
    }

    private void Update()
    {
        if (isAiming)
        {
            ProcessAimRotation();
        }
    }
}
