using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        public AimingState AimingState { get; private set; }
        public AttackState AttackState { get; private set; }
        public StateBase CurrentState { get; private set; }
        
        private InputBase _inputBase = null;
        private Movement _movement = null;
        private Animator _animator = null;

        [Header("Aiming Params")]
        [SerializeField] private float aimSpeedMultiplier = 1f;
        [SerializeField] private float recoilDelay = 0.2f;

        [Header("Attack Params")]
        [SerializeField] private float comboTimer = 0.5f;

        [Header("Debugging")]
        [SerializeField] private string currentStateName;
        [SerializeField] private List<StateBase> states = new List<StateBase>();

        public void Initialize(InputBase input, Movement movement, Animator animator)
        {
            _inputBase = input;
            _movement = movement;
            _animator = animator;

            AddMovementState();
            AddDodgeState(2F, .5F);
            
            // MovementState = new MovementState(0, this, inputBase, movement, movementSpeedMultiplier);
            // RollingState = new RollingState(2, this, anim, inputBase, movement, rollingSpeedMultiplier, rollDuration);
            // AimingState = new AimingState(1, this, anim, inputBase, movement, aimSpeedMultiplier, recoilDelay);
            // AttackState = new AttackState(1, this, anim, inputBase, movement, comboTimer);
            //
            // AddAnyTransition(RollingState, () => true, () => true);
            // AddTransition(RollingState, AimingState, () => RollingState.IsRollingComplete, () => false);
            // AddTransition(RollingState, MovementState, () => RollingState.IsRollingComplete, () => false);
            // AddTransition(RollingState, AttackState, () => RollingState.IsRollingComplete, () => false);
            // AddTransition(AimingState, MovementState, () => true, () => false);
            // AddTransition(AttackState, MovementState, () => AttackState.IsAttackComplete, () => false);
            //
            // ChangeState(MovementState);
        }

        public void AddMovementState()
        {
            var movementState = gameObject.AddComponent<MovementState>();
            movementState.Initialize(_inputBase, _movement, _animator);
            states.Add(movementState);
        }

        public void AddDodgeState(float speedMultiplier, float duration)
        {
            var dodgeState = gameObject.AddComponent<DodgeState>();
            dodgeState.Initialize(_inputBase, _movement, _animator, speedMultiplier, duration);
            states.Add(dodgeState);
            dodgeState.OnComplete += OnCompleteState;
        }

        private void OnCompleteState()
        {
            var defaultState = states[0];
            ChangeState(defaultState.GetType());
        }
        
        private void Update()
        {
            CurrentState.UpdateState();
        }

        public void ChangeState(Type to)
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
                    currentStateName = CurrentState.ToString();
                }
            }
            else
            {
                CurrentState = state;
                CurrentState.EnterState();
                currentStateName = CurrentState.ToString();
            }
        }
    }
}