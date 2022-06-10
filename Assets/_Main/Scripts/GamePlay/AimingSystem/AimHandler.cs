using _Main.Scripts.GamePlay.Movement;
using DG.Tweening;
using UnityEngine;

public class AimHandler : MonoBehaviour
{
    [SerializeField] private float aimRotationSpeed = 5f;
    [SerializeField] private float aimEndDelayForRecoilAnim = 0.2f;

    private InputBase input;
    private Movement movement;
    private AnimationBase anim;
    private RotationHandler rotator;

    bool isAiming;

    private void Awake()
    {
        input = GetComponent<InputBase>();
        movement = GetComponent<Movement>();
        anim = GetComponent<AnimationBase>();
        rotator = GetComponent<RotationHandler>();

        input.OnAimActionStarted += OnAimStarted;
        input.OnAimActionEnded += OnAimEnded;
    }

    private void OnAimStarted()
    {
        isAiming = true;
        movement.StopMovement();
        movement.StopRotation();
        anim.PlayAimAnim(isAiming);
    }

    private void OnAimEnded()
    {
        isAiming = false;
        anim.PlayAimAnim(isAiming);
        DOVirtual.DelayedCall(aimEndDelayForRecoilAnim, () =>
        {
            movement.StartMovement();
            movement.StartRotation();
        });
    }

    private void ProcessAimRotation()
    {
        rotator.ProcessRotation(input.GetLookInput(), aimRotationSpeed);
    }

    private void Update()
    {
        if (isAiming)
        {
            ProcessAimRotation();
        }
    }
}
