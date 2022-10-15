using System;
using System.Collections.Generic;
using _Main.Scripts.GamePlay.ActionSystem;
using _Main.Scripts.GamePlay.AttackSystem;
using _Main.Scripts.GamePlay.InputSystem;
using _Main.Scripts.GamePlay.MovementSystem;
using DG.Tweening;
using UnityEngine;

namespace _Main.Scripts.GamePlay.StateMachine
{
    public class AttackState : StateBase, IAction, ITransition, IAnimationOverridable
    {
        public bool IsAttacking { get; private set; }

        private InputBase _input;
        private MovementBase _movementBase;
        private AttackControllerBase _attackController;
        private AttackBase _selectedMeleeAttack;
        public Action OnComplete;
        private Tweener attackMovementTween;

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator, AttackControllerBase attackController)
        {
            _input = input;
            _movementBase = movementBase;
            _attackController = attackController;
            Animator = animator;
            _transition = this;
            _attackController.OnSelectedMeleeAttackChanged += OnMeleeAttackChanged;

            _transition.AddTransition(typeof(MovementState), () => !IsAttacking, () => false);
            _transition.AddTransition(typeof(DodgeState), () => true, () => true);
            _transition.AddTransition(typeof(DeathState), () => true, () => true);
            _transition.AddTransition(typeof(DamageTakenState), () => true, () => true);
        }

        public override void EnterState()
        {
            SubscribeToInputActions();
            SubscribeToCurrentAttack();
            _movementBase.StopMovementAndRotation();
            OnAttackButtonPressed();
        }

        private void OnAttackButtonPressed()
        {
            _movementBase.StartMovementAndRotation();
            OnActionStart?.Invoke();
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            attackMovementTween?.Kill();
            UnSubscribeToInputActions();
            UnSubscribeToCurrentAttack();
            IsAttacking = false;
            StopAnimation();
            _movementBase.StopMovementAndRotation();
        }

        public override void CancelState()
        {
            ExitState();
            OnActionCanceled?.Invoke();
        }

        private void OnAttackPerformed()
        {
            attackMovementTween?.Kill();
            PlayAnimation();
            transform.rotation = Quaternion.LookRotation(_input.GetLookInput());
            var info = (MeleeAttackAnimationData)_selectedMeleeAttack.CurrentAttackAnimationData;
            //attackMovementTween = transform.DOMove(transform.forward * info.attackMovementAmount, info.attackMovementDuration).SetRelative().SetEase(Ease.Linear).SetDelay(info.attackMovementDelay);
            _movementBase.MoveOverTime(transform.position + transform.forward * info.attackMovementAmount, info.attackMovementDuration, info.attackMovementDelay, info.useGravity, info.useAnimationMovement);
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
            UnSubscribeToCurrentAttack();
            _selectedMeleeAttack = selectedAttack;
            SubscribeToCurrentAttack();
        }

        private void SubscribeToCurrentAttack()
        {
            _selectedMeleeAttack.OnAttackPerformed += OnAttackPerformed;
            _selectedMeleeAttack.OnAttackCompleted += OnAttackCompleted;
        }

        private void UnSubscribeToCurrentAttack()
        {
            if (_selectedMeleeAttack != null)
            {
                _selectedMeleeAttack.OnAttackPerformed -= OnAttackPerformed;
                _selectedMeleeAttack.OnAttackCompleted -= OnAttackCompleted;
            }
        }

        private void SubscribeToInputActions()
        {
            _input.OnAttackActionStarted += OnAttackButtonPressed;
            _input.OnAttackActionEnded += OnAttackButtonReleased;
        }

        private void UnSubscribeToInputActions()
        {
            _input.OnAttackActionStarted -= OnAttackButtonPressed;
            _input.OnAttackActionEnded -= OnAttackButtonReleased;
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

        public int HashCode { get; private set; } = Animator.StringToHash("Locomotion");
        public int SpeedMultHashCode { get; private set; } = Animator.StringToHash("AnimationSpeedMultiplier");
        public Animator Animator { get; private set; } = null;

        public RuntimeAnimatorController OriginalController { get; private set; } = null;

        public void SetAnimatorOverrideController()
        {
            OriginalController = Animator.runtimeAnimatorController;
            Animator.runtimeAnimatorController = _attackController.SelectedMeleeAttack.CurrentAttackAnimationData.overrideController;
            var animData = (MeleeAttackAnimationData)_selectedMeleeAttack.CurrentAttackAnimationData;
            Animator.SetFloat(SpeedMultHashCode, animData.animationSpeedMultiplier);
        }

        public void ResetAnimatorController()
        {
            Animator.runtimeAnimatorController = OriginalController;
        }

        public void PlayAnimation()
        {
            SetAnimatorOverrideController();
            var info = (MeleeAttackAnimationData)_selectedMeleeAttack.CurrentAttackAnimationData;
            Animator.CrossFadeInFixedTime(info.fadeToAnimationName, 0.05f);
        }

        public void StopAnimation()
        {
            Animator.CrossFadeInFixedTime(HashCode, 0.1f);
            DOVirtual.DelayedCall(0.15f, ResetAnimatorController);
        }

        #endregion
    }
}
