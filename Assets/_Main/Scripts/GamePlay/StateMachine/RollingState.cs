using _Main.Scripts.GamePlay.Movement;
using DG.Tweening;
using UnityEngine;

public class RollingState : StateBase
{
    AnimationBase anim;
    InputBase input;
    Movement movement;
    float rollSpeedMultiplier;
    float rollDuration;

    public RollingState(StateMachine stateMachine, AnimationBase anim, InputBase input, Movement movement, float rollSpeedMultiplier, float rollDuration) : base(stateMachine)
    {
        this.anim = anim;
        this.input = input;
        this.movement = movement;
        this.rollSpeedMultiplier = rollSpeedMultiplier;
        this.rollDuration = rollDuration;
    }

    private Vector3 direction;

    public override void EnterState()
    {
        movement.StartMovementAndRotation();
        anim.PlayRollAnim();
        direction = input.GetMovementInput() == Vector3.zero ? stateMachine.transform.forward : input.GetMovementInput().normalized;
        stateMachine.transform.forward = direction;
        DOVirtual.DelayedCall(rollDuration, () => StopRoll());
    }

    private void StopRoll()
    {
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
        throw new System.NotImplementedException();
    }
}
