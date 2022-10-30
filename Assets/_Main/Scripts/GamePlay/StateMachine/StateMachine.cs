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
        public StateBase CurrentState { get; private set; }
        public StateBase DefaultState { get; private set; }

        private InputBase _inputBase = null;
        private MovementBase _movementBase = null;
        private Animator _animator = null;

        [Header("Debugging")]
        [SerializeField] private string _currentStateName;
        [SerializeField] private List<StateBase> _states = new List<StateBase>();

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator)
        {
            _inputBase = input;
            _movementBase = movementBase;
            _animator = animator;
        }

        public void SetInitialState(Type state)
        {
            if (CurrentState != null) return;
            if (_states.Count == 0) return;

            var foundState = GetState(state);
            DefaultState = foundState ? foundState : _states[0];
            CurrentState = DefaultState;
            CurrentState.EnterState();
            _currentStateName = CurrentState.ToString();
        }

        public void AddMovementState()
        {
            if (GetState(typeof(MovementState))) return;

            var movementState = gameObject.AddComponent<MovementState>();
            movementState.Initialize(_inputBase, _movementBase, _animator);
            _states.Add(movementState);
        }

        public void AddDodgeState(float speedMultiplier, float duration)
        {
            if (GetState(typeof(DodgeState))) return;

            var dodgeState = gameObject.AddComponent<DodgeState>();
            dodgeState.Initialize(_inputBase, _movementBase, _animator, speedMultiplier, duration);
            _states.Add(dodgeState);
            dodgeState.OnComplete += OnCompleteState;
        }

        public void AddAttackState(AttackControllerBase attackController)
        {
            if (GetState(typeof(MeleeAttackState))) return;

            var attackState = gameObject.AddComponent<MeleeAttackState>();
            attackState.Initialize(_inputBase, _movementBase, _animator, attackController);
            _states.Add(attackState);
            attackState.OnComplete += OnCompleteState;
        }

        public void AddAimingState(float aimSpeedMultiplier, float recoilDelay, AttackControllerBase attackController)
        {
            if (GetState(typeof(AimingState))) return;
            var aimingState = gameObject.AddComponent<AimingState>();
            aimingState.Initialize(_inputBase, _movementBase, _animator, aimSpeedMultiplier, recoilDelay, attackController);
            _states.Add(aimingState);
            aimingState.OnComplete += OnCompleteState;
        }

        public void AddDamageTakenState(float duration)
        {
            if (GetState(typeof(DamageTakenState))) return;

            var damageTaken = gameObject.AddComponent<DamageTakenState>();
            damageTaken.Initialize(_movementBase, _animator, duration);
            _states.Add(damageTaken);
            damageTaken.OnComplete += OnCompleteState;
        }

        public void AddDeathState()
        {
            if (GetState(typeof(DeathState))) return;

            var deathState = gameObject.AddComponent<DeathState>();
            deathState.Initialize(_movementBase, _animator);
            _states.Add(deathState);
        }

        public void RemoveState(Type targetState)
        {
            if (_states.Count == 1)
            {
                Debug.LogWarning($"The operation is blocked! : StateMachine doesn't have more than one state.");

                return;
            }

            var foundState = GetState(targetState);

            if (foundState)
            {
                _states.Remove(foundState);

                if (foundState == DefaultState)
                {
                    DefaultState = _states[0];
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
            if (CurrentState != null)
                CurrentState.UpdateState();
        }

        public StateBase GetState(Type type)
        {
            return _states.FirstOrDefault(state => state.GetType() == type);
        }

        public void ChangeState(Type to)
        {
            if (CurrentState == null) return;
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
                            _currentStateName = CurrentState.ToString();
                        }
                    }
                }
            }
        }
    }
}