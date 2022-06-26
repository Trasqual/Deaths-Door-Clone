using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class DodgeState : StateBase, ITransition, IAnimation
    {
        private InputBase _input = null;
        private Movement _movement = null;
        private float _speedMultiplier = 0F;
        private float _duration = 0F;
        private Vector3 _direction;
        private bool _isDodgeComplete = false;
        public Action OnComplete;

        public void Initialize(InputBase inputBase, Movement movement, Animator animator, float speedMultiplier, float duration)
        {
            _input = inputBase;
            _movement = movement;
            Animator = animator;
            _speedMultiplier = speedMultiplier;
            _duration = duration;
            _transition = this;
            
            _transition.AddTransition(typeof(AimingState), () => _isDodgeComplete, () => false);
            _transition.AddTransition(typeof(MovementState), () => _isDodgeComplete, () => false);
            _transition.AddTransition(typeof(AttackState), () => _isDodgeComplete, () => false);
        }


        public override void EnterState()
        {
            _movement.StartMovementAndRotation();
            PlayAnimation();
            _direction = _input.GetMovementInput() == Vector3.zero ? transform.forward : _input.GetMovementInput().normalized;
            transform.forward = _direction;
            _isDodgeComplete = false;
            DOVirtual.DelayedCall(_duration, OnDodgeComplete);
        }

        private void OnDodgeComplete()
        {
            _isDodgeComplete = true;
            OnComplete?.Invoke();
        }

        public override void UpdateState()
        {
            _movement.Move(_direction, _speedMultiplier, 0f);
        }
        public override void ExitState()
        {
            _movement.StopMovementAndRotation();
            _direction = Vector3.zero;
        }
        public override void CancelState()
        {
            // no-op
        }
        
        #region Animation

        public int HashCode { get; private set; } = Animator.StringToHash("roll");
        public Animator Animator { get; private set; } = null;
        
        public void PlayAnimation()
        {
            Animator.SetTrigger(HashCode);
        }

        public void StopAnimation()
        {
            // no-op
        }
        
        #endregion
        
        #region Transition

        private ITransition _transition = null;
        public List<Transition> Transitions { get; private set; } =  new();

        #endregion
    }
}