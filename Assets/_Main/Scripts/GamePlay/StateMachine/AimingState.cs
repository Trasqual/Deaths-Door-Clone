using _Main.Scripts.GamePlay.Movement;
using DG.Tweening;

public class AimingState : StateBase
{

    AnimationBase anim;
    InputBase input;
    Movement movement;
    private float aimSpeedMultiplier = 1f;
    private float recoilDelay = 0.2f;

    public AimingState(StateMachine stateMachine, AnimationBase anim, InputBase input, Movement movement, float aimSpeedMultiplier, float recoilDelay) : base(stateMachine)
    {
        this.anim = anim;
        this.input = input;
        this.movement = movement;
        this.aimSpeedMultiplier = aimSpeedMultiplier;
        this.recoilDelay = recoilDelay;
    }

    public override void EnterState()
    {
        anim.PlayAimAnim(true);
        movement.StartMovementAndRotation();
    }

    public override void UpdateState()
    {
        movement.Move(input.GetLookInput(), 0f, aimSpeedMultiplier);
    }

    public void EndAim()
    {
        anim.PlayAimAnim(false);
        DOVirtual.DelayedCall(recoilDelay, () =>
        {
            stateMachine.ChangeState(stateMachine.MovementState);
        });
    }

    public override void ExitState()
    {
        movement.StopMovementAndRotation();
    }

    public override void CancelState()
    {

    }
}
