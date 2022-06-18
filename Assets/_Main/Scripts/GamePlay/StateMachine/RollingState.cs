using _Main.Scripts.GamePlay.Movement;
using DG.Tweening;
using UnityEngine;

public class RollingState : StateBase
{
    private readonly AnimationBase anim;
    private readonly InputBase input;
    private readonly Movement movement;
    private readonly float rollSpeedMultiplier;
    private readonly float rollDuration;

    public RollingState(int priority, StateMachine stateMachine, AnimationBase anim, InputBase input, Movement movement, float rollSpeedMultiplier, float rollDuration) : base(priority, stateMachine)
    {
        this.anim = anim;
        this.input = input;
        this.movement = movement;
        this.rollSpeedMultiplier = rollSpeedMultiplier;
        this.rollDuration = rollDuration;
    }

    public bool IsRollingComplete { get; private set; }
    private Vector3 direction;

    public override void EnterState()
    {
        movement.StartMovementAndRotation();
        anim.PlayRollAnim();
        direction = input.GetMovementInput() == Vector3.zero ? stateMachine.transform.forward : input.GetMovementInput().normalized;
        stateMachine.transform.forward = direction;
        IsRollingComplete = false;
        DOVirtual.DelayedCall(rollDuration, () => StopRoll());
    }

    private void StopRoll()
    {
        IsRollingComplete = true;
        stateMachine.ChangeState(stateMachine.MovementState);
    }

    public override void UpdateState()
    {
        movement.Move(direction, rollSpeedMultiplier, 0f);
    }

    public override void ExitState()
    {
        movement.StopMovementAndRotation();
        direction = Vector3.zero;
    }

    public override void CancelState()
    {
    }
}
