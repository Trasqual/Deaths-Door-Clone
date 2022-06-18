using _Main.Scripts.GamePlay.Movement;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public MovementState MovementState { get; private set; }
    public AimingState AimingState { get; private set; }
    public RollingState RollingState { get; private set; }
    public AttackState AttackState { get; private set; }

    private StateBase currentState;
    private InputBase inputBase;
    private Movement movement;
    private AnimationBase anim;

    [Header("Movement Params")]
    [SerializeField] private float movementSpeedMultiplier = 1f;

    [Header("Rolling Params")]
    [SerializeField] private float rollingSpeedMultiplier = 2f;
    [SerializeField] private float rollDuration = 0.5f;

    [Header("Aiming Params")]
    [SerializeField] private float aimSpeedMultiplier = 1f;
    [SerializeField] private float recoilDelay = 0.2f;

    private void Awake()
    {
        inputBase = GetComponent<InputBase>();
        movement = GetComponent<Movement>();
        anim = GetComponent<AnimationBase>();

        MovementState = new MovementState(0, this, inputBase, movement, movementSpeedMultiplier);
        RollingState = new RollingState(2, this, anim, inputBase, movement, rollingSpeedMultiplier, rollDuration);
        AimingState = new AimingState(1, this, anim, inputBase, movement, aimSpeedMultiplier, recoilDelay);

        ChangeState(MovementState);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void ChangeState(StateBase state)
    {
        if (currentState == state) return;
        if (currentState != null)
            currentState.ExitState();
        currentState = state;
        state.EnterState();
    }
}