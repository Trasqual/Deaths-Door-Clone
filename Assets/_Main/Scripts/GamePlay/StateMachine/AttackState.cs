using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
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
        public Action OnComplete;

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator, CharacterBase character)
        {
            _input = input;
            _movementBase = movementBase;
            _character = character;
            Animator = animator;
            _transition = this;
            _input.OnAttackActionStarted += OnAttackStart;
            _input.OnAttackActionEnded += ActionEnd;

            _transition.AddTransition(typeof(MovementState), () => !IsAttacking, () => false);
            _transition.AddTransition(typeof(DodgeState), () => true, () => true);
            _transition.AddTransition(typeof(DeathState), () => true, () => true);
            _transition.AddTransition(typeof(DamageTakenState), () => true, () => true);
        }

        public override void EnterState()
        {

        }

        private void OnAttackStart()
        {
            OnActionStart?.Invoke();
            _movementBase.StartMovementAndRotation();
            _movementBase.Move(_input.GetLookInput(), 0f, 1f);
            _character.SelectedMeleeAttack.OnAttackEnded += OnAttackEnd;
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            _movementBase.StopMovementAndRotation();
            _character.SelectedMeleeAttack.OnAttackEnded -= OnAttackEnd;
        }

        public override void CancelState()
        {
            OnActionCanceled?.Invoke();
            IsAttacking = false;
            StopAnimation();
            _movementBase.StopMovementAndRotation();
            _character.SelectedMeleeAttack.OnAttackEnded -= OnAttackEnd;
        }

        private void OnAttackEnd()
        {
            IsAttacking = false;
            OnComplete?.Invoke();
        }

        private void ActionEnd()
        {
            OnActionEnd?.Invoke();
            PlayAnimation();
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
            Animator.SetBool(HashCode, true);
        }

        public void StopAnimation()
        {
            ResetAnimatorController();
            Animator.SetBool(HashCode, false);
        }

        #endregion
    }
}
