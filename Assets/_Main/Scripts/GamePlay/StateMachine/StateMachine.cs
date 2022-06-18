using _Main.Scripts.GamePlay.Movement;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public MovementState MovementState { get; private set; }
    public AimingState AimingState { get; private set; }
    public RecoilState RecoilState { get; private set; }
    public RollingState RollingState { get; private set; }
    public AttackState AttackState { get; private set; }

    StateBase currentState;
    public InputBase InputBase { get; private set; }
    public Movement Movement { get; private set; }
    public AnimationBase Anim { get; private set; }

    [Header("Movement Params")]
    [SerializeField] float movementSpeedMultiplier = 1f;
    public float MovementSpeedMultiplier => movementSpeedMultiplier;

    [Header("Rolling Params")]
    [SerializeField] float rollingSpeedMultiplier = 2f;
    public float RollingSpeedMultiplier => rollingSpeedMultiplier;
    [SerializeField] float rollDuration = 1.5f;
    public float RollDuration => rollDuration;

    private void Awake()
    {
        MovementState = new MovementState(this);
        RollingState = new RollingState(this);
    }

    public void ChangeState(StateBase state)
    {
        if (currentState == state) return;

        currentState.ExitState();
        currentState = state;
        state.EnterState();
    }
}