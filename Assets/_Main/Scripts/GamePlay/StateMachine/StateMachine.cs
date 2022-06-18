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
    InputBase inputBase;
    Movement movement;
    AnimationBase anim;

    [Header("Movement Params")]
    [SerializeField] float movementSpeedMultiplier = 1f;

    [Header("Rolling Params")]
    [SerializeField] float rollingSpeedMultiplier = 2f;
    [SerializeField] float rollDuration = 1.5f;

    [Header("Aiming Params")]
    [SerializeField] float aimSpeedMultiplier = 1f;
    [SerializeField] float recoilDelay = 0.2f;

    private void Awake()
    {
        inputBase = GetComponent<InputBase>();
        movement = GetComponent<Movement>();
        anim = GetComponent<AnimationBase>();

        MovementState = new MovementState(this, inputBase, movement, movementSpeedMultiplier);
        RollingState = new RollingState(this, anim, inputBase, movement, rollingSpeedMultiplier, rollDuration);
        AimingState = new AimingState(this, anim, inputBase, movement, aimSpeedMultiplier, recoilDelay);

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