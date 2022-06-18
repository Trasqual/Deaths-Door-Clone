using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingState : StateBase
{
    public RollingState(StateMachine stateMachine) : base(stateMachine) { }

    private Vector3 direction;

    public override void EnterState()
    {
        stateMachine.Movement.StartMovementAndRotation();
        stateMachine.Anim.PlayRollAnim();
        direction = stateMachine.InputBase.GetMovementInput() == Vector3.zero ? stateMachine.transform.forward : stateMachine.InputBase.GetMovementInput().normalized;
        DOVirtual.DelayedCall(stateMachine.RollDuration, () => StopRoll());
    }

    private void StopRoll()
    {
        stateMachine.ChangeState(stateMachine.MovementState);
    }

    public override void UpdateState()
    {
        stateMachine.Movement.Move(direction, stateMachine.RollingSpeedMultiplier);
    }

    public override void ExitState()
    {
        stateMachine.Movement.StopMovementAndRotation();
        direction = Vector3.zero;
    }

    public override void CancelState()
    {
        throw new System.NotImplementedException();
    }
}
