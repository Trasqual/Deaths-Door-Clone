using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class MovementState : StateBase, ITransition, IAnimation
    {
        private InputBase _input = null;
        private MovementBase _movementBase = null;
        
        #region Transition

        private ITransition _transition = null;
        public List<Transition> Transitions { get; private set; } =  new();
        public bool TryGetTransition(Type to, out Transition targetTransition)
        {
            foreach (var transition in Transitions)
            {
                if (transition.To == to)
                {
                    targetTransition = transition;
                    return true;
                }
            }

            targetTransition = null;
            return false;
        }

        #endregion

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator)
        {
            _input = input;
            _movementBase = movementBase;
            Animator = animator;
            _transition = this;
            
            _transition.AddTransition(typeof(AttackState), () => true, () => false);
            _transition.AddTransition(typeof(AimingState), () => true, () => false);
            _transition.AddTransition(typeof(DodgeState), () => true, () => true);
            _transition.AddTransition(typeof(DeathState), () => true, () => true);
            _transition.AddTransition(typeof(DamageTakenState), () => true, () => true);
        }
        
        public override void EnterState()
        {
            _movementBase.StartMovementAndRotation();
        }

        public override void UpdateState()
        {
            _movementBase.Move(_input.GetMovementInput(), 1F, 1F);
            PlayAnimation();
        }

        public override void ExitState()
        {
            _movementBase.StopMovementAndRotation();
            StopAnimation();
        }

        public override void CancelState()
        {
            _movementBase.StopMovementAndRotation();
            StopAnimation();
        }

        #region Animation

        public int HashCode { get; private set; } = Animator.StringToHash("movement");
        public Animator Animator { get; private set; } = null;
        public void PlayAnimation()
        {
            var value = transform.InverseTransformDirection(_input.GetMovementInput().normalized).magnitude;
            
            Animator.SetFloat(HashCode, value, 0.1f, Time.deltaTime);
        }
        public void StopAnimation()
        {
            Animator.SetFloat(HashCode, 0F, 0.1f, Time.deltaTime);
        }

        #endregion
    }
}