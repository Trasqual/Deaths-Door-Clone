using System.Collections.Generic;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class MovementState : StateBase, ITransition, IAnimation
    {
        private InputBase _input = null;
        private Movement _movement = null;
        
        #region Transition

        private ITransition _transition = null;
        public List<Transition> Transitions { get; private set; } =  new();

        #endregion

        public void Initialize(InputBase input, Movement movement, Animator animator)
        {
            _input = input;
            _movement = movement;
            Animator = animator;
            _transition = this;
            
            _transition.AddTransition(typeof(AttackState), () => true, () => true);
            _transition.AddTransition(typeof(AimingState), () => true, () => false);
        }
        
        public override void EnterState()
        {
            _movement.StartMovementAndRotation();
        }

        public override void UpdateState()
        {
            _movement.Move(_input.GetMovementInput(), 1F, 1F);
            PlayAnimation();
        }

        public override void ExitState()
        {
            _movement.StopMovementAndRotation();
            StopAnimation();
        }

        public override void CancelState()
        {
            _movement.StopMovementAndRotation();
            StopAnimation();
        }

        #region Animation

        public int HashCode { get; private set; } = Animator.StringToHash("movement");
        public Animator Animator { get; private set; } = null;
        public void PlayAnimation()
        {
            var value = transform.InverseTransformDirection(_input.GetMovementInput()).magnitude;
            
            Animator.SetFloat(HashCode, value, 0.1f, Time.deltaTime);
        }
        public void StopAnimation()
        {
            Animator.SetFloat(HashCode, 0F, 0.1f, Time.deltaTime);
        }

        #endregion
    }
}