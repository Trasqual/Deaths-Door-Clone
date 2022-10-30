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
    public class MeleeAttackState : StateBase, IAction, ITransition, IAnimationOverridable
    {
        public Action OnComplete;

        public bool IsAttacking { get; private set; }
        public bool IsStateLocked { get; private set; }

        private Tween _stateLockTween;

        private InputBase _input;
        private MovementBase _movementBase;
        private AttackControllerBase _attackController;
        private AttackBase _selectedMeleeAttack;

        private bool IsOnAttackCooldown;

        public void Initialize(InputBase input, MovementBase movementBase, Animator animator, AttackControllerBase attackController)
        {
            _input = input;
            _movementBase = movementBase;
            _attackController = attackController;
            Animator = animator;
            _transition = this;
            _attackController.OnSelectedMeleeAttackChanged += OnMeleeAttackChanged;
            OriginalController = Animator.runtimeAnimatorController;

            _transition.AddTransition(typeof(MovementState), () => !IsAttacking, () => false);
            _transition.AddTransition(typeof(DodgeState), () => !IsStateLocked, () => true);
            _transition.AddTransition(typeof(DeathState), () => true, () => true);
            _transition.AddTransition(typeof(DamageTakenState), () => true, () => true);
        }

        #region StateMethods
        public override void EnterState()
        {
            if (IsOnAttackCooldown)
            {
                OnComplete?.Invoke();
                return;
            }
            SubscribeToInputActions();
            SubscribeToCurrentAttack();
            _movementBase.StopMovementAndRotation();
            OnAttackButtonPressed();
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
            UnSubscribeToInputActions();
            UnSubscribeToCurrentAttack();
            IsAttacking = false;
            _movementBase.StopMovementAndRotation();
            if (!IsOnAttackCooldown)
            {
                IsOnAttackCooldown = true;
                var meleeAttack = (MeleeAttackBase)_selectedMeleeAttack;
                DOVirtual.DelayedCall(meleeAttack.GeneralAttackCooldown, () => IsOnAttackCooldown = false);
                StopAnimation();
            }
        }

        public override void CancelState()
        {
            UnSubscribeToInputActions();
            UnSubscribeToCurrentAttack();
            ResetAnimatorController();  
            IsAttacking = false;
            _movementBase.StopMovementAndRotation();
            StopAnimation();
            OnActionCanceled?.Invoke();
        }
        #endregion

        #region InputCallbacks
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

        private void OnAttackButtonPressed()
        {
            _movementBase.StartMovementAndRotation();
            OnActionStart?.Invoke();
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

        #endregion

        #region CurrentAttackCallbacks

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

        private void OnAttackPerformed()
        {
            _stateLockTween?.Kill();
            IsStateLocked = false;

            PlayAnimation();
            transform.rotation = Quaternion.LookRotation(_input.GetLookInput());
            var info = (MeleeAttackAnimationData)_selectedMeleeAttack.CurrentComboAnimationData;
            _movementBase.MoveOverTime(transform.position + transform.forward * info.attackMovementAmount, info.attackMovementDuration, info.attackMovementDelay, info.useGravity, info.useAnimationMovement);

            IsStateLocked = info.useAnimationMovement;
            _stateLockTween = DOVirtual.DelayedCall(info.attackCD, () => IsStateLocked = false);
        }

        private void OnAttackCompleted()
        {
            OnComplete?.Invoke();
        }

        #endregion

        #region Actions(IAction)

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
            Animator.runtimeAnimatorController = _attackController.SelectedMeleeAttack.CurrentComboAnimationData.OverrideController;
            var animData = (MeleeAttackAnimationData)_selectedMeleeAttack.CurrentComboAnimationData;
            Animator.SetFloat(SpeedMultHashCode, animData.animationSpeedMultiplier);
        }

        public void ResetAnimatorController()
        {
            Animator.runtimeAnimatorController = OriginalController;
        }

        public void PlayAnimation()
        {
            SetAnimatorOverrideController();
            var info = (MeleeAttackAnimationData)_selectedMeleeAttack.CurrentComboAnimationData;
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
