
using _Main.Scripts.GamePlay.Movement;

public class MovementState : StateBase
{
    InputBase input;
    Movement movement;
    float speedMultiplier;

    public MovementState(StateMachine stateMachine, InputBase input, Movement movement, float movementSpeedMultiplier) : base(stateMachine)
    {
        this.input = input;
        this.movement = movement;
        speedMultiplier = movementSpeedMultiplier;
    }

    public override void EnterState()
    {
        movement.StartMovementAndRotation();
    }

    public override void UpdateState()
    {
        movement.Move(input.GetMovementInput(), speedMultiplier, 1f);
    }

    public override void ExitState()
    {
        movement.StopMovementAndRotation();
    }

    public override void CancelState()
    {
        movement.StopMovementAndRotation();
    }
}
