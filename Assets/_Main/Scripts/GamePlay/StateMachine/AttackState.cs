using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class AttackState : StateBase, IAction, ITransition, IAnimationOverridable
    {
        public bool IsAttacking { get; private set; }

        private InputBase _input;
        private MovementBase _movementBase;
        private CharacterBase _character;
        private AttackBase _selectedMeleeAttack;
        public Action OnComplete;

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator, CharacterBase character)
        {
            _input = input;
            _movementBase = movementBase;
            _character = character;
            Animator = animator;
            _transition = this;
            _input.OnAttackActionStarted += OnAttackButtonPressed;
            _input.OnAttackActionEnded += OnAttackButtonReleased;
            _character.OnSelectedMeleeAttackChanged += OnMeleeAttackChanged;

            _transition.AddTransition(typeof(MovementState), () => !IsAttacking, () => false);
            _transition.AddTransition(typeof(DodgeState), () => true, () => true);
            _transition.AddTransition(typeof(DeathState), () => true, () => true);
            _transition.AddTransition(typeof(DamageTakenState), () => true, () => true);
        }

        public override void EnterState()
        {
            _movementBase.StopMovementAndRotation();
        }

        private void OnAttackButtonPressed()
        {
            OnActionStart?.Invoke();
            _movementBase.StartMovementAndRotation();
            transform.rotation = Quaternion.LookRotation(_input.GetLookInput());
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            IsAttacking = false;
            StopAnimation();
            _movementBase.StopMovementAndRotation();
        }

        public override void CancelState()
        {
            OnActionCanceled?.Invoke();
            IsAttacking = false;
            StopAnimation();
            _movementBase.StopMovementAndRotation();
        }

        private void OnAttackPerformed()
        {
            PlayAnimation();
        }

        private void OnAttackCompleted()
        {
            StopAnimation();
            OnComplete?.Invoke();
        }

        private void OnAttackButtonReleased()
        {
            OnActionEnd?.Invoke();
        }

        private void OnMeleeAttackChanged(AttackBase selectedAttack)
        {
            if (_selectedMeleeAttack != null)
            {
                _selectedMeleeAttack.OnAttackPerformed -= OnAttackPerformed;
                _selectedMeleeAttack.OnAttackCompleted -= OnAttackCompleted;
            }
            _selectedMeleeAttack = selectedAttack;
            _selectedMeleeAttack.OnAttackPerformed += OnAttackPerformed;
            _selectedMeleeAttack.OnAttackCompleted += OnAttackCompleted;
        }

        #region Actions

        public event Action OnActionStart;
        public event Action OnActionEnd;
        public event Action OnActionCanceled;

        #endregion

        #region Transition

        private ITransition _transition = null;
        public List<Transition> Transitions { get; private set; } = new();
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

        #region Animation

        public int HashCode { get; private set; } = Animator.StringToHash("attack");
        public Animator Animator { get; private set; } = null;

        public RuntimeAnimatorController OriginalController { get; private set; } = null;

        public void SetAnimatorOverrideController()
        {
            OriginalController = Animator.runtimeAnimatorController;
            Animator.runtimeAnimatorController = _character.SelectedMeleeAttack.CurrentAttackAnimationData.overrideController;
        }

        public void ResetAnimatorController()
        {
            Animator.runtimeAnimatorController = OriginalController;
        }

        public void PlayAnimation()
        {
            SetAnimatorOverrideController();
            Animator.SetTrigger(HashCode);
        }

        public void StopAnimation()
        {
            ResetAnimatorController();
        }

        #endregion
    }
}
