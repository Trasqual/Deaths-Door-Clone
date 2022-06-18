using _Main.Scripts.GamePlay.Movement;
using DG.Tweening;
using System;

public class AimingState : StateBase, IAction
{
    public event Action OnActionStart;
    public event Action OnActionEnd;

    private readonly AnimationBase anim;
    private readonly InputBase input;
    private readonly Movement movement;
    private readonly float aimSpeedMultiplier = 1f;
    private readonly float recoilDelay = 0.2f;

    private Tween recoilDelayTween;

    public AimingState(int priority, StateMachine stateMachine, AnimationBase anim, InputBase input, Movement movement, float aimSpeedMultiplier, float recoilDelay) : base(priority, stateMachine)
    {
        this.anim = anim;
        this.input = input;
        this.movement = movement;
        this.aimSpeedMultiplier = aimSpeedMultiplier;
        this.recoilDelay = recoilDelay;
    }

    public override void EnterState()
    {
        anim.PlayAimAnim(true, false);
        movement.StartMovementAndRotation();
        OnActionStart?.Invoke();
    }

    public override void UpdateState()
    {
        movement.Move(input.GetLookInput(), 0f, aimSpeedMultiplier);
    }

    public void EndAim()
    {
        anim.PlayAimAnim(false, false);
        OnActionEnd?.Invoke();
        recoilDelayTween = DOVirtual.DelayedCall(recoilDelay, () => stateMachine.ChangeState(stateMachine.MovementState));
    }

    public override void ExitState()
    {
        movement.StopMovementAndRotation();
    }

    public override void CancelState()
    {
        recoilDelayTween.Kill();
        anim.PlayAimAnim(false, true);
        movement.StopMovementAndRotation();
    }
}
