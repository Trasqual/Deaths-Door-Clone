using System;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        // public AimingState AimingState { get; private set; }
        // public AttackState AttackState { get; private set; }
        public StateBase CurrentState { get; private set; }
        public StateBase DefaultState { get; private set; }
        
        private InputBase _inputBase = null;
        private MovementBase _movementBase = null;
        private Animator _animator = null;

        [Header("Attack Params")]
        [SerializeField] private float comboTimer = 0.5f;

        [Header("Debugging")]
        [SerializeField] private string currentStateName;
        [SerializeField] private List<StateBase> states = new List<StateBase>();

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator)
        {
            _inputBase = input;
            _movementBase = movementBase;
            _animator = animator;

            // MovementState = new MovementState(0, this, inputBase, movement, movementSpeedMultiplier);
            // RollingState = new RollingState(2, this, anim, inputBase, movement, rollingSpeedMultiplier, rollDuration);
            // AimingState = new AimingState(1, this, anim, inputBase, movement, aimSpeedMultiplier, recoilDelay);
            // AttackState = new AttackState(1, this, anim, inputBase, movement, comboTimer);
            //
            // AddAnyTransition(RollingState, () => true, () => true);
            // AddTransition(RollingState, AimingState, () => RollingState.IsRollingComplete, () => false);
            // AddTransition(RollingState, MovementState, () => RollingState.IsRollingComplete, () => false);
            // AddTransition(RollingState, AttackState, () => RollingState.IsRollingComplete, () => false);
            // AddTransition(AttackState, MovementState, () => AttackState.IsAttackComplete, () => false);
            //
            // ChangeState(MovementState);
        }

        public void SetInitialState(Type state)
        {
            if(CurrentState != null) return;
            if(states.Count == 0) return;
            
            var foundState = GetState(state);
            DefaultState = foundState ? foundState : states[0];
            CurrentState = DefaultState;
            CurrentState.EnterState();
            currentStateName = CurrentState.ToString();
        }

        public void AddMovementState()
        {
            if(GetState(typeof(MovementState))) return;
            
            var movementState = gameObject.AddComponent<MovementState>();
            movementState.Initialize(_inputBase, _movementBase, _animator);
            states.Add(movementState);
        }

        public void AddDodgeState(float speedMultiplier, float duration)
        {
            if(GetState(typeof(DodgeState))) return;

            var dodgeState = gameObject.AddComponent<DodgeState>();
            dodgeState.Initialize(_inputBase, _movementBase, _animator, speedMultiplier, duration);
            states.Add(dodgeState);
            dodgeState.OnComplete += OnCompleteState;
        }

        public void AddAimingState(float aimSpeedMultiplier, float recoilDelay)
        {
            if(GetState(typeof(AimingState))) return;

            var aimingState = gameObject.AddComponent<AimingState>();
            aimingState.Initialize(_inputBase, _movementBase, _animator, aimSpeedMultiplier, recoilDelay);
            states.Add(aimingState);
            aimingState.OnComplete += OnCompleteState;
        }

        public void RemoveState(Type targetState)
        {
            if (states.Count == 1)
            {
                Debug.LogWarning($"The operation is blocked! : StateMachine doesn't have more than one state.");
                
                return;
            }
            
            var foundState = GetState(targetState);

            if (foundState)
            {
                states.Remove(foundState);
                
                if (foundState == DefaultState)
                {
                    DefaultState = states[0];
                }
                
                if (foundState == CurrentState)
                {
                    ChangeState(DefaultState.GetType());
                }
                
                Destroy(foundState);
                
                // TODO
                
                // Unsubscribe removed states
            }
        }

        private void OnCompleteState(Type requestedState)
        {
            var targetState = GetState(requestedState);
            ChangeState(targetState ? 
                targetState.GetType() : 
                DefaultState.GetType());
        }
        private void OnCompleteState()
        {
            ChangeState(DefaultState.GetType());
        }
        
        private void Update()
        {
            if(CurrentState != null)
                CurrentState.UpdateState();
        }

        public StateBase GetState(Type type)
        {
            return states.FirstOrDefault(state => state.GetType() == type);
        }
        public void ChangeState(Type to)
        {
            if(CurrentState == null) return;
            if (CurrentState.GetType() == to) return;

            var toState = GetState(to);
            
            if (toState)
            {
                if (CurrentState is ITransition stateTransition)
                {
                    if (stateTransition.TryGetTransition(to, out var suitableTransition))
                    {
                        if (suitableTransition.Condition())
                        {
                            if (suitableTransition.Override())
                            {
                                CurrentState.CancelState();
                            }
                            else
                            {
                                CurrentState.ExitState();
                            }
                            
                            CurrentState = toState;
                            CurrentState.EnterState();
                            currentStateName = CurrentState.ToString();
                        }
                    }
                }
            }
        }
    }
}