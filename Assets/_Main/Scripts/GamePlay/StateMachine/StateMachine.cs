using _Main.Scripts.GamePlay.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public MovementState MovementState { get; private set; }
    public AimingState AimingState { get; private set; }
    public RollingState RollingState { get; private set; }
    public AttackState AttackState { get; private set; }
    public StateBase CurrentState { get; private set; }

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

    [Header("Attack Params")]
    [SerializeField] private float comboTimer = 0.5f;

    [Header("Debugging")]
    [SerializeField] private string CurrentStateName;

    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> anyTransitions = new List<Transition>();

    private void Awake()
    {
        inputBase = GetComponent<InputBase>();
        movement = GetComponent<Movement>();
        anim = GetComponent<AnimationBase>();

        MovementState = new MovementState(0, this, inputBase, movement, movementSpeedMultiplier);
        RollingState = new RollingState(2, this, anim, inputBase, movement, rollingSpeedMultiplier, rollDuration);
        AimingState = new AimingState(1, this, anim, inputBase, movement, aimSpeedMultiplier, recoilDelay);
        AttackState = new AttackState(1, this, anim, inputBase, movement, comboTimer);

        AddAnyTransition(RollingState, () => true, () => true);
        AddTransition(MovementState, AimingState, () => true, () => false);
        AddTransition(RollingState, AimingState, () => RollingState.IsRollingComplete, () => false);
        AddTransition(RollingState, MovementState, () => RollingState.IsRollingComplete, () => false);
        AddTransition(RollingState, AttackState, () => RollingState.IsRollingComplete, () => false);
        AddTransition(MovementState, AttackState, () => true, () => true);
        AddTransition(AimingState, MovementState, () => true, () => false);
        AddTransition(AttackState, MovementState, () => AttackState.IsAttackComplete, () => false);

        ChangeState(MovementState);
    }

    private void Update()
    {
        CurrentState.UpdateState();
    }

    public void ChangeState(StateBase state)
    {
        if (CurrentState == state) return;

        if (CurrentState != null)
        {
            var transition = GetTransition(CurrentState, state);
            if (transition.Condition())
            {
                if (transition.Override())
                {
                    CurrentState?.CancelState();
                }
                else
                {
                    CurrentState?.ExitState();
                }
                CurrentState = state;
                CurrentState.EnterState();
                CurrentStateName = CurrentState.ToString();
            }
        }
        else
        {
            CurrentState = state;
            CurrentState.EnterState();
            CurrentStateName = CurrentState.ToString();
        }
    }

    public class Transition
    {
        public StateBase To;
        public Func<bool> Condition;
        public Func<bool> Override;

        public Transition(StateBase to, Func<bool> condition, Func<bool> shouldOverride)
        {
            To = to;
            Condition = condition;
            Override = shouldOverride;
        }
    }

    public void AddTransition(StateBase from, StateBase to, Func<bool> condition, Func<bool> shouldOverride)
    {
        if (!transitions.TryGetValue(from.GetType(), out var setTransitions))
        {
            setTransitions = new List<Transition>();
            transitions[from.GetType()] = setTransitions;
        }

        setTransitions.Add(new Transition(to, condition, shouldOverride));
    }

    public void AddAnyTransition(StateBase to, Func<bool> condition, Func<bool> shouldOverride)
    {
        anyTransitions.Add(new Transition(to, condition, shouldOverride));
    }

    private Transition GetTransition(StateBase from, StateBase to)
    {
        var anyCheck = anyTransitions.Find(x => x.To == to);
        if (anyCheck != null)
        {
            return anyCheck;
        }

        var transitionCheck = transitions[from.GetType()].Find(x => x.To == to);
        if (transitionCheck != null)
        {
            return transitionCheck;
        }

        return null;
    }
}