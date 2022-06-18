
public class MovementState : StateBase
{
    public MovementState(StateMachine stateMachine) : base(stateMachine) { }

    public override void EnterState()
    {
        stateMachine.Movement.StartMovementAndRotation();
    }

    public override void UpdateState()
    {
        stateMachine.Movement.Move(stateMachine.InputBase.GetMovementInput(), stateMachine.MovementSpeedMultiplier);
    }

    public override void ExitState()
    {
        stateMachine.Movement.StopMovementAndRotation();
    }

    public override void CancelState()
    {
        stateMachine.Movement.StopMovementAndRotation();
    }
}
